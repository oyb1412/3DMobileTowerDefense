using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyInfo : MonoBehaviour
{
    public static UIEnemyInfo Instance;
    private GameObject _panel;
    private Image _icon;
    private Text _nameText;
    private Text _moveSpeedText;
    private Text _hpText;
    private Text _physicsDefnseText;
    private Text _magicDefnseText;
    private Text _provideGoldText;
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Init();
    }

    private void Init() {
        _panel = Util.FindChild(gameObject, "Panel", false);
        _icon = Util.FindChild(_panel, "Icon", false).GetComponent<Image>();
        _nameText = Util.FindChild(_panel, "Name", false).GetComponent<Text>();
        _moveSpeedText = Util.FindChild(_panel, "MoveSpeedText", true).GetComponent<Text>();
        _hpText = Util.FindChild(_panel, "HpText", true).GetComponent<Text>();
        _physicsDefnseText = Util.FindChild(_panel, "PhysicsDefenseText", true).GetComponent<Text>();
        _magicDefnseText = Util.FindChild(_panel, "MagicDefenseText", true).GetComponent<Text>();
        _provideGoldText = Util.FindChild(_panel, "ProvideGoldText", true).GetComponent<Text>();
        _panel.SetActive(false);
    }

    public void SetEnemyInfoUI(bool trigger, EnemySelection enemy) {
        _panel.SetActive(trigger);
        if (trigger) {
            EnemyStatus status = enemy.EnemyStatus;
            enemy.GetComponentInParent<EnemyController>().OnHpEvent += SetHpText;
            SetHpText(status.CurrentHp, status.MaxHp);
            _nameText.text = status.transform.parent.gameObject.name;
            _moveSpeedText.text = status.MoveSpeed.ToString();
            _physicsDefnseText.text = status.PhysicsDefense.ToString();
            _magicDefnseText.text = status.MagicDefense.ToString();
            _provideGoldText.text = status.ProvideGold.ToString();
            _icon.sprite = status.Icon;
        } else
            enemy.GetComponentInParent<EnemyController>().OnHpEvent = null;
    }

    private void SetHpText(int currentHp, int maxHp) {
        _hpText.text = $"{currentHp.ToString()} / {maxHp.ToString()}";
    }
}
