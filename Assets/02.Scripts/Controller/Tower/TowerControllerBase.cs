using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControllerBase : MonoBehaviour
{
    private SphereCollider _attackRangeCollider;
    private GameObject _targetEnemy;
    private TowerStatus _status;

    private void Start() {
        Init();
    }

    protected virtual void Init() {
        _status = GetComponentInParent<TowerStatus>();
        _attackRangeCollider = GetComponent<SphereCollider>();
        _attackRangeCollider.radius = _status.AttackRange * GameSystem.TowerAttackRangeImageSize * .5f;
    }

    private void OnTriggerEnter(Collider c) {
        if (!c.CompareTag("Enemy"))
            return;

        if (_targetEnemy != null)
            return;

        _targetEnemy = c.gameObject;
        Debug.Log($"{_targetEnemy.name} 사거리 내로 진입");
    }

    private void OnTriggerExit(Collider c) {
        if (_targetEnemy == null)
            return;

        if (c != _targetEnemy)
            return;

        _targetEnemy = null;
    }
}
