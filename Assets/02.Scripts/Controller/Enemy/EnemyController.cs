using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// �ֳʹ� ���� ��Ʈ�ѷ�
/// </summary>
public class EnemyController : MonoBehaviour
{
    private const int ENEMY_DAMAGE = 10;  //�ֳʹ̰� ���������� ���� �� �÷��̾ ���� ������
    private const float ENEMY_ANIMATION_FADETIME = .3f;  //�ִϸ��̼� ��ȯ �ð�
    private const float ENEMY_ROTATETIME = .3f;  //�ֳʹ� ȸ�� �ð�

    private Animator _animator;  //�� �ִϸ�����
    private EnemyStatus _status;  //�ɷ�ġ 
    private Define.EnemyState _state;  //����
    private CapsuleCollider _collider;  //�� �ݶ��̴�
    private Transform[] _movePoint;  //������ �̵��� �� ����Ʈ
    private Transform _movePoints;  //�̵��� ����Ʈ���� �θ�
    public Action OnRewardEvent;  //����� ȣ��Ǵ� ������ ���� �̺�Ʈ
    public Action<int, int> OnHpEvent;  //����� ȣ��Ǵ� UI������ ���� �̺�Ʈ
    private Tween _moveTween;  //���� �̵����� Ʈ��
    private Tween _rotateTween;  //���� ȸ������ Ʈ��

    private int _lastMoveIndex;  //���� ���� �ε��� ��ȣ

    public int MoveIndex { get; set; }  //������ ���� �̵����� ����Ʈ �ε���
    public EnemyStatus Status => _status;  //�ɷ�ġ ������Ƽ
    public int CurrentHp { get { return _status.CurrentHp; } }  //���� ü�� ������Ƽ

    /// <summary>
    /// ���� ��ȯ ������Ƽ
    /// ����� �ݶ��̴� ���� �� ��� �ൿ ����
    /// ���� ��ȯ �� �ִϸ����� ��ȯ
    /// </summary>
    public Define.EnemyState State {  
        get { return _state; }
        set {
            if (_state == value)
                return;

            if(value == Define.EnemyState.Die) {
                _collider.enabled = false;
                StopAllBehaivoir();
            }

            _animator.CrossFade(value.ToString(), ENEMY_ANIMATION_FADETIME);
            _state = value;
        }
    }

    private void OnEnable() {
        if(_collider == null)
            _collider = _collider.GetComponentInChildren<CapsuleCollider>();

        _collider.enabled = true;
    }

    /// <summary>
    /// �⺻ �ʱ�ȭ
    /// �Ű������� �ʿ� ���� ���� �ʱ�ȭ ������ �ʱ�ȭ
    /// </summary>
    private void Awake() {
        _animator = GetComponentInChildren<Animator>();  //�� �ִϸ����� Get
        _collider = GetComponentInChildren<CapsuleCollider>();  //�� �ݶ��̴� Get
        _status = GetComponentInChildren<EnemyStatus>();  //�� �ɷ�ġ Get
        _movePoints = GameObject.Find(Define.MOVE_POINT).transform;  //���̾��Ű â���� �̵� ����Ʈ �θ� Find
        _movePoint = new Transform[_movePoints.childCount];  //���̾��Ű â�� �̵� ����Ʈ ������ŭ �̵� ����Ʈ ���� ũ�⸦ �ʱ�ȭ
        for (int i = 0; i < _movePoints.childCount; i++) {
            _movePoint[i] = _movePoints.GetChild(i).transform;  //�� �̵� ����Ʈ�� ��ġ�� �̵� ����Ʈ ������ �ο�
        }
        _lastMoveIndex = _movePoint.Length;  //������ �̵� ����Ʈ�� �ο�
    }

    /// <summary>
    /// �ֳʹ� �ʱ�ȭ �Լ�
    /// �ֳʹ� ������ ȣ��
    /// ��ġ, �ݶ��̴� ����
    /// </summary>
    /// <param name="pos">������ų �ֳʹ� ��ġ</param>
    /// <param name="number">�ֳʹ� �ڵ� �ε���</param>
    public void Init(Vector3 pos, int number) {
        transform.position = pos;  //��ġ �ʱ�ȭ
        _status.Init(number);  //�ɷ�ġ �ʱ�ȭ �� ���� �ڵ� �ο�
        State = Define.EnemyState.Move;  //�⺻ ���� Move�� ����
        MoveIndex = 0;  //ù �̵� ����Ʈ�� 0�� ����Ʈ�� ����
        transform.LookAt(_movePoint[MoveIndex].position);  //�̵� ����Ʈ�� ȸ��
        StartCoroutine(CoMove());  //�̵� �ڷ�ƾ ����
        GetComponentInChildren<UIEnemy>().Init();  //�� ���ֿ� �ο��� UI �ʱ�ȭ
    }

    /// <summary>
    /// ������Ʈ �Լ�
    /// �ֳʹ��� ���¿� ���� ���¸� üũ
    /// </summary>
    private void Update() {
        if (State != Define.EnemyState.Move ||
            !GameSystem.Instance.IsPlay())  //�ֳʹ̰� �̵� ���°� �ƴϰų�, ������ ����Ǹ� �ֳʹ��� ��� �ൿ ����
            StopAllBehaivoir();
    }

