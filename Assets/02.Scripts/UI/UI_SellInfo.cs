using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_SellInfo : MonoBehaviour
{
    private GameObject _panel;
    private Text _rewardText;
    private Text _sellText;
    private Button _sellBtn;
    private RectTransform _rectTransform;
    void Start()
    {
        Init();
    }

    private void Init() {
        _panel = Util.FindChild(gameObject, "Panel", false);
        _rewardText = Util.FindChild(_panel, "RewardText", true).GetComponent<Text>();
        _sellText = Util.FindChild(_panel, "SellText", true).GetComponent<Text>();
        _sellBtn = Util.FindChild(_panel, "SellBtn", true).GetComponent<Button>();
        _rectTransform = _panel.GetComponent<RectTransform>();

        Managers.Language.SetText(_sellText, Define.TextKey.Demolition);

        gameObject.SetActive(false);
    }

    public void SetBtn(UnityAction call) {
        _sellBtn.onClick.RemoveAllListeners();

        Util.SetButtonEvent(_sellBtn, null, call);
    }
    public void SetSellInfoUI(int reward) {
        _rewardText.text = reward.ToString();
    }

    public void SetPosition(RectTransform rect) {
        _rectTransform.anchoredPosition = rect.anchoredPosition + new Vector2(964f, 740f);
    }
}
