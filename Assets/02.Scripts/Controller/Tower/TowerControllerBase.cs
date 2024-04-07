using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerControllerBase : MonoBehaviour
{
    private SphereCollider _attackRangeCollider;
    private const float LimitPosRight = 75f;
    private const float LimitPosLeft = -105f;
    protected GameObject _base;
    protected string _projectilePath;
    protected Transform _firePoint;

    [SerializeField]protected GameObject _targetEnemy;
    protected TowerStatus _status;
    [SerializeField]private Define.TowerState _state;
    protected float _currentAttackDelay;
    protected GameObject _projectileObject;

    private void Start() {
        Init();
        _base = GetComponentInParent<TowerBase>().gameObject;
        _state = Define.TowerState.Idle;
    }

    protected virtual void Init() {
        _status = GetComponentInParent<TowerStatus>();
        _attackRangeCollider = GetComponent<SphereCollider>();
        _firePoint = Util.FindChild(transform.parent.gameObject, "FirePoint", false).transform;
        _attackRangeCollider.radius = _status.AttackRange * GameSystem.TowerAttackRangeImageSize * .5f;
        _projectilePath = $"Projectile/{_status.TowerType.ToString()}/{_status.TowerType.ToString()}ProjectileLvl{_status.Level}";
    }

    protected void ChangeState(GameObject go) {
        _state = Util.NullCheck(go) ?  Define.TowerState.Idle : Define.TowerState.Attack;
    }

    private void Update() {
        if (!GameSystem.Instance.IsPlay())
            return;

        switch (_state) {
            case Define.TowerState.Idle:
                OnIdleUpdate();
                break;

            case Define.TowerState.Attack:
                OnAttackUpdate();
                break;
        }
    }

    protected abstract void OnIdleUpdate();
    protected void OnAttackUpdate() {
        if (!_targetEnemy) {
            _targetEnemy = null;
            ChangeState(_targetEnemy);
            return;
        }

        _currentAttackDelay += Time.deltaTime;

        if (_status.AttackDelay > _currentAttackDelay)
            return;

        OnAttackEvent();
    }

    protected virtual void OnAttackEvent() {
        if (!_targetEnemy) {
            ChangeState(null);
            return;
        }

        if (_targetEnemy.GetComponentInParent<EnemyController>().CurrentHp <= 0) {
            _targetEnemy = null;
            ChangeState(_targetEnemy);
            return;
        }
    }

    private void OnTriggerStay(Collider c) {
        if (!c.CompareTag("Enemy"))
            return;

        if (!Util.NullCheck(_targetEnemy))
            return;

        if (c.transform.parent.position.x > LimitPosRight ||
            c.transform.parent.position.x < LimitPosLeft)
            return;

        _targetEnemy = c.gameObject;
        ChangeState(_targetEnemy);
    }

    private void OnTriggerExit(Collider c) {
        if(!c.CompareTag("Enemy"))
            return;

        if (Util.NullCheck(_targetEnemy))
            return;

        if (c.gameObject != _targetEnemy)
            return;

        _targetEnemy = null;
        ChangeState(_targetEnemy);
    }
}
