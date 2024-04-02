using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : SelectedObject, ISelectedObject {
    [SerializeField]private GameObject _nextTowerCon;
    private GameObject _attackRange;
    protected TowerStatus _towerStatus;

    public TowerStatus TowerStatus => _towerStatus;
    public Transform MyTransform { get { return transform; } }
    private void Start() {
        Init();
    }

    protected virtual void Init() {
        _towerStatus = GetComponent<TowerStatus>();
        _attackRange = Util.FindChild(gameObject, "AttackRange", true);
        _attackRange.SetActive(false);
        _attackRange.GetComponent<RectTransform>().sizeDelta = 
            new Vector2(_towerStatus.AttackRange * GameSystem.TowerAttackRangeImageSize, _towerStatus.AttackRange * GameSystem.TowerAttackRangeImageSize);
    }
    public void OnDeSelect() {
        UITower.Instance.SetTowerUI(false, this);
        _attackRange.SetActive(false);
    }

    public ISelectedObject OnSelect() {
        UITower.Instance.SetTowerUI(true, this);
        _attackRange.SetActive(true);

        return this;
    }

    public void SellTower() {
        int sellGold = Managers.Data.GetSellCost(TowerStatus.Level, (int)_towerStatus.TowerType);
        GameSystem.Instance.SetGold(sellGold);

        Managers.Resources.Destroy(gameObject);
    }

    public void UpgradeTower() {
        if(TowerStatus.Level >= GameSystem.TowerMaxLevel ||
            _nextTowerCon == null) {
            Debug.Log("최대레벨입니다.");
            return;
        }

        int cost = Managers.Data.GetSellCost(TowerStatus.Level + 1, (int)_towerStatus.TowerType);
        GameSystem.Instance.SetGold(-cost);

        GameObject go = Managers.Resources.Instantiate(_nextTowerCon.gameObject, null);
        go.transform.position = transform.position;

        Managers.Resources.Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _towerStatus.AttackRange * GameSystem.TowerAttackRangeImageSize * .5f);
    }
}