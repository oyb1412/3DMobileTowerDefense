using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UITower : MonoBehaviour {
    public static UITower Instance;

    private UI_EnterInfo _creatorInfoPanel;
    private UI_SellInfo _sellInfoPanel;
    private Data _data;
    private GameObject _upgrade;
    private GameObject _sell;
    private GameObject _bundle;
    private GameObject _center;
    private GameObject _lower;
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
        _creatorInfoPanel = GameObject.Find("UI_CreatorInfo").GetComponent<UI_EnterInfo>();
        _sellInfoPanel = GameObject.Find("UI_SellInfo").GetComponent<UI_SellInfo>();
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

        Util.SetButtonEvent(_upgrade.GetComponent<Button>(), null, SelecteCreator);
        Util.SetButtonEvent(_sell.GetComponent<Button>(), null, SelecteSell);

        _data = Managers.Data;

        _bundle.SetActive(false);
    }

    private void SelecteCreator() {
        if (_selectTower.TowerStatus.Level == GameSystem.MAX_TOWER_LEVEL)
            return;

        _creatorInfoPanel.gameObject.SetActive(true);
        _sellInfoPanel.gameObject.SetActive(false);

        string name = $"{_selectTower.TowerStatus.TowerType.ToString()} Lvl{_selectTower.TowerStatus.Level + 1}";
        int damage = _data.GetTowerAttackDamage((int)_selectTower.TowerStatus.TowerType, _selectTower.TowerStatus.Level + 1);
        float delay = _data.GetTowerAttacnDelay((int)_selectTower.TowerStatus.TowerType, _selectTower.TowerStatus.Level + 1);
        float range = _data.GetTowerAttacmRange((int)_selectTower.TowerStatus.TowerType, _selectTower.TowerStatus.Level + 1);

        int cost = _data.GetTowerCost((int)_selectTower.TowerStatus.TowerType, _selectTower.TowerStatus.Level + 1);

        ResetOutline();
        Util.SetOutLine(_upgrade, true);

        _creatorInfoPanel.SetEnterInfoUI(name,  damage, delay, range, cost, _selectTower.TowerStatus.TowerType);
        _creatorInfoPanel.SetBtn(UpgradeTower, cost);
    }

    private void SelecteSell() {
        _creatorInfoPanel.gameObject.SetActive(false);
        _sellInfoPanel.gameObject.SetActive(true);

        int reward = _data.GetSellCost((int)_selectTower.TowerStatus.TowerType, _selectTower.TowerStatus.Level);

        ResetOutline();
        Util.SetOutLine(_sell, true);

        _sellInfoPanel.SetSellInfoUI(reward);
        _sellInfoPanel.SetBtn(SellTower);
    }

    private void ResetOutline() {
        Util.SetOutLine(_sell, false);
        Util.SetOutLine(_upgrade, false);
    }

    private void UpgradeTower() {
        _selectTower.UpgradeTower();
        _bundle.SetActive(false);
        _creatorInfoPanel.gameObject.SetActive(false);
    }

    private void SellTower() {
        _selectTower.SellTower();
        _bundle.SetActive(false);
        _sellInfoPanel.gameObject.SetActive(false);
    }

    public void SetTowerUI(bool trigger, TowerBase tower) {
        SetTowerLowerUI(trigger, tower);
        SetTowerCenterUI(trigger, tower);
    }
    private void SetTowerCenterUI(bool trigger, TowerBase tower) {
        _bundle.SetActive(trigger);
        _selectTower = null;
        ResetOutline();
        if (trigger) {
            int max = Mathf.Min(GameSystem.MAX_TOWER_LEVEL, tower.TowerStatus.Level + 1);
            if(tower.TowerStatus.Level >= GameSystem.MAX_TOWER_LEVEL) {
                _upgrade.SetActive(false);
            }
            else
                _upgrade.SetActive(true);

            _upgradeIcon.sprite = Managers.Data.GetTowerIcon((int)tower.TowerStatus.TowerType, max);
            _selectTower = tower;
        }
        else {
            _sellInfoPanel.gameObject.SetActive(false);
            _creatorInfoPanel.gameObject.SetActive(false);

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