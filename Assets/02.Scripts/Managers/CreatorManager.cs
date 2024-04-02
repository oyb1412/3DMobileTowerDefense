using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorManager {
    public void CreateTower(string name, Vector3 createPos) {
        GameObject tower = Managers.Resources.Instantiate($"Towers/{name}/{name}Cons", null);
        tower.transform.position = createPos;
    }
}
