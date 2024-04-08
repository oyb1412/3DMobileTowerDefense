using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CheckPointParent;

public class CheckPoint : MonoBehaviour
{
    private EnemyList _firstEnemy;
    private CheckPointParent _checkPointParent;
    public int MyNumber { get; set; }
    private void Start() {
        _checkPointParent = GetComponentInParent<CheckPointParent>();
    }
    private void OnTriggerEnter(Collider c) {
        if (!c.CompareTag("Enemy"))
            return;

        _firstEnemy = new EnemyList(MyNumber, c.gameObject);

        _checkPointParent.AddnSort(_firstEnemy);
    }
}
