using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour, ISelectedObject {
    private string _nextConPath;
    private GameObject _attackRange;
    private GameObject _mark;
    private ParticleSystem _destroyEffect;
    protected TowerStatus _towerStatus;
    public Action<int> OnKillEvent;
    public TowerStatus TowerStatus => _towerStatus;
    public Transform MyTransform { get { return transform; } }
    private void Start() {
        Init();
    }

    protected virtual void Init() {
        _towerStatus = GetComponent<TowerStatus>();
        _attackRange = Util.FindChild(gameObject, "AttackRange", true);
        _mark = Util.FindChild(gameObject, "Mark", true);
 
        _attackRange.GetComponent<RectTransform>().sizeDelta = 
            new Vector2(_towerStatus.AttackRange * GameSystem.TowerAttackRangeImageSize, _towerStatus.AttackRange * GameSystem.TowerAttackRangeImageSize);

        _nextConPath = $"{_towerStatus.TowerType.ToString()}_Lvl{_towerStatus.Level + 1}Cons";

        _attackRange.SetActive(false);
        _mark.SetActive(false);

        _destroyEffect = Resources.Load<ParticleSystem>("Prefabs/Effect/DestroyEffect");
    }

    public void SetKill() {
        _towerStatus.KillNumber++;
        OnKillEvent?.Invoke(_towerStatus.KillNumber);
    }
    public void OnDeSelect() {
        UITower.Instance.SetTowerUI(false, this);
        _attackRange.SetActive(false);
        _mark.SetActive(false);
    }

    public ISelectedObject OnSelect() {
        UITower.Instance.SetTowerUI(true, this);
        _attackRange.SetActive(true);
        _mark.SetActive(true);

        return this;
    }

    public void SellTower() {
        int sellGold = Managers.Data.GetSellCost((int)_towerStatus.TowerType, TowerStatus.Level);
        GameSystem.Instance.SetGold(sellGold);
        GameObject go = Managers.Resources.Instantiate(_destroyEffect.gameObject, null);
        go.transform.position = transform.position;
        Managers.Resources.Destroy(gameObject);
    }

    public void UpgradeTower() {
        if(TowerStatus.Level >= GameSystem.TowerMaxLevel) {
            Debug.Log("최대레벨입니다.");
            return;
        }

        int cost = Managers.Data.GetTowerCost((int)_towerStatus.TowerType, TowerStatus.Level + 1);
        GameSystem.Instance.SetGold(-cost);

        BuildingTower con = Managers.Resources.Instantiate($"Towers/{TowerStatus.TowerType.ToString()}/{_nextConPath}", null).GetComponent<BuildingTower>();
        con.Init(transform.position, _towerStatus.KillNumber);

        Managers.Resources.Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _towerStatus.AttackRange * GameSystem.TowerAttackRangeImageSize * .5f);
    }

    public bool IsValid() {
        return this;
    }
}