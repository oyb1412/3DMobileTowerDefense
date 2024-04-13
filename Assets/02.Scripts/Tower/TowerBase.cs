using System;
using UnityEngine;

/// <summary>
/// 타워 상호작용 관리
/// </summary>
public class TowerBase : MonoBehaviour, ISelectedObject {
    private string _nextConPath;  //업그레이드 시 생성할 Con의 path
    private GameObject _attackRange;  //화면에 표시되는 타워 사거리 오브젝트
    private GameObject _mark;  //화면에 표시되는 타워 선택 오브젝트
    private ParticleSystem _destroyEffect;  //타워 철거시 play할 이펙트
    protected TowerStatus _towerStatus;  //타워 능력치
    public Action<int> OnKillEvent;  //애너미 처치 시 UI업데이트 변경을 위한 이벤트
    public TowerStatus TowerStatus => _towerStatus;
    public int TowerHandle { get; set; }  //타워에 부여되는 고유 핸들
    public Transform MyTransform { get { return transform; } }
    
    /// <summary>
    /// 타워 생성시 초기화
    /// </summary>
    public virtual void Init() {
        _towerStatus = GetComponent<TowerStatus>();
        _attackRange = Util.FindChild(gameObject, "AttackRange", true);
        _mark = Util.FindChild(gameObject, "Mark", true);
 
        _nextConPath = $"{_towerStatus.TowerType.ToString()}_Lvl{_towerStatus.Level + 1}Cons";

        _attackRange.SetActive(false);
        _mark.SetActive(false);

        _destroyEffect = Resources.Load<ParticleSystem>("Prefabs/Effect/DestroyEffect");
    }

    /// <summary>
    /// 타워가 애너미 처치 시 호출
    /// </summary>
    public void SetKill() {
        _towerStatus.KillNumber++;
        OnKillEvent?.Invoke(_towerStatus.KillNumber);
    }

    /// <summary>
    /// 타워 선택 해제
    /// </summary>
    public void OnDeSelect() {
        UITower.Instance.SetTowerUI(false, this);  //타워 UI 비활성화
        _attackRange.SetActive(false);
        _mark.SetActive(false);
    }

    /// <summary>
    /// 타워 선택 
    /// </summary>
    public ISelectedObject OnSelect() {
        UITower.Instance.SetTowerUI(true, this);  //타워 UI 활성화
        _attackRange.SetActive(true);
        _mark.SetActive(true);

        _attackRange.GetComponent<RectTransform>().sizeDelta =  //타워 공격 사거리에 비례해 이미지 크기 설정
            new Vector2(_towerStatus.AttackRange * GameSystem.TOWER_ATTACKRANGE_SIZE, _towerStatus.AttackRange * GameSystem.TOWER_ATTACKRANGE_SIZE);

        return this;
    }

    /// <summary>
    /// 타워 판매
    /// </summary>
    public void SellTower() {
        int sellGold = Managers.Data.GetSellCost((int)_towerStatus.TowerType, TowerStatus.Level);
        GameSystem.Instance.SetGold(sellGold);
        GameSystem.Instance.RemoveTowerObject(TowerHandle);
        GameObject go = Managers.Resources.Instantiate(_destroyEffect.gameObject, null);
        go.transform.position = transform.position;
        Managers.Audio.PlaySfx(Define.SfxType.Demolition);
        Managers.Resources.Destroy(gameObject);
    }

    /// <summary>
    /// 타워 업그레이드
    /// </summary>
    public void UpgradeTower() {
        if(TowerStatus.Level >= GameSystem.MAX_TOWER_LEVEL)
            return;

        int cost = Managers.Data.GetTowerCost((int)_towerStatus.TowerType, TowerStatus.Level + 1);
        GameSystem.Instance.SetGold(-cost);

        ConBase con = Managers.Resources.Instantiate($"Towers/{TowerStatus.TowerType.ToString()}/{_nextConPath}", null).GetComponent<ConBase>();
        con.Init(transform.position,_towerStatus.KillNumber);
        int handle = GameSystem.Instance.AddConObject(con, con.Status.TowerType, con.Status.Level, con.Status.KillNumber);
        con.ConHandle = handle;

        GameSystem.Instance.RemoveTowerObject(TowerHandle);
        Managers.Resources.Destroy(gameObject);
    }

    /// <summary>
    /// 타워 사거리를 시각적으로 표현할 기즈모
    /// </summary>
    //private void OnDrawGizmosSelected() {
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, _towerStatus.AttackRange * GameSystem.TOWER_ATTACKRANGE_SIZE * .5f);
    //}

    public bool IsValid() => this;
}