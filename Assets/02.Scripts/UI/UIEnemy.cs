using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemy : MonoBehaviour
{
    private Slider _hpSlider;
    private RectTransform _hpSliderRect;
    private RectTransform _rewardPanelRect;
    private Transform _unitPoint;
    private GameObject _rewardPanel;
    private Text _rewardText;
    private EnemyController _root;
    [SerializeField] float UpperAmout;

    void Awake()
    {
        _hpSlider = Util.FindChild(gameObject, "HpSlider", false).GetComponent<Slider>();
        _unitPoint = Util.FindChild(transform.parent.gameObject, "Model", false).transform;
        _rewardPanel = Util.FindChild(gameObject, "RewardPanel", false);
        _rewardText = Util.FindChild(_rewardPanel, "RewardText", false).GetComponent<Text>();
        _root = transform.parent.GetComponent<EnemyController>();

        _root.OnHpEvent += HpEventDelegate;
        _root.OnRewardEvent += OnRewardEvent;
        _root.Status.OnDeadEvent += OnDeadUpdate;

        _hpSliderRect = _hpSlider.GetComponent<RectTransform>();

        _rewardPanelRect = _rewardPanel.GetComponent<RectTransform>();

        Vector3 pos = new Vector3(_unitPoint.position.x, _unitPoint.position.y, _unitPoint.position.z + UpperAmout);
        Util.RectToWorldPosition(pos, _hpSliderRect);
        Util.RectToWorldPosition(pos, _rewardPanelRect);

        _rewardPanel.SetActive(false);
    }

    private void OnDeadUpdate() {
        _rewardPanel.SetActive(false);
        _hpSlider.gameObject.SetActive(false);
    }

    public void Init() {
        _hpSlider.gameObject.SetActive(true);
        _hpSlider.maxValue = _root.Status.MaxHp;
        _hpSlider.value = _hpSlider.maxValue;
        _rewardText.text = $"+ {_root.Status.RewardGold}g";
    }

    private void Update() {
        Vector3 pos = new Vector3(_unitPoint.position.x, _unitPoint.position.y, _unitPoint.position.z + UpperAmout);
        Util.RectToWorldPosition(pos, _hpSliderRect);
    }

    private void OnRewardEvent() {
        _hpSlider.gameObject.SetActive(false);
        _rewardPanel.SetActive(true);

        Vector3 pos = new Vector3(_unitPoint.position.x, _unitPoint.position.y, _unitPoint.position.z + UpperAmout);
        Util.RectToWorldPosition(pos, _rewardPanelRect);

        StartCoroutine(CoRewardPanelMove());
    }

    IEnumerator CoRewardPanelMove() {
        while(true) {
            _rewardPanelRect.anchoredPosition += Vector2.up / 2;
            yield return null;
            if (Util.NullCheck(_rewardPanel)) {
                StopAllCoroutines();
                break;
            }
        }
    }
    private void HpEventDelegate(int currentHp, int maxHp) {
        _hpSlider.maxValue = maxHp;
        _hpSlider.value = currentHp;
    }
}
