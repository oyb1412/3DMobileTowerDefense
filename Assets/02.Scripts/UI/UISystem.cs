using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISystem : UIBase {
    private Text _goldText;
    void Start() {
        Init();
    }

    private void Init() {
        _goldText = Util.FindChild(gameObject, "GoldText", false).GetComponent<Text>();
        _goldText.text = GameSystem.Instance.CurrentGold.ToString();
        GameSystem.Instance.OnGoldEvent += ((currentGold) => _goldText.text = currentGold.ToString());
    }
}