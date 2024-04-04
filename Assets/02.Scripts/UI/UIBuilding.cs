using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuilding : MonoBehaviour {
    [SerializeField]float LowerAmout;
    private RectTransform _sliderRect;
    private Slider _creatorSlider;
    private BuildingTower _buildingTower;
    private Transform _towerPoint;

    private void Start() {
        _towerPoint = transform.root;
        _buildingTower = GetComponentInParent<BuildingTower>();
        _creatorSlider = GetComponentInChildren<Slider>();
        _sliderRect = _creatorSlider.GetComponent<RectTransform>();

        _buildingTower.OnCreatEvent += ((amout) => _creatorSlider.value = amout);
    }

    private void LateUpdate() {
        Vector3 pos = new Vector3(_towerPoint.position.x, _towerPoint.position.y, _towerPoint.position.z + LowerAmout);
        Util.RectToWorldPosition(pos, _sliderRect);
    }
}