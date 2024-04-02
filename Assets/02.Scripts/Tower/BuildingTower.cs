using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTower : MonoBehaviour
{
    [SerializeField]private GameObject _completedTowerObject;
    private BuildingStatus _status;
    private Slider _creatorSlider;
    
    private float _curretCreatingAmout;
    private float _maxCreatingAmout;

    private void Start() {
        _creatorSlider = GetComponentInChildren<Slider>();
        _status = GetComponent<BuildingStatus>();
        _curretCreatingAmout = _status.CurrentBuildingAmout;
        _maxCreatingAmout = _status.MaxBuildingAmout;
    }

    private void Update() {
        _curretCreatingAmout += Time.deltaTime;
        _creatorSlider.value = _curretCreatingAmout / _maxCreatingAmout;
        if (_curretCreatingAmout >= _maxCreatingAmout)
            CreateTower();
    }

    private void CreateTower() {
        TowerStatus tower = Managers.Resources.Instantiate(_completedTowerObject, null).GetComponent<TowerStatus>();
        tower.Init(_status.Level, transform.position, _status.TowerType);
        Managers.Resources.Destroy(gameObject);
    }

}
