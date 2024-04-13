using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ���� �ý��� ����
/// </summary>
public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;  //�̱��� �ν��Ͻ�

    public const int MAX_TOWER_LEVEL = 4;  //Ÿ���� �ִ� ����
    public const int MAX_GAME_ROUND = 25;  //�ִ� ���� ����
    public const int MAX_GAME_HP = 100;  //�ִ� ���� ü��
    public const float TOWER_ATTACKRANGE_SIZE = 10f;  //Ÿ���� ���� ��Ÿ��� ����� �̹����� ǥ��

    private UI_GameEnd _gameoverUI;  //���� ����� ȣ���� UI
    private ParticleSystem _summonEffect;  //�ֳʹ� ���� �� play�� ����Ʈ
    private Define.GameState _gameState = Define.GameState.Play;  //������ ����
    private Data.GameSystemData _saveData;  //���� ������

    public Action<int> OnGoldEvent;  //��� ȹ�� �̺�Ʈ
    public Action<int> OnGameRoundEvent;  //���� ���� ��� �̺�Ʈ
    public Action<int> OnGameHpEvent;  //���� ü�� ���� �̺�Ʈ
    public Action<int> OnScoreEvent;  //���� ȹ�� �̺�Ʈ
    public Action<int> OnTimeEvent;  //���� �ð� ���� �̺�Ʈ
    public Action OnStartEvent;  //���� ���� �̺�Ʈ

    private int _currentGameScore;  //���� ���� ����
    private int _currentGameHp;  //���� ���� ü��
    private int _currentGameRound = 0;  //���� ���� ����
    private float _currentTime;  //���� ���� �ð�
    private float _maxTime = 60f;  //�ִ� ���� �ð�
    private int _currentGold = 400;  //���� ���� ���
    private int _objectHandle = 0;  //��ȯ ���ֿ��� �ο��� �ڵ�
    //todo �ڵ� ��ġ�� �ʰ� �ʱ�ȭ�ؾߵ�

    public int CurrentGold { get { return _currentGold; } set { _currentGold = value; } }
    public int GameRound => _currentGameRound;
    public int GameScore => _currentGameScore;
    public int GameHp => _currentGameHp;
    public Define.GameState GameState => _gameState;

    private Dictionary<int, ConData> _conObjects = new Dictionary<int, ConData>();  //���� ������ ��� tower con
    private Dictionary<int, TowerData> _towerObjects = new Dictionary<int, TowerData>();  //���� ������ ��� tower
    public Dictionary<int, ConData> ConObjects => _conObjects;
    public Dictionary<int, TowerData> TowerObjects => _towerObjects;

    /// <summary>
    /// ��� ������Ʈ ����� Ŭ����
    /// </summary>
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

    /// <summary>
    /// ��� tower con������ ����� Ŭ����
    /// </summary>
    [System.Serializable]
    public class ConData : ObjectDatas {
        public ConData() { }
        public ConData(Transform trans, string name, Define.TowerType type, int level, int kill) : base(trans, name, level) {
            Type = (int)type;
            KillNumber = kill;
        }
        public int KillNumber;
    }

    /// <summary>
    /// ��� tower ������ ����� Ŭ����
    /// </summary>
    [System.Serializable]
    public class TowerData : ObjectDatas {
        public TowerData() { }
        public TowerData(Transform trans, string name, int kill, Define.TowerType type, int level) : base(trans, name, level) {
            KillNumber = kill;
            Type = (int)type;
        }
        public int KillNumber;
    }

    private void Awake() {
        Instance = this;
        _gameoverUI = GameObject.Find("UI_GameOver").GetComponent<UI_GameEnd>();
    }

    private void Start() {
        _summonEffect = GetComponentInChildren<ParticleSystem>();
    }

    public void Init() {
        _currentGameHp = MAX_GAME_HP;
    }

    /// <summary>
    /// �̾��ϱ⸦ ���� ��, ���� ���� ȣ��
    /// </summary>
    public void Continue() {
        _saveData = Managers.Data.GetSaveData();
        _currentGameHp = _saveData.CurrentHp;
        _currentGameScore = _saveData.CurrentScore;
        _currentGold = _saveData.CurrentGold;
        _currentGameRound = _saveData.CurrentRound;
    }
  
    /// <summary>
    /// ��ư�� ���� ���� ���� ����
    /// </summary>
    public void GameStart() {
        _currentGameRound++;

        if (_currentGameRound > MAX_GAME_ROUND) {  //�ִ� ������� Ŭ���� ���� ��
            _gameoverUI.gameObject.SetActive(true);
            _gameoverUI.SetGameEndUI(_currentGameRound, _currentGameScore, true);
            _gameState = Define.GameState.GameOver;
            Managers.Audio.SetBgm(false);
            Managers.Audio.PlaySfx(Define.SfxType.Victory);
            return;
        }

        OnGameRoundEvent?.Invoke(_currentGameRound); //���� ���� ���� �̺�Ʈ ȣ��
        _currentTime = 0f;
        Managers.Spawn.SpawnEnemy(_currentGameRound, _summonEffect);  //�ֳʹ� ��ȯ
    }

    /// <summary>
    /// ���ѽð��� ������ ���� ���� ����
    /// </summary>
    private void Update() {
        if (!IsPlay())  //���� ���� �� ����
            return;

        _currentTime += Time.deltaTime;
        OnTimeEvent?.Invoke((int)_maxTime - (int)_currentTime);

        if(_currentTime * 3f > _maxTime &&
            _currentGameRound < MAX_GAME_ROUND) {
            OnStartEvent?.Invoke();
        }

        if (_currentTime < _maxTime)
            return;

        _currentGameRound++;

        if(_currentGameRound > MAX_GAME_ROUND) {  //�ִ� ������� Ŭ���� ��
            _gameoverUI.gameObject.SetActive(true);
            _gameoverUI.SetGameEndUI(_currentGameRound, _currentGameScore, true);
            _gameState = Define.GameState.GameOver;
            Managers.Audio.SetBgm(false);
            Managers.Audio.PlaySfx(Define.SfxType.Victory);
            return;
        }

        OnGameRoundEvent?.Invoke(_currentGameRound);  //���� ���� ���� �̺�Ʈ ȣ��
        _currentTime = 0f;
        Managers.Spawn.SpawnEnemy(_currentGameRound, _summonEffect);  //�ֳʹ� ��ȯ
    }

    /// <summary>
    /// ���� ������
    /// </summary>
    /// <returns></returns>
    public bool IsPlay() => _gameState == Define.GameState.Play;

    /// <summary>
    /// ���� ���� ��
    /// </summary>
    /// <param name="gold">���� ���</param>
    /// <returns></returns>
    public bool EnoughGold(int gold) => gold <= _currentGold;
 
    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="score"></param>
    public void SetScore(int score) {
        _currentGameScore += score;
        OnScoreEvent?.Invoke(_currentGameScore);
    }

    /// <summary>
    /// ��� ����
    /// </summary>
    /// <param name="gold"></param>
    public void SetGold(int gold) {
        _currentGold += gold;
        OnGoldEvent?.Invoke(_currentGold);  //��� �̺�Ʈ �ߵ�
        if (CurrentGold < 0)
            CurrentGold = 0;
    }

    /// <summary>
    /// ���� ü�� ����
    /// </summary>
    /// <param name="value"></param>
    public void SetGameHp(int value) {
        _currentGameHp += value;
        OnGameHpEvent?.Invoke(_currentGameHp);  //ü�� �̺�Ʈ �ߵ�

        if(_currentGameHp <= 0) {  //ü���� 0���� ������ �� ���� ����
            _gameoverUI.gameObject.SetActive(true);
            _gameoverUI.SetGameEndUI(_currentGameRound, _currentGameScore, false);
            _gameState = Define.GameState.GameOver;
            Managers.Audio.SetBgm(false);
            Managers.Audio.PlaySfx(Define.SfxType.Lose);
        }
    }

    /// <summary>
    /// �ֳʹ̿��� �ο��� �ڵ� ����
    /// </summary>
    /// <param name="handle">������ �ڵ�</param>
    public void SetObjectHandle(int handle) => _objectHandle = handle;

    /// <summary>
    /// Ÿ���� ���� ��, ��ųʸ� ��Ͽ� �߰�
    /// </summary>
    /// <param name="go">�߰��� Ÿ��</param>
    /// <param name="type">�߰��� Ÿ���� Ÿ��</param>
    /// <param name="level">Ÿ���� ����</param>
    /// <returns>Ÿ���� �ڵ�</returns>
    public int AddTowerObject(TowerBase go, Define.TowerType type, int level) {
        while(_towerObjects.ContainsKey(_objectHandle)) {
            _objectHandle++;  //�ڵ� ��� ����
        }
        TowerData data = new TowerData(go.transform, go.gameObject.name, go.TowerStatus.KillNumber, type, level);  //������ Ÿ���� ���� ����
        _towerObjects.Add(_objectHandle, data);  //��Ͽ� �ڵ�� Ÿ�� ������ �߰�
        return _objectHandle;
    }

    /// <summary>
    /// Ÿ���� con ���� ��, ��ųʸ� ��Ͽ� �߰�
    /// </summary>
    /// <param name="go">�߰��� Ÿ�� con</param>
    /// <param name="type">�߰��� Ÿ��con�� Ÿ��</param>
    /// <param name="level">Ÿ��con�� ����</param>
    /// <returns>Ÿ��con�� �ڵ�</returns>
    public int AddConObject(ConBase go, Define.TowerType type, int level, int kill) {
        while (_conObjects.ContainsKey(_objectHandle)) {
            _objectHandle++;  //�ڵ� ��� ����
        }
        ConData data = new ConData(go.transform, go.gameObject.name, type, level, kill);
        _conObjects.Add(_objectHandle, data);
        return _objectHandle;
    }

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    public void SaveGameData() {
        int round = GameRound - 1;
        int gold = CurrentGold;
        int hp = GameHp;
        int score = GameScore;
        var conData = ConObjects;
        var towerData = TowerObjects;

        foreach (var tower in towerData)   //���� ��ġ�� ����
            tower.Value.SetPosition();
        foreach (var con in conData) 
            con.Value.SetPosition();

        var savedata = new Data.GameSystemData(round, gold, hp, score, conData, towerData);  //������ ����

        string gamedata = JsonConvert.SerializeObject(savedata, Formatting.Indented);
        Managers.Data.SaveJson(Application.persistentDataPath, "SaveData", gamedata);  //Json���� ����
    }

    public void RemoveTowerObject(int handle) => _towerObjects.Remove(handle);  //Ÿ�� ���Ž� ������ ����
    public void RemoveConObject(int handle) => _conObjects.Remove(handle);
}
