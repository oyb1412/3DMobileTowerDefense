using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 게임 시스템 관리
/// </summary>
public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;  //싱글턴 인스턴스

    public const int MAX_TOWER_LEVEL = 4;  //타워의 최대 레벨
    public const int MAX_GAME_ROUND = 25;  //최대 게임 라운드
    public const int MAX_GAME_HP = 100;  //최대 게임 체력
    public const float TOWER_ATTACKRANGE_SIZE = 10f;  //타워의 공격 사거리에 비례해 이미지를 표기

    private UI_GameEnd _gameoverUI;  //게임 종료시 호출할 UI
    private ParticleSystem _summonEffect;  //애너미 생성 시 play할 이펙트
    private Define.GameState _gameState = Define.GameState.Play;  //게임의 상태
    private Data.GameSystemData _saveData;  //저장 데이터

    public Action<int> OnGoldEvent;  //골드 획득 이벤트
    public Action<int> OnGameRoundEvent;  //게임 라운드 상승 이벤트
    public Action<int> OnGameHpEvent;  //게임 체력 변동 이벤트
    public Action<int> OnScoreEvent;  //점수 획득 이벤트
    public Action<int> OnTimeEvent;  //게임 시간 변동 이벤트
    public Action OnStartEvent;  //라운드 시작 이벤트

    private int _currentGameScore;  //현재 게임 점수
    private int _currentGameHp;  //현재 게임 체력
    private int _currentGameRound = 0;  //현재 게임 라운드
    private float _currentTime;  //현재 게임 시간
    private float _maxTime = 60f;  //최대 게임 시간
    private int _currentGold = 400;  //현재 보유 골드
    private int _objectHandle = 0;  //소환 유닛에게 부여할 핸들
    //todo 핸들 곂치지 않게 초기화해야됨

    public int CurrentGold { get { return _currentGold; } set { _currentGold = value; } }
    public int GameRound => _currentGameRound;
    public int GameScore => _currentGameScore;
    public int GameHp => _currentGameHp;
    public Define.GameState GameState => _gameState;

    private Dictionary<int, ConData> _conObjects = new Dictionary<int, ConData>();  //내가 생성한 모든 tower con
    private Dictionary<int, TowerData> _towerObjects = new Dictionary<int, TowerData>();  //내가 생성한 모든 tower
    public Dictionary<int, ConData> ConObjects => _conObjects;
    public Dictionary<int, TowerData> TowerObjects => _towerObjects;

    /// <summary>
    /// 모든 오브젝트 저장용 클래스
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
    /// 모든 tower con데이터 저장용 클래스
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
    /// 모든 tower 데이터 저장용 클래스
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
    /// 이어하기를 했을 시, 게임 정보 호출
    /// </summary>
    public void Continue() {
        _saveData = Managers.Data.GetSaveData();
        _currentGameHp = _saveData.CurrentHp;
        _currentGameScore = _saveData.CurrentScore;
        _currentGold = _saveData.CurrentGold;
        _currentGameRound = _saveData.CurrentRound;
    }
  
    /// <summary>
    /// 버튼을 눌러 다음 라운드 시작
    /// </summary>
    public void GameStart() {
        _currentGameRound++;

        if (_currentGameRound > MAX_GAME_ROUND) {  //최대 라운드까지 클리어 했을 시
            _gameoverUI.gameObject.SetActive(true);
            _gameoverUI.SetGameEndUI(_currentGameRound, _currentGameScore, true);
            _gameState = Define.GameState.GameOver;
            Managers.Audio.SetBgm(false);
            Managers.Audio.PlaySfx(Define.SfxType.Victory);
            return;
        }

        OnGameRoundEvent?.Invoke(_currentGameRound); //다음 라운드 시작 이벤트 호출
        _currentTime = 0f;
        Managers.Spawn.SpawnEnemy(_currentGameRound, _summonEffect);  //애너미 소환
    }

    /// <summary>
    /// 제한시간이 지나면 다음 라운드 시작
    /// </summary>
    private void Update() {
        if (!IsPlay())  //게임 종료 시 정지
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

        if(_currentGameRound > MAX_GAME_ROUND) {  //최대 라운드까지 클리어 시
            _gameoverUI.gameObject.SetActive(true);
            _gameoverUI.SetGameEndUI(_currentGameRound, _currentGameScore, true);
            _gameState = Define.GameState.GameOver;
            Managers.Audio.SetBgm(false);
            Managers.Audio.PlaySfx(Define.SfxType.Victory);
            return;
        }

        OnGameRoundEvent?.Invoke(_currentGameRound);  //다음 라운드 시작 이벤트 호출
        _currentTime = 0f;
        Managers.Spawn.SpawnEnemy(_currentGameRound, _summonEffect);  //애너미 소환
    }

    /// <summary>
    /// 게임 진행중
    /// </summary>
    /// <returns></returns>
    public bool IsPlay() => _gameState == Define.GameState.Play;

    /// <summary>
    /// 보유 골드와 비교
    /// </summary>
    /// <param name="gold">비교할 골드</param>
    /// <returns></returns>
    public bool EnoughGold(int gold) => gold <= _currentGold;
 
    /// <summary>
    /// 점수 설정
    /// </summary>
    /// <param name="score"></param>
    public void SetScore(int score) {
        _currentGameScore += score;
        OnScoreEvent?.Invoke(_currentGameScore);
    }

    /// <summary>
    /// 골드 설정
    /// </summary>
    /// <param name="gold"></param>
    public void SetGold(int gold) {
        _currentGold += gold;
        OnGoldEvent?.Invoke(_currentGold);  //골드 이벤트 발동
        if (CurrentGold < 0)
            CurrentGold = 0;
    }

    /// <summary>
    /// 게임 체력 설정
    /// </summary>
    /// <param name="value"></param>
    public void SetGameHp(int value) {
        _currentGameHp += value;
        OnGameHpEvent?.Invoke(_currentGameHp);  //체력 이벤트 발동

        if(_currentGameHp <= 0) {  //체력이 0보다 적어질 시 게임 종료
            _gameoverUI.gameObject.SetActive(true);
            _gameoverUI.SetGameEndUI(_currentGameRound, _currentGameScore, false);
            _gameState = Define.GameState.GameOver;
            Managers.Audio.SetBgm(false);
            Managers.Audio.PlaySfx(Define.SfxType.Lose);
        }
    }

    /// <summary>
    /// 애너미에게 부여할 핸들 설정
    /// </summary>
    /// <param name="handle">보유한 핸들</param>
    public void SetObjectHandle(int handle) => _objectHandle = handle;

    /// <summary>
    /// 타워를 생성 후, 딕셔너리 목록에 추가
    /// </summary>
    /// <param name="go">추가할 타워</param>
    /// <param name="type">추가할 타워의 타입</param>
    /// <param name="level">타워의 레벨</param>
    /// <returns>타워의 핸들</returns>
    public int AddTowerObject(TowerBase go, Define.TowerType type, int level) {
        while(_towerObjects.ContainsKey(_objectHandle)) {
            _objectHandle++;  //핸들 밸류 증가
        }
        TowerData data = new TowerData(go.transform, go.gameObject.name, go.TowerStatus.KillNumber, type, level);  //생성한 타워의 정보 생성
        _towerObjects.Add(_objectHandle, data);  //목록에 핸들과 타워 데이터 추가
        return _objectHandle;
    }

    /// <summary>
    /// 타워의 con 생성 후, 딕셔너리 목록에 추가
    /// </summary>
    /// <param name="go">추가할 타워 con</param>
    /// <param name="type">추가할 타워con의 타입</param>
    /// <param name="level">타워con의 레벨</param>
    /// <returns>타워con의 핸들</returns>
    public int AddConObject(ConBase go, Define.TowerType type, int level, int kill) {
        while (_conObjects.ContainsKey(_objectHandle)) {
            _objectHandle++;  //핸들 밸류 증가
        }
        ConData data = new ConData(go.transform, go.gameObject.name, type, level, kill);
        _conObjects.Add(_objectHandle, data);
        return _objectHandle;
    }

    /// <summary>
    /// 게임 데이터 저장
    /// </summary>
    public void SaveGameData() {
        int round = GameRound - 1;
        int gold = CurrentGold;
        int hp = GameHp;
        int score = GameScore;
        var conData = ConObjects;
        var towerData = TowerObjects;

        foreach (var tower in towerData)   //현재 위치를 저장
            tower.Value.SetPosition();
        foreach (var con in conData) 
            con.Value.SetPosition();

        var savedata = new Data.GameSystemData(round, gold, hp, score, conData, towerData);  //데이터 생성

        string gamedata = JsonConvert.SerializeObject(savedata, Formatting.Indented);
        Managers.Data.SaveJson(Application.persistentDataPath, "SaveData", gamedata);  //Json으로 저장
    }

    public void RemoveTowerObject(int handle) => _towerObjects.Remove(handle);  //타워 제거시 데이터 제거
    public void RemoveConObject(int handle) => _conObjects.Remove(handle);
}
