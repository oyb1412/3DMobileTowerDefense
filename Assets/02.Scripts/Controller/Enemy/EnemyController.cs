using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
    private const float _EnemyAnimationFadeTime = .3f;
    private const float _aollowRange = 1f;
    private Animator _animator;
    private EnemyStatus _status;
    private Define.EnemyState _state;
    private int _moveIndex;
    private int _lastMoveIndex;
    private CapsuleCollider _collider;
    private Transform[] _movePoint;
    public Action<int, int> OnHpEvent;
    public Action OnDieEvent;
    private Tween _moveTween;
    private Tween _rotateTween;

    public int CurrentHp { get { return _status.CurrentHp; } }
    public Define.EnemyState State {
        get { return _state; }
        set {
            if (_state == value)
                return;

            if(value == Define.EnemyState.Die) {
                _collider.enabled = false;
                StopAllBehaivoir();
            }

            _animator.CrossFade(value.ToString(), _EnemyAnimationFadeTime);
            _state = value;
        }
    }
    
 
    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponentInChildren<CapsuleCollider>();
    }

    private void Start() {
        _status = GetComponentInChildren<EnemyStatus>();
        _status.Init();
        State = Define.EnemyState.Move;
        Transform movePoints = GameObject.Find("MovePoints").transform;
        _movePoint = new Transform[movePoints.childCount];
        _lastMoveIndex = _movePoint.Length;
        for (int i = 0; i< movePoints.childCount; i++) {
            _movePoint[i] = movePoints.GetChild(i).transform;
        }

        transform.LookAt(_movePoint[_moveIndex].position);

        StartCoroutine(CoMove());
    }

    private void StopAllBehaivoir() {
        StopAllCoroutines();
        _moveTween?.Kill(false);
        _rotateTween?.Kill(false);
        DOTween.Kill(gameObject, false);
    }
    public void TakeDamage(int damage) {
        _status.CurrentHp += damage;

        OnHpEvent?.Invoke(_status.CurrentHp , _status.MaxHp);

        if (_status.CurrentHp <= 0) {
            OnDieEvent?.Invoke();
            GameSystem.Instance.SetGold(_status.ProvideGold);
            State = Define.EnemyState.Die;
        }
    }

    IEnumerator CoMove() {

        Vector3 dir = _movePoint[_moveIndex].position - transform.position;

        float moveTime = Mathf.Max(dir.magnitude / _status.MoveSpeed, 1f);

        _moveTween = transform.DOMove(_movePoint[_moveIndex].position, moveTime).SetEase(Ease.Linear);

        yield return _moveTween.WaitForCompletion();

        _moveIndex++;

        if (_movePoint[_moveIndex - 1].position.x < _movePoint[_moveIndex].position.x && _movePoint[_moveIndex - 1].position.z == _movePoint[_moveIndex].position.z) {
            _rotateTween = transform.DORotateQuaternion(Quaternion.Euler(0f, 90f, 0f), .3f).SetEase(Ease.Linear);
        } else if (_movePoint[_moveIndex - 1].position.x > _movePoint[_moveIndex].position.x && _movePoint[_moveIndex - 1].position.z == _movePoint[_moveIndex].position.z) {
            _rotateTween = transform.DORotateQuaternion(Quaternion.Euler(0f, -90f, 0f), .3f).SetEase(Ease.Linear);
        } else if (_movePoint[_moveIndex - 1].position.z > _movePoint[_moveIndex].position.z && _movePoint[_moveIndex - 1].position.x == _movePoint[_moveIndex].position.x) {
            _rotateTween = transform.DORotateQuaternion(Quaternion.Euler(0f, 180f, 0f), .3f).SetEase(Ease.Linear);
        } else if (_movePoint[_moveIndex - 1].position.z < _movePoint[_moveIndex].position.z && _movePoint[_moveIndex - 1].position.x == _movePoint[_moveIndex].position.x) {
            _rotateTween = transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 0f), .3f).SetEase(Ease.Linear);
        }

        if(_moveIndex == _lastMoveIndex) {
            GameSystem.Instance.SetGameHp(-1);
            StopAllBehaivoir();
            Managers.Resources.Destroy(gameObject);
        }

        if (State != Define.EnemyState.Move)
            StopAllBehaivoir();
        else
            StartCoroutine(CoMove());
    }
}
