using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorManager {
    public void CreateTower(string name, Vector3 createPos) {
        BuildingTower tower = Managers.Resources.Instantiate($"Towers/{name}/{name}_Lvl1Cons", null).GetComponent<BuildingTower>();
        tower.Init(createPos);
    }
}
