using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;
    private UI_GameOver _gameoverUI;
    private ParticleSystem _summonEffect;
    private Define.GameState _gameState = Define.GameState.Play;
    public const int EnemyMaxLevel = 6;
    public const int TowerMaxLevel = 4;
    public const float TowerAttackRangeImageSize = 10f;
    public const int MaxGameLevel = 25;
    public const int MaxGameHp = 100;

    private int _currentGameScore;
    private int _currentGameHp;
    private int _gameLevel = 0;
    private float _currentTime;
    private float _maxTime = 60f;
    private int _currentGold = 350;

    public Action<int> OnGoldEvent;
    public Action<int> OnGameLevelEvent;
    public Action<int> OnGameHpEvent;
    public Action<int> OnScoreEvent;
    public Action<int> OnTimeEvent;
    public Action OnStartEvent;

    private Data.GameSystemData _saveData;
    public int GameLevel => _gameLevel;
    public int GameScore => _currentGameScore;
    public int GameHp => _currentGameHp;
    public Define.GameState GameState => _gameState;

    private int _objectHandle = 0;

    private Dictionary<int, ConData> _conObjects = new Dictionary<int, ConData>();
    private Dictionary<int, TowerData> _towerObjects = new Dictionary<int, TowerData>();
    
    public Dictionary<int, ConData> ConObjects => _conObjects;
    public Dictionary<int, TowerData> TowerObjects => _towerObjects;

    [System.Serializable]
    public class ObjectDatas {
        public ObjectDatas(Transform trans, string name, int level) {
            Name = name;
            Level = level;
            Trans = trans;
        }
        public ObjectDatas() { }
        public string Name;
        public float PosX;
        public float PosZ;
        public int Type;
        public int Level;

        [JsonIgnore]
        public Transform Trans;

        public void SetPosition() {
            PosX = Trans.position.x;
            PosZ = Trans.position.z;
        }
    }

    [System.Serializable]
    public class ConData : ObjectDatas {
        public ConData() { }
        public ConData(Transform trans, string name, Define.TowerType type, int level, int kill) : base(trans, name, level) {
            Type = (int)type;
            KillNumber = kill;
        }
        public int KillNumber;
    }

    [System.Serializable]
    public class TowerData : ObjectDatas {
        public TowerData() { }
        public TowerData(Transform trans, string name, int kill, Define.TowerType type, int level) : base(trans, name, level) {
            KillNumber = kill;
            Type = (int)type;
        }
        public int KillNumber;
    }

    public int CurrentGold { get { return _currentGold; } set { _currentGold = value; } }
    private void Awake() {
        Instance = this;
        _gameoverUI = GameObject.Find("UI_GameOver").GetComponent<UI_GameOver>();
    }

    private void Start() {
        _summonEffect = GetComponentInChildren<ParticleSystem>();
    }

    public void Init() {
        _currentGameHp = MaxGameHp;
    }

    public void Continue() {
        _saveData = Managers.Data.GetSaveData();
        _currentGameHp = _saveData.CurrentHp;
        _currentGameScore = _saveData.CurrentScore;
        _currentGold = _saveData.CurrentGold;
        _gameLevel = _saveData.CurrentRound;
    }
  
    public void GameStart() {
        _gameLevel++;

        if (_gameLevel > MaxGameLevel) {
            _gameoverUI.gameObject.SetActive(true);
            _gameoverUI.SetGameOverUI(_gameLevel, _currentGameScore, true);
            _gameState = Define.GameState.GameOver;
            return;
        }

        OnGameLevelEvent?.Invoke(_gameLevel);
        _currentTime = 0f;
        Managers.Spawn.SpawnEnemy(_gameLevel, _summonEffect);
    }
    private void Update() {
        if (!IsPlay())
            return;

        _currentTime += Time.deltaTime;
        OnTimeEvent?.Invoke((int)_maxTime - (int)_currentTime);

        if(_currentTime * 3f > _maxTime &&
            _gameLevel < MaxGameLevel) {
            OnStartEvent?.Invoke();
        }

        if (_currentTime < _maxTime)
            return;

        _gameLevel++;

        if(_gameLevel > MaxGameLevel) {
            _gameoverUI.gameObject.SetActive(true);
            _gameoverUI.SetGameOverUI(_gameLevel, _currentGameScore, true);
            _gameState = Define.GameState.GameOver;
            Managers.Audio.PlayBgm(false);
            Managers.Audio.PlaySfx(Define.SfxType.Victory);
            return;
        }

        OnGameLevelEvent?.Invoke(_gameLevel);
        _currentTime = 0f;
        Managers.Spawn.SpawnEnemy(_gameLevel, _summonEffect);
    }

    public bool IsPlay() {
        return _gameState == Define.GameState.Play;
    }

    public bool EnoughGold(int gold) {
        return gold <= _currentGold;
    }


    public void SetScore(int score) {
        _currentGameScore += score;
        OnScoreEvent?.Invoke(_currentGameScore);
    }

    public void SetGold(int gold) {
        _currentGold += gold;
        OnGoldEvent?.Invoke(_currentGold);
        if (CurrentGold < 0)
            CurrentGold = 0;
    }

    public void SetGameHp(int value) {
        _currentGameHp += value;
        OnGameHpEvent?.Invoke(_currentGameHp);

        if(_currentGameHp <= 0) {
            _gameoverUI.gameObject.SetActive(true);
            _gameoverUI.SetGameOverUI(_gameLevel, _currentGameScore, false);
            _gameState = Define.GameState.GameOver;
            Managers.Audio.PlayBgm(false);
            Managers.Audio.PlaySfx(Define.SfxType.Lose);
        }
    }

    public void SetObjectHandle(int handle) {
        _objectHandle = handle;
    }

    public int AddTowerObject(TowerBase go, Define.TowerType type, int level) {
        _objectHandle++;
        TowerData data = new TowerData(go.transform, go.gameObject.name, go.TowerStatus.KillNumber, type, level);
        _towerObjects.Add(_objectHandle, data);
        return _objectHandle;
    }

    public int AddConObject(BuildingTower go, Define.TowerType type, int level, int kill) {
        _objectHandle++;
        ConData data = new ConData(go.transform, go.gameObject.name, type, level, kill);
        _conObjects.Add(_objectHandle, data);
        return _objectHandle;
    }

    public void SaveGameData() {
        int round = GameLevel - 1;
        int gold = CurrentGold;
        int hp = GameHp;
        int score = GameScore;
        var conData = ConObjects;
        var towerData = TowerObjects;

        foreach (var tower in towerData) {
            tower.Value.SetPosition();
        }
        foreach (var con in conData) {
            con.Value.SetPosition();
        }
        var savedata = new Data.GameSystemData(round, gold, hp, score, conData, towerData);

        string gamedata = JsonConvert.SerializeObject(savedata, Formatting.Indented);
        Managers.Data.SaveJson(Application.persistentDataPath, "SaveData", gamedata);
    }

    public void RemoveTowerObject(int handle) {
        _towerObjects.Remove(handle);
    }
    public void RemoveConObject(int handle) {
        _conObjects.Remove(handle);
    }

}
