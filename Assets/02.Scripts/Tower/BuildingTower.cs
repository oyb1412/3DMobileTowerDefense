using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTower : MonoBehaviour
{
    private const float LowerAmout = -5f;
    private BuildingStatus _status;
    private string _createTowerPath;
    private float _soundAmout;
    public Action<float> OnCreatEvent;

    public BuildingStatus Status { get { return _status; } }
    public int ConHandle { get; set; }

    private void Start() {
        
    }
    public void Init(Vector3 pos, int killNumber = 0) {
        transform.position = pos;
        _status = GetComponent<BuildingStatus>();
        _status.KillNumber = killNumber;
        string name = gameObject.name;
        _createTowerPath = name.Substring(0, name.Length - 4);
    }


    private void Update() {
        if (!GameSystem.Instance.IsPlay())
            return;

        _status.CurrentBuildingAmout += Time.deltaTime;
        _soundAmout += Time.deltaTime;
        if(_soundAmout >= 1.5f) {
            Managers.Audio.PlaySfx(Define.SfxType.Build);
            _soundAmout = 0;
        }
            
        if (_status.CurrentBuildingAmout >= _status.MaxBuildingAmout)
            CreateTower();

        OnCreatEvent?.Invoke(_status.CurrentBuildingAmout / _status.MaxBuildingAmout);
    }

    private void CreateTower() {
        TowerStatus tower = Managers.Resources.Instantiate($"Towers/{_status.TowerType.ToString()}/{_createTowerPath}", null).GetComponent<TowerStatus>();
        tower.Init(_status.KillNumber, _status.Level, transform.position, _status.TowerType);
        var towerbase = tower.GetComponent<TowerBase>();
        towerbase.Init();
        towerbase.TowerHandle = GameSystem.Instance.AddTowerObject(towerbase, tower.TowerType, tower.Level);
        Managers.Audio.PlaySfx(Define.SfxType.BuildCompleted);
        GameSystem.Instance.RemoveConObject(ConHandle);
        Managers.Resources.Destroy(gameObject);
    }

}
