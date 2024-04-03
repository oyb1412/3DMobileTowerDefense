using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTower : MonoBehaviour
{
    private BuildingStatus _status;
    private Slider _creatorSlider;
    private string _createTowerPath;
    private float _curretCreatingAmout;
    private float _maxCreatingAmout;

    private void Start() {
        _creatorSlider = GetComponentInChildren<Slider>();
        _status = GetComponent<BuildingStatus>();
        _curretCreatingAmout = _status.CurrentBuildingAmout;
        _maxCreatingAmout = _status.MaxBuildingAmout;
        string name = gameObject.name;
        _createTowerPath = name.Substring(0, name.Length - 4);
    }

    private void Update() {
        _curretCreatingAmout += Time.deltaTime;
        _creatorSlider.value = _curretCreatingAmout / _maxCreatingAmout;
        if (_curretCreatingAmout >= _maxCreatingAmout)
            CreateTower();
    }

    private void CreateTower() {
        TowerStatus tower = Managers.Resources.Instantiate($"Towers/{_status.TowerType.ToString()}/{_createTowerPath}", null).GetComponent<TowerStatus>();
        tower.Init(_status.Level, transform.position, _status.TowerType); 
        Managers.Resources.Destroy(gameObject);
    }

}
