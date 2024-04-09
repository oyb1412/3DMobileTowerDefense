using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class TowerControllerBase : MonoBehaviour
{
    private SphereCollider _attackRangeCollider;
    private const float LimitPosRight = 75f;
    private const float LimitPosLeft = -105f;
    [SerializeField] private List<GameObject> _targets = new List<GameObject>();
    protected GameObject _base;
    protected string _projectilePath;
    protected Transform _firePoint;

    [SerializeField]protected GameObject _targetEnemy;
    protected TowerStatus _status;
    [SerializeField]private Define.TowerState _state;
    protected float _currentAttackDelay;
    protected GameObject _projectileObject;

    private void Awake() {
        _status = GetComponentInParent<TowerStatus>();
        _attackRangeCollider = GetComponent<SphereCollider>();
    }
    private void Start() {
        Init();
        _base = GetComponentInParent<TowerBase>().gameObject;
        _state = Define.TowerState.Idle;
    }

    protected virtual void Init() {
        _firePoint = Util.FindChild(transform.parent.gameObject, "FirePoint", false).transform;
        _attackRangeCollider.radius = _status.AttackRange * (GameSystem.TowerAttackRangeImageSize * .5f);
        _projectilePath = $"Projectile/{_status.TowerType.ToString()}(Mobile)/{_status.TowerType.ToString()}ProjectileLvl{_status.Level}";
    }

    protected void ChangeState(bool trigger) {
        _state = trigger ?  Define.TowerState.Attack : Define.TowerState.Idle;
    }

    private void OnEnable() {
        _currentAttackDelay = _status.AttackDelay;
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

    protected void OnIdleUpdate() {
        if (_targets.Count > 0)
            ChangeState(true);
    }
    protected void OnAttackUpdate() {
        _currentAttackDelay += Time.deltaTime;

        if (_status.AttackDelay > _currentAttackDelay)
            return;

        if (!Util.NullCheck(_targetEnemy) &&
            Vector2.Distance(transform.position, _targetEnemy.transform.position) >= _status.AttackRange) {
            _targetEnemy = GetFirstEnemy();
        }

        if (Util.NullCheck(_targetEnemy)) {
            _targetEnemy = GetFirstEnemy();
        }

        if(Util.NullCheck(_targetEnemy)) {
            ChangeState(false);
            return;
        }


        OnAttackEvent();
    }

    protected virtual void OnAttackEvent() {
        if (_targets.Count <= 0) {
            ChangeState(false);
            return;
        }

        if (Util.NullCheck(_targetEnemy)) {
            ChangeState(false);
            return;
        }

        if (_targetEnemy.GetComponentInParent<EnemyController>().CurrentHp <= 0) {
            _targetEnemy = null;
            ChangeState(false);
            return;
        }
    }

    private GameObject GetFirstEnemy() {
        if (_targets.Count <= 0) {
            ChangeState(false);
            return null;
        }

        GameObject firstTarget = _targets[0];

        for (int i = 0; i < _targets.Count; i++) {
            if (Util.NullCheck(_targets[i])) {
                _targets.RemoveAt(i);
                continue;
            }
            try {
                if (_targets[i].GetComponent<EnemyStatus>().Number < firstTarget.GetComponent<EnemyStatus>().Number &&
                    _targets[i].GetComponentInParent<EnemyController>().CurrentHp > 0) {
                    firstTarget = _targets[i];
                }
            }
            catch(Exception e) {
                Debug.Log($"{e.ToString()}에러 발생.{i}번째");
            }
            
        }
        return firstTarget;
    }

    private void OnTriggerStay(Collider c) {
        if (!c.CompareTag("Enemy"))
            return;

        if (c.transform.parent.position.x > LimitPosRight ||
            c.transform.parent.position.x < LimitPosLeft)
            return;

        if (_targets.Contains(c.gameObject))
            return;

        _targets.Add(c.gameObject);
    }

    private void OnTriggerExit(Collider c) {
        if (!c.CompareTag("Enemy"))
            return;

        if (!_targets.Contains(c.gameObject))
            return;

        _targets.Remove(c.gameObject);
    }
}
