using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTower : MonoBehaviour
{
    private const float LowerAmout = -5f;
    private BuildingStatus _status;
    private string _createTowerPath;
    private float _curretCreatingAmout;
    private float _maxCreatingAmout;
    public Action<float> OnCreatEvent;

    private void Start() {
        
    }
    public void Init(Vector3 pos, int killNumber = 0) {
        transform.position = pos;
        _status = GetComponent<BuildingStatus>();
        _status.KillNumber = killNumber;
        _curretCreatingAmout = _status.CurrentBuildingAmout;
        _maxCreatingAmout = _status.MaxBuildingAmout;
        string name = gameObject.name;
        _createTowerPath = name.Substring(0, name.Length - 4);
    }

    private void Update() {
        _curretCreatingAmout += Time.deltaTime;
        if (_curretCreatingAmout >= _maxCreatingAmout)
            CreateTower();

        OnCreatEvent?.Invoke(_curretCreatingAmout / _maxCreatingAmout);
    }

    private void CreateTower() {
        TowerStatus tower = Managers.Resources.Instantiate($"Towers/{_status.TowerType.ToString()}/{_createTowerPath}", null).GetComponent<TowerStatus>();
        tower.Init(_status.KillNumber, _status.Level, transform.position, _status.TowerType); 
        Managers.Resources.Destroy(gameObject);
    }

}
