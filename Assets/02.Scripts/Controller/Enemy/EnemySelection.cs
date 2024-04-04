using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelection : MonoBehaviour, ISelectedObject {
    public Transform MyTransform => transform;

    private EnemyStatus _enemyStatus;
    public EnemyStatus EnemyStatus => _enemyStatus;

    private void Start() {
        _enemyStatus = GetComponent<EnemyStatus>();
    }

    public void OnDeSelect() {
        UIEnemyInfo.Instance.SetEnemyInfoUI(false, this);
    }

    public ISelectedObject OnSelect() {
        UIEnemyInfo.Instance.SetEnemyInfoUI(true, this);
        return this;
    }

    public bool IsValid() {
        return this;
    }
}