    /// <summary>
    /// ��� �ൿ ����
    /// </summary>
    private void StopAllBehaivoir() {
        StopAllCoroutines();  //��� �ڷ�ƾ ����
        _moveTween?.Kill(false);  //�̵� Ʈ�� ����
        _rotateTween?.Kill(false);  //ȸ�� Ʈ�� ����
        DOTween.Kill(gameObject, false);  //�������� �� �ִ� ��� Ʈ�� ���� ����
    }

    /// <summary>
    /// ���� �޾��� �� ȣ��
    /// </summary>
    /// <param name="damage">���� ������</param>
    /// <param name="attacker">������ ����� gameobject</param>
    public void TakeDamage(int damage, GameObject attacker) {
        TowerBase tower = attacker.GetComponent<TowerBase>();  //������ ����� Ÿ�� ������ ����
        if (tower == null)
            return;

        int physicsDefense = _status.PhysicsDefense;  //���� ���� 
        int magicDefense = _status.MagicDefense;  //���� ����
        var type = tower.TowerStatus.TowerType;  //������ Ÿ���� Ÿ��

        if (type == Define.TowerType.ArcherTower || type == Define.TowerType.CanonTower)  //������ Ÿ���� Ÿ�Կ� ���� ������ ���
            damage += physicsDefense;
        else if (type == Define.TowerType.MagicTower)
            damage += magicDefense;
        else {
            damage += (physicsDefense / 5);
            damage += (magicDefense / 5);
        }

        damage = Mathf.Min(damage, -1);  //�������� �׻� 1�̻�

        _status.CurrentHp += damage;  //ü�� ����

        OnHpEvent?.Invoke(_status.CurrentHp , _status.MaxHp);  //����ü��, �ִ�ü���� ������� UI�̺�Ʈ�� ����� �̺�Ʈ ȣ��

        if (_status.CurrentHp <= 0) {  //�ֳʹ� ��� ��
            OnRewardEvent?.Invoke();  //ȭ�鿡 ��� ���� UI ����
            tower.SetKill();  //óġ�� Ÿ���� �̺�Ʈ ȣ�� 
            GameSystem.Instance.SetGold(_status.RewardGold);  //��� ����
            GameSystem.Instance.SetScore(_status.RewardScore);  //���� ����
            State = Define.EnemyState.Die;  //���� ��ȯ
        }
    }

    /// <summary>
    /// �̵� �ڷ�ƾ
    /// ������ ��ġ�� ������ �� ���� ���ȣ��
    /// </summary>
    IEnumerator CoMove() {

        Vector3 dir = _movePoint[MoveIndex].position - transform.position;  //���� �̵� ����Ʈ������ ����

        float moveTime = Mathf.Max(dir.magnitude / _status.MoveSpeed, 1f);  //���� �̵� ����Ʈ������ �̵� �ð��� ���

        _moveTween = transform.DOMove(_movePoint[MoveIndex].position, moveTime).SetEase(Ease.Linear);  //���� �̵� ����Ʈ���� ������ �ӵ��� �̵�

        yield return _moveTween.WaitForCompletion();  //���� �̵� ����Ʈ���� �̵��� �� ���� ���

        MoveIndex++;  //�̵� �Ϸ� �� �̵� ����Ʈ ������ ���� Index�� 1 �߰�

        if (gameObject.activeInHierarchy) {  //�ֳʹ� ������Ʈ ���� üũ
            if (MoveIndex >= _lastMoveIndex) {  //���� ������ ����
                Managers.MainCamera.CameraShake();  //ī�޶� ����ũ ȿ��
                Managers.Audio.PlaySfx(Define.SfxType.EnemyArrive);  //�ֳʹ� ������ ��� ȿ���� ���
                GameSystem.Instance.SetGameHp(ENEMY_DAMAGE);  //�ֳʹ� ���� �� �÷��̾� Hp ����
                State = Define.EnemyState.Die;  //���� ����
            }
            else {  //���� �������� �ʾҴٸ�
                Rotate(_movePoint[MoveIndex - 1].position, _movePoint[MoveIndex].position);  //���� ��ġ�� ȸ��
                StartCoroutine(CoMove());  //���� ��ġ�� �̵�
            }
        }
    }

    /// <summary>
    /// �ֳʹ��� ��ġ�� ���� �̵� ���� ��ġ�� ������� ȸ���� ���
    /// </summary>
    /// <param name="currentPos">���� ��ġ</param>
    /// <param name="targetPos">���� �̵� ���� ��ġ</param>
    private void Rotate(Vector3 currentPos, Vector3 targetPos) {
        float x = targetPos.x - currentPos.x;
        float z = targetPos.z - currentPos.z;

        float angle = 0f;

        if (x > 0 && z == 0) angle = 90f;  // �������� ȸ��
        else if (x < 0 && z == 0) angle = -90f;  // �������� ȸ��
        else if (z > 0 && x == 0) angle = 0f;  // �������� ȸ��
        else if (z < 0 && x == 0) angle = 180f;  // �������� ȸ��

        _rotateTween = transform.DORotateQuaternion(Quaternion.Euler(0f, angle, 0f), ENEMY_ROTATETIME).SetEase(Ease.Linear);
    }
}
