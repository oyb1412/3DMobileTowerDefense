using System;
using UnityEngine;

/// <summary>
/// Con 기본 설정 관리
/// </summary>
public class ConBase : MonoBehaviour
{
    private const float CREATE_SOUND_DELAY = 1f;

    private ConStatus _status;  //Con능력치
    public Action<float> OnCreatEvent;  //타워 생성 시 이벤트

    private string _createTowerPath;  //생성 완료시 생성할 타워의 path
    private float _soundAmout;

    public ConStatus Status { get { return _status; } }
    public int ConHandle { get; set; }  //Con에 부여되는 고유 핸들

    /// <summary>
    /// 초기화
    /// </summary>
    /// <param name="pos">위치</param>
    /// <param name="killNumber">이전 타워의 kill수 상속</param>
    public void Init(Vector3 pos, int killNumber = 0) {
        transform.position = pos;
        _status = GetComponent<ConStatus>();
        _status.CurrentBuildingAmout = 0;
        _status.KillNumber = killNumber;
        string name = gameObject.name;
        _createTowerPath = name.Substring(0, name.Length - 4);
    }

    private void Update() {
        if (!GameSystem.Instance.IsPlay())  //게임 종료 시 정지
            return;

        _status.CurrentBuildingAmout += Time.deltaTime;
        _soundAmout += Time.deltaTime;
        if(_soundAmout >= CREATE_SOUND_DELAY) {  //create 사운드 호출
            Managers.Audio.PlaySfx(Define.SfxType.Build);
            _soundAmout = 0;
        }
            
        if (_status.CurrentBuildingAmout >= _status.MaxBuildingAmout)
            CreateTower();

        OnCreatEvent?.Invoke(_status.CurrentBuildingAmout / _status.MaxBuildingAmout);
    }

    /// <summary>
    /// 타워 생성
    /// </summary>
    private void CreateTower() {
        TowerStatus tower = Managers.Resources.Instantiate($"Towers/{_status.TowerType.ToString()}/{_createTowerPath}", null).GetComponent<TowerStatus>();
        tower.Init(_status.KillNumber, _status.Level, transform.position, _status.TowerType);
        var towerbase = tower.GetComponent<TowerBase>();
        towerbase.Init();
        towerbase.TowerHandle = GameSystem.Instance.AddTowerObject(towerbase, tower.TowerType, tower.Level);
        Managers.Audio.PlaySfx(Define.SfxType.BuildCompleted);
        GameSystem.Instance.RemoveConObject(ConHandle);
        Managers.Resources.Destroy(gameObject);
    }
}
