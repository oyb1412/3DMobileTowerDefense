using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;
using static UnityEditor.PlayerSettings;

public class UITower : UIBase {
    public static UITower Instance;

    private GameObject _upgrade;
    private GameObject _sell;
    private GameObject _bundle;
    private GameObject _center;
    private RectTransform _centerRect;
    private TowerBase _selectTower;
    private Text _towerDamage;
    private Text _towerDelay;
    private Text _towerRange;
    private void Awake() {
        Instance = this;
    }
    void Start() {
        Init();
    }

    private void Init() {
        _bundle = Util.FindChild(gameObject, "Bundle", false);
        _center = Util.FindChild(gameObject, "Center", true);
        _upgrade = Util.FindChild(_bundle, "UpgradeTower", true);
        _sell = Util.FindChild(_bundle, "SellTower", true);
        _towerDamage = Util.FindChild(_bundle, "AttackDamage", true).GetComponent<Text>();
        _towerDelay = Util.FindChild(_bundle, "AttackDelay", true).GetComponent<Text>();
        _towerRange = Util.FindChild(_bundle, "AttackRange", true).GetComponent<Text>();

        _upgrade.GetComponent<Button>().onClick.AddListener(UpgradeTower);
        _sell.GetComponent<Button>().onClick.AddListener(SellTower);

        _centerRect = _center.GetComponent<RectTransform>();
        _bundle.SetActive(false);
    }

    private void UpgradeTower() {
        _selectTower.UpgradeTower();
        _bundle.SetActive(false);
    }

    private void SellTower() {
        _selectTower.SellTower();
        _bundle.SetActive(false);
    }

    public void SetTowerUI(bool trigger, TowerBase tower) {
        SetTowerLowerUI(trigger, tower);
        SetTowerCenterUI(trigger, tower);
    }
    private void SetTowerCenterUI(bool trigger, TowerBase tower) {
        _bundle.SetActive(trigger);
        _selectTower = null;
        _upgrade.SetActive(false);

        if (trigger) {
            if(GameSystem.Instance.EnoughGold(Managers.Data.GetTowerCost((int)tower.TowerStatus.TowerType, tower.TowerStatus.Level))) {
                _upgrade.SetActive(true);
                _upgrade.GetComponentInChildren<Text>().text = $"·¹º§{tower.TowerStatus.Level + 1} {tower.TowerStatus.TowerType.ToString()}";
            }
            _selectTower = tower;
            Util.RectToWorldPosition(tower.transform.position, _centerRect);
            _sell.GetComponentInChildren<Text>().text = $"{Managers.Data.GetSellCost((int)tower.TowerStatus.TowerType, tower.TowerStatus.Level)}";
        }
    }

    private void SetTowerLowerUI(bool trigger, TowerBase tower) {
        if(trigger) {
            TowerStatus status = tower.TowerStatus;
            _towerDamage.text = status.AttackDamage.ToString();
            _towerDelay.text = status.AttackDelay.ToString();
            _towerRange.text = status.AttackRange.ToString();
        }
    }
}