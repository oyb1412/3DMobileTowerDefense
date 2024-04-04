using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyInfo : MonoBehaviour
{
    public static UIEnemyInfo Instance;
    private GameObject _panel;
    private Text _moveSpeedText;
    private Text _hpText;
    private Text _physicsDefnseText;
    private Text _magicDefnseText;
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Init();
    }

    private void Init() {
        _panel = Util.FindChild(gameObject, "Panel", false);
        _moveSpeedText = Util.FindChild(_panel, "MoveSpeed", false).GetComponent<Text>();
        _hpText = Util.FindChild(_panel, "Hp", false).GetComponent<Text>();
        _physicsDefnseText = Util.FindChild(_panel, "PhysicsDefense", false).GetComponent<Text>();
        _magicDefnseText = Util.FindChild(_panel, "MagicDefense", false).GetComponent<Text>();
        _panel.SetActive(false);
    }

    public void SetEnemyInfoUI(bool trigger, EnemySelection enemy) {
        _panel.SetActive(trigger);
        if(trigger) {
            EnemyStatus status = enemy.EnemyStatus;

            _moveSpeedText.text = status.MoveSpeed.ToString();
            SetHpText(status.CurrentHp, status.MaxHp);
            _hpText.text = $"{status.CurrentHp.ToString()} / {status.MaxHp.ToString()}";
            _physicsDefnseText.text = status.PhysicsDefense.ToString();
            _magicDefnseText.text = status.MagicDefense.ToString();
        }
    }

    private void SetHpText(int currentHp, int maxHp) {
        _hpText.text = $"{currentHp.ToString()} / {maxHp.ToString()}";
    }
}
