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
    private GameObject _lower;
    private RectTransform _centerRect;
    private TowerBase _selectTower;
    private Text _towerName;
    private Text _towerDamage;
    private Text _towerDelay;
    private Text _towerRange;
    private Text _towerKillNumber;
    private Image _lowerIcon;
    private Image _upgradeIcon;
    private void Awake() {
        Instance = this;
    }
    void Start() {
        Init();
    }

    private void Init() {
        _bundle = Util.FindChild(gameObject, "Bundle", false);
        _center = Util.FindChild(_bundle, "Center", false);
        _lower = Util.FindChild(_bundle, "Lower", false);
        _upgrade = Util.FindChild(_center, "UpgradeTower", false);
        _sell = Util.FindChild(_center, "SellTower", false);
        _towerName = Util.FindChild(_lower, "Name", false).GetComponent<Text>();
        _towerDamage = Util.FindChild(_lower, "AttackDamageText", true).GetComponent<Text>();
        _towerDelay = Util.FindChild(_lower, "AttackDelayText", true).GetComponent<Text>();
        _towerRange = Util.FindChild(_lower, "AttackRangeText", true).GetComponent<Text>();
        _towerKillNumber = Util.FindChild(_lower, "KillNumberText", true).GetComponent<Text>();
        _lowerIcon = Util.FindChild(_lower, "Icon", false).GetComponent<Image>();
        _upgradeIcon = Util.FindChild(_center, "Icon", true).GetComponent<Image>();

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
                int max = Mathf.Min(GameSystem.TowerMaxLevel, tower.TowerStatus.Level + 1);
                _upgradeIcon.sprite = Managers.Data.GetTowerIcon((int)tower.TowerStatus.TowerType, max);
            }
            _selectTower = tower;
            Util.RectToWorldPosition(tower.transform.position, _centerRect);
           // _sell.GetComponentInChildren<Text>().text = $"{Managers.Data.GetSellCost((int)tower.TowerStatus.TowerType, tower.TowerStatus.Level)}";
        }
    }

    private void SetTowerLowerUI(bool trigger, TowerBase tower) {
        if (trigger) {
            TowerStatus status = tower.TowerStatus;
            _towerDamage.text = status.AttackDamage.ToString();
            _towerDelay.text = status.AttackDelay.ToString();
            _towerRange.text = status.AttackRange.ToString();
            _towerKillNumber.text = status.KillNumber.ToString();
            _towerName.text = status.gameObject.name;
            _lowerIcon.sprite = status.Icon;
            tower.OnKillEvent += ((kill) => _towerKillNumber.text = kill.ToString());
        } else
            tower.OnKillEvent = null;
    }
}