using System;
using UnityEngine;

/// <summary>
/// Ÿ�� ��ȣ�ۿ� ����
/// </summary>
public class TowerBase : MonoBehaviour, ISelectedObject {
    private string _nextConPath;  //���׷��̵� �� ������ Con�� path
    private GameObject _attackRange;  //ȭ�鿡 ǥ�õǴ� Ÿ�� ��Ÿ� ������Ʈ
    private GameObject _mark;  //ȭ�鿡 ǥ�õǴ� Ÿ�� ���� ������Ʈ
    private ParticleSystem _destroyEffect;  //Ÿ�� ö�Ž� play�� ����Ʈ
    protected TowerStatus _towerStatus;  //Ÿ�� �ɷ�ġ
    public Action<int> OnKillEvent;  //�ֳʹ� óġ �� UI������Ʈ ������ ���� �̺�Ʈ
    public TowerStatus TowerStatus => _towerStatus;
    public int TowerHandle { get; set; }  //Ÿ���� �ο��Ǵ� ���� �ڵ�
    public Transform MyTransform { get { return transform; } }
    
    /// <summary>
    /// Ÿ�� ������ �ʱ�ȭ
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
    /// Ÿ���� �ֳʹ� óġ �� ȣ��
    /// </summary>
    public void SetKill() {
        _towerStatus.KillNumber++;
        OnKillEvent?.Invoke(_towerStatus.KillNumber);
    }

    /// <summary>
    /// Ÿ�� ���� ����
    /// </summary>
    public void OnDeSelect() {
        UITower.Instance.SetTowerUI(false, this);  //Ÿ�� UI ��Ȱ��ȭ
        _attackRange.SetActive(false);
        _mark.SetActive(false);
    }

    /// <summary>
    /// Ÿ�� ���� 
    /// </summary>
    public ISelectedObject OnSelect() {
        UITower.Instance.SetTowerUI(true, this);  //Ÿ�� UI Ȱ��ȭ
        _attackRange.SetActive(true);
        _mark.SetActive(true);

        _attackRange.GetComponent<RectTransform>().sizeDelta =  //Ÿ�� ���� ��Ÿ��� ����� �̹��� ũ�� ����
            new Vector2(_towerStatus.AttackRange * GameSystem.TOWER_ATTACKRANGE_SIZE, _towerStatus.AttackRange * GameSystem.TOWER_ATTACKRANGE_SIZE);

        return this;
    }

    /// <summary>
    /// Ÿ�� �Ǹ�
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
    /// Ÿ�� ���׷��̵�
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
    /// Ÿ�� ��Ÿ��� �ð������� ǥ���� �����
    /// </summary>
    //private void OnDrawGizmosSelected() {
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, _towerStatus.AttackRange * GameSystem.TOWER_ATTACKRANGE_SIZE * .5f);
    //}

    public bool IsValid() => this;
}