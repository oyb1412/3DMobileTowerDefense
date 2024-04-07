using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelection : MonoBehaviour, ISelectedObject {
    public Transform MyTransform => transform;

    private EnemyStatus _enemyStatus;
    public EnemyStatus EnemyStatus => _enemyStatus;

    private GameObject _mark;
    private void Start() {
        _enemyStatus = GetComponent<EnemyStatus>();
        _mark = Util.FindChild(gameObject, "Mark", false);
        _mark.SetActive(false);
    }

    public void OnDeSelect() {
        UIEnemyInfo.Instance.SetEnemyInfoUI(false, this);
        _mark.SetActive(false);
    }

    public ISelectedObject OnSelect() {
        UIEnemyInfo.Instance.SetEnemyInfoUI(true, this);
        _mark.SetActive(true);
        return this;
    }

    public bool IsValid() {
        return this;
    }
}
