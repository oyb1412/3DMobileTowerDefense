using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class UIEnemy : MonoBehaviour
{
    private Slider _hpSlider;
    private RectTransform _hpSliderRect;
    private Transform _unitPoint;
    [SerializeField] float UpperAmout;

    void Start()
    {
        _hpSlider = Util.FindChild(gameObject, "HpSlider", false).GetComponent<Slider>();
        EnemyController root = transform.root.GetComponent<EnemyController>();

        root.OnHpEvent += HpEventDelegate;
        root.OnDieEvent += (() => Managers.Resources.Destroy(gameObject)) ;
        _hpSlider.value = _hpSlider.maxValue;


        _hpSliderRect = _hpSlider.GetComponent<RectTransform>();
        _unitPoint = Util.FindChild(transform.root.gameObject, "Model", false).transform;
    }

    private void Update() {
        Vector3 pos = new Vector3(_unitPoint.position.x, _unitPoint.position.y, _unitPoint.position.z + UpperAmout);
        Util.RectToWorldPosition(pos, _hpSliderRect);
    }

    private void HpEventDelegate(int currentHp, int maxHp) {
        _hpSlider.maxValue = maxHp;
        _hpSlider.value = currentHp;
    }
}
