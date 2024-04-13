using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Ÿ�� ���׷��̵� �� ��ġ ���� ǥ�� UI
/// </summary>
public class UI_EnterInfo : MonoBehaviour
{
    private Button _enterButton;  //�Ǽ� ��ư
    private Text _name;  //Ÿ�� �̸�
    private Text _info;  //Ÿ�� ����
    private Text _damageText;  //Ÿ�� ������
    private Text _delayText;  //Ÿ�� ���� ������
    private Text _rangeText;  //Ÿ�� ��Ÿ�
    private Text _costText;  //Ÿ�� ���
    private Text _createText;  //�Ǽ� �ؽ�Ʈ

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
    /// ��尡 ����� �� �Ǽ� ��ư Ȱ��ȭ �� �̺�Ʈ ����
    /// </summary>
    /// <param name="call">������ �̺�Ʈ</param>
    /// <param name="cost">Ÿ�� �Ǽ� ���</param>
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
    /// Ÿ���� ������ ������� UI �ʱ�ȭ
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
