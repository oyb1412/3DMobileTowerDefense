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
    [SerializeField] float UpperAmout;

    void Start()
    {
        _hpSlider = Util.FindChild(gameObject, "HpSlider", false).GetComponent<Slider>();
        _unitPoint = Util.FindChild(transform.parent.gameObject, "Model", false).transform;
        _rewardPanel = Util.FindChild(gameObject, "RewardPanel", false);
        _rewardText = Util.FindChild(_rewardPanel, "RewardText", false).GetComponent<Text>();
        EnemyController root = transform.parent.GetComponent<EnemyController>();

        root.OnHpEvent += HpEventDelegate;
        root.OnRewardEvent += OnRewardEvent;
        root.Status.OnDeadEvent += (() => Managers.Resources.Destroy(gameObject)) ;

        _hpSlider.value = _hpSlider.maxValue;
        _hpSliderRect = _hpSlider.GetComponent<RectTransform>();

        _rewardText.text = $"+ {root.Status.ProvideGold}g";
        _rewardPanelRect = _rewardPanel.GetComponent<RectTransform>();

        Vector3 pos = new Vector3(_unitPoint.position.x, _unitPoint.position.y, _unitPoint.position.z + UpperAmout);
        Util.RectToWorldPosition(pos, _hpSliderRect);
        Util.RectToWorldPosition(pos, _rewardPanelRect);

        _rewardPanel.SetActive(false);

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
        }
    }
    private void HpEventDelegate(int currentHp, int maxHp) {
        _hpSlider.maxValue = maxHp;
        _hpSlider.value = currentHp;
    }
}
