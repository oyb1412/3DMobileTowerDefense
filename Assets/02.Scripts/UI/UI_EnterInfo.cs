using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 타워 업그레이드 및 설치 정보 표시 UI
/// </summary>
public class UI_EnterInfo : MonoBehaviour
{
    private Button _enterButton;  //건설 버튼
    private Text _name;  //타워 이름
    private Text _info;  //타워 정보
    private Text _damageText;  //타워 데미지
    private Text _delayText;  //타워 공격 딜레이
    private Text _rangeText;  //타워 사거리
    private Text _costText;  //타워 비용
    private Text _createText;  //건설 텍스트

    private void Start() {
        _enterButton = Util.FindChild(gameObject, "CreateBtn", true).GetComponent<Button>();
        _name = Util.FindChild(gameObject, "Name", true).GetComponent<Text>();
        _info = Util.FindChild(gameObject, "Info", true).GetComponent<Text>();
        _damageText = Util.FindChild(gameObject, "AttackDamageText", true).GetComponent<Text>();
        _delayText = Util.FindChild(gameObject, "AttackDelayText", true).GetComponent<Text>();
        _rangeText = Util.FindChild(gameObject, "AttackRangeText", true).GetComponent<Text>();
        _costText = Util.FindChild(gameObject, "CostText", true).GetComponent<Text>();
        _createText = Util.FindChild(gameObject, "CreateText", true).GetComponent<Text>();

        Managers.Language.SetText(_createText, Define.TextKey.Build);

        gameObject.SetActive(false);
    }

    /// <summary>
    /// 골드가 충분할 시 건설 버튼 활성화 및 이벤트 구독
    /// </summary>
    /// <param name="call">구독할 이벤트</param>
    /// <param name="cost">타워 건설 비용</param>
    public void SetBtn(UnityAction call, int cost) {
        if (!GameSystem.Instance.EnoughGold(cost)) {
            _enterButton.interactable = false;
            return;
        }
        else {
            _enterButton.interactable = true;
            _enterButton.onClick.RemoveAllListeners();
            Util.SetButtonEvent(_enterButton, null, call);
        }
    }

    /// <summary>
    /// 타워의 정보를 기반으로 UI 초기화
    /// </summary>
    public void SetEnterInfoUI(string name, int damage, float delay, float range, int cost, Define.TowerType type) {
        _name.text = name;
        _damageText.text = damage.ToString();
        _delayText.text = delay.ToString();
        _rangeText.text = range.ToString();
        _costText.text = cost.ToString();
        switch (type) {
            case Define.TowerType.ArcherTower:
                Managers.Language.SetText(_info, Define.TextKey.ArcherTowerDescription);
                break;
            case Define.TowerType.CanonTower:
                Managers.Language.SetText(_info, Define.TextKey.CanonTowerDescription);
                break;
            case Define.TowerType.MagicTower:
                Managers.Language.SetText(_info, Define.TextKey.MagicTowerDescription);
                break;
            case Define.TowerType.DeathTower:
                Managers.Language.SetText(_info, Define.TextKey.DeathTowerDescription);
                break;
        }
    }
}
