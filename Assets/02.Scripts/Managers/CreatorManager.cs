using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorManager {
    public void CreateTower(string name, Vector3 createPos) {
        string path = name.Substring(0, name.Length - 5);
        BuildingTower tower = Managers.Resources.Instantiate($"Towers/{path}/{path}_Lvl1Cons", null).GetComponent<BuildingTower>();
        tower.Init(createPos);
        int handle = GameSystem.Instance.AddConObject(tower, tower.Status.TowerType, tower.Status.Level, tower.Status.KillNumber);
        tower.ConHandle = handle;
    }
}
