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
    private int _lastMoveIndex;
    private CapsuleCollider _collider;
    private Transform[] _movePoint;
    private Transform _movePoints;
    public Action OnRewardEvent;
    public Action<int, int> OnHpEvent;
    private Tween _moveTween;
    private Tween _rotateTween;
    private ParticleSystem _arriveEffect;


    public int MoveIndex { get; set; }

    public EnemyStatus Status => _status;
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



    public void Init(Vector3 pos, int number, int moveIndex = 0) {
        transform.position = pos;
        _collider.enabled = true;
        _status.Init(number);
        State = Define.EnemyState.Move;
        MoveIndex = moveIndex;
        transform.LookAt(_movePoint[MoveIndex].position);
        StartCoroutine(CoMove());
        GetComponentInChildren<UIEnemy>().Init();
    }

    public void Init(Vector3 pos, int move, int hp, int number) {
        transform.position = pos;
        _collider.enabled = true;
        _status.Init(number);
        _status.CurrentHp = hp;
        State = Define.EnemyState.Move;
        MoveIndex = move;
        transform.LookAt(_movePoint[MoveIndex].position);
        StartCoroutine(CoMove());
        GetComponentInChildren<UIEnemy>().Init();
    }


    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponentInChildren<CapsuleCollider>();
        _status = GetComponentInChildren<EnemyStatus>();
        _movePoints = GameObject.Find("MovePoints").transform;
        _movePoint = new Transform[_movePoints.childCount];
        for (int i = 0; i < _movePoints.childCount; i++) {
            _movePoint[i] = _movePoints.GetChild(i).transform;
        }
        _lastMoveIndex = _movePoint.Length;

    }

    private void Start() {
    }

    private void Update() {
        if (State != Define.EnemyState.Move ||
            !GameSystem.Instance.IsPlay())
            StopAllBehaivoir();
    }

    private void StopAllBehaivoir() {
        StopAllCoroutines();
        _moveTween?.Kill(false);
        _rotateTween?.Kill(false);
        DOTween.Kill(gameObject, false);
    }
    public void TakeDamage(int damage, GameObject attacker) {
        TowerBase tower = attacker.GetComponent<TowerBase>();
        int physicsDefense = _status.PhysicsDefense;
        int magicDefense = _status.MagicDefense;
        var type = tower.TowerStatus.TowerType;

        if (type == Define.TowerType.ArcherTower || type == Define.TowerType.CanonTower)
            damage += physicsDefense;
        else if (type == Define.TowerType.MagicTower)
            damage += magicDefense;
        else {
            damage += (physicsDefense / 5);
            damage += (magicDefense / 5);
        }
        damage = Mathf.Min(damage, 1);

        _status.CurrentHp += damage;

        OnHpEvent?.Invoke(_status.CurrentHp , _status.MaxHp);

        if (_status.CurrentHp <= 0) {
            OnRewardEvent?.Invoke();
            attacker.GetComponent<TowerBase>().SetKill();
            GameSystem.Instance.SetGold(_status.ProvideGold);
            GameSystem.Instance.SetScore(_status.ProvideScore);
            State = Define.EnemyState.Die;
        }
    }

    IEnumerator CoMove() {

        Vector3 dir = _movePoint[MoveIndex].position - transform.position;

        float moveTime = Mathf.Max(dir.magnitude / _status.MoveSpeed, 1f);

        _moveTween = transform.DOMove(_movePoint[MoveIndex].position, moveTime).SetEase(Ease.Linear);

        yield return _moveTween.WaitForCompletion();

        MoveIndex++;

        if (MoveIndex == _lastMoveIndex && gameObject.activeInHierarchy) {
            Managers.MainCamera.CameraShake();
            Managers.Audio.PlaySfx(Define.SfxType.EnemyArrive);
            GameSystem.Instance.SetGameHp(-10);
            State = Define.EnemyState.Die;
        }

        if(MoveIndex < _lastMoveIndex) {
            if (_movePoint[MoveIndex - 1].position.x < _movePoint[MoveIndex].position.x && _movePoint[MoveIndex - 1].position.z == _movePoint[MoveIndex].position.z) {
                _rotateTween = transform.DORotateQuaternion(Quaternion.Euler(0f, 90f, 0f), .3f).SetEase(Ease.Linear);
            } else if (_movePoint[MoveIndex - 1].position.x > _movePoint[MoveIndex].position.x && _movePoint[MoveIndex - 1].position.z == _movePoint[MoveIndex].position.z) {
                _rotateTween = transform.DORotateQuaternion(Quaternion.Euler(0f, -90f, 0f), .3f).SetEase(Ease.Linear);
            } else if (_movePoint[MoveIndex - 1].position.z > _movePoint[MoveIndex].position.z && _movePoint[MoveIndex - 1].position.x == _movePoint[MoveIndex].position.x) {
                _rotateTween = transform.DORotateQuaternion(Quaternion.Euler(0f, 180f, 0f), .3f).SetEase(Ease.Linear);
            } else if (_movePoint[MoveIndex - 1].position.z < _movePoint[MoveIndex].position.z && _movePoint[MoveIndex - 1].position.x == _movePoint[MoveIndex].position.x) {
                _rotateTween = transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 0f), .3f).SetEase(Ease.Linear);
            }
        }

        if(MoveIndex < _lastMoveIndex && gameObject.activeInHierarchy)
            StartCoroutine(CoMove());
    }
}
