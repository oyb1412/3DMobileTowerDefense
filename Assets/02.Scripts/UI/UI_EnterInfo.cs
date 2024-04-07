using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EnterInfo : UIBase
{
    private Button _enterButton;
    private Text _name;
    private Text _info;
    private Text _damageText;
    private Text _delayText;
    private Text _rangeText;
    private Text _costText;
    private GameObject _panel;
    private RectTransform _rectTransform;
    private void Start() {
        _enterButton = Util.FindChild(gameObject, "CreateBtn", true).GetComponent<Button>();
        _panel = Util.FindChild(gameObject, "Panel", false);
        _name = Util.FindChild(gameObject, "Name", true).GetComponent<Text>();
        _info = Util.FindChild(gameObject, "Info", true).GetComponent<Text>();
        _damageText = Util.FindChild(gameObject, "AttackDamageText", true).GetComponent<Text>();
        _delayText = Util.FindChild(gameObject, "AttackDelayText", true).GetComponent<Text>();
        _rangeText = Util.FindChild(gameObject, "AttackRangeText", true).GetComponent<Text>();
        _costText = Util.FindChild(gameObject, "CostText", true).GetComponent<Text>();

        _rectTransform = _panel.GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }
    public void SetBtn(UnityAction call, int cost) {
        if (!GameSystem.Instance.EnoughGold(cost)) {
            _enterButton.interactable = false;
            return;
        }
        else {
            _enterButton.interactable = true;
            _enterButton.onClick.RemoveAllListeners();
            _enterButton.onClick.AddListener(call);
        }
    }
    public void SetEnterInfoUI(string name, string info, int damage, float delay, float range, int cost) {
        _name.text = name;
        _info.text = info;
        _damageText.text = damage.ToString();
        _delayText.text = delay.ToString();
        _rangeText.text = range.ToString();
        _costText.text = cost.ToString();
    }

    public void SetPosition(RectTransform rect) {
        _rectTransform.anchoredPosition = rect.anchoredPosition + new Vector2(964f, 740f);
    }
}
