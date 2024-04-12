using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� Ÿ�� ����
/// </summary>
public abstract class TowerControllerBase : MonoBehaviour
{
    private const float LIMIT_RIGHTPOS = 75f;  //�ش� ��ġ���� x���� ���� ���� Ÿ���� ���� ����
    private const float LIMIT_LEFTPOS = -105f;  //�ش� ��ġ���� x���� ���� ���� Ÿ���� ���� ����

    private SphereCollider _attackRangeCollider;  //Ÿ�� ������ �ݶ��̴�
    private List<GameObject> _targets = new List<GameObject>();  //���� ������ ���� �� ����Ʈ

    protected float _currentAttackDelay;  //���� ���� ������
    protected string _projectilePath;  //������ �߻�ü�� ��ġ�� path
    //TODO �߻�ü �̸� ROAD�ص� �� �����ϴ� ������� ����
    //�߻�ü �н���, string.format���� ������ json�� ����

    protected GameObject _base; //Ÿ�� ���̽� 
    protected Transform _firePoint;  //�߻�ü ���� ��ġ

    protected GameObject _targetEnemy;  //���� ���
    protected TowerStatus _status;  //Ÿ�� �ɷ�ġ
    private Define.TowerState _state = Define.TowerState.Idle;  //Ÿ���� ����
    protected GameObject _projectileObject;  //�߻�ü ������Ʈ

    private void Awake() {
        _status = GetComponentInParent<TowerStatus>();  //Ÿ���� �ɷ�ġ ����
        _attackRangeCollider = GetComponent<SphereCollider>();  //Ÿ���� �ݶ��̴� GET
        _base = GetComponentInParent<TowerBase>().gameObject;  //Ÿ�� ���̽� ����
    }

    private void Start() => Init();  //�⺻ ���� �ʱ�ȭ

    /// <summary>
    /// Ÿ�� �⺻ ���� �ʱ�ȭ
    /// </summary>
    protected virtual void Init() {
        _firePoint = Util.FindChild(transform.parent.gameObject, Define.FIREPOINT, false).transform;  //Ÿ�� �߻� ��ġ FIND
        _attackRangeCollider.radius = _status.AttackRange * (GameSystem.TowerAttackRangeImageSize * .5f);  //Ÿ�� ���� ��Ÿ� �ð������� ǥ���ϱ� ���� ũ�� ����
        _projectilePath = $"Projectile/{_status.TowerType.ToString()}/{_status.TowerType.ToString()}ProjectileLvl{_status.Level}";  //Ÿ�� �߻�ü �н� ����
    }

    /// <summary>
    /// Ÿ�� ���� ����
    /// </summary>
    /// <param name="state">������ ����</param>
    protected void ChangeState(Define.TowerState state) => _state = state;

    /// <summary>
    /// POOL�� ���� ���Ӱ� Active���� �� ������ �ʱ�ȭ
    /// </summary>
    private void OnEnable() => _currentAttackDelay = _status.AttackDelay;

    private void Update() {
        if (!GameSystem.Instance.IsPlay())  //���� ���� �� ����
            return;

        switch (_state) {  //�� ���¿� �´� �ൿ ȣ��
            case Define.TowerState.Idle:
                OnIdleUpdate();
                break;

            case Define.TowerState.Attack:
                OnAttackUpdate();
                break;
        }
    }

    /// <summary>
    /// Idle����
    /// </summary>
    protected void OnIdleUpdate() {
        if (_targets.Count > 0)  //������ ���� ������ Attack ���·� ��ȯ
            ChangeState(Define.TowerState.Attack);
    }

    /// <summary>
    /// Attack����
    /// </summary>
    protected void OnAttackUpdate() {
        _currentAttackDelay += Time.deltaTime;  //���� ������ ���

        if (_status.AttackDelay > _currentAttackDelay)  //���� ���ǿ� ���յǱ� ������ return
            return;

        if (Vector2.Distance(transform.position, _targetEnemy.transform.position) >= _status.AttackRange &&
            !Util.NullCheck(_targetEnemy)) {  //���� ���� ��� ���� NULL���°ų�, ���� �������� �����
            _targetEnemy = GetFirstEnemy();  //���Ӱ� ���� ����� ����
        }

        if (Util.NullCheck(_targetEnemy)) {  //���� ���� ��� ���� NULL���¸�
            _targetEnemy = GetFirstEnemy();  //���Ӱ� ���� ����� ����
        }

        if(Util.NullCheck(_targetEnemy)) {  //���� ���� ��� ���� NULL���¸�
            ChangeState(Define.TowerState.Idle);  //���� ��ȯ
            return;
        }

        OnAttackEvent();  //���� �̺�Ʈ ȣ��
    }

    /// <summary>
    /// Ÿ�� �⺻ ���� �̺�Ʈ
    /// </summary>
    protected virtual void OnAttackEvent() {
        if (_targets.Count <= 0 || Util.NullCheck(_targetEnemy)) {  //������ ���� ���ų�, ���� ����� NULL���¸�
            ChangeState(Define.TowerState.Idle);  //Idle���·� ��ȯ
            return;
        }

        if (_targetEnemy.GetComponentInParent<EnemyController>().CurrentHp <= 0) {  //���� ����� ü���� ����� ���ٸ�
            _targetEnemy = null;  //���� ��󿡼� ����
            ChangeState(Define.TowerState.Idle);  //Idle���·� ��ȯ
            return;
        }
    }

    /// <summary>
    /// ���� �ռ������� ���� ����
    /// </summary>
    /// <returns></returns>
    private GameObject GetFirstEnemy() {
        if (_targets.Count <= 0) {  //������ ���� ���ٸ�
            ChangeState(Define.TowerState.Idle);  //Idle���·� ��ȯ
            return null;
        }

        GameObject firstTarget = _targets[0];  //ù��° ���� ����

        for (int i = 0; i < _targets.Count; i++) {  //�������ķ� ���� ���ο� �ִ� ���� ����
            if (Util.NullCheck(_targets[i])) {  //TODO �ֳʹ� ���� ��üũ�� ����, ü�µ� ���ÿ� üũ
                _targets.RemoveAt(i);  //�ֳʹ̰� NULL���¸�, ����Ʈ���� ���� �� �ǳʶ�
                continue;
            }
            if (_targets[i].GetComponent<EnemyStatus>().Number < firstTarget.GetComponent<EnemyStatus>().Number &&
                _targets[i].GetComponentInParent<EnemyController>().CurrentHp > 0) { 
                firstTarget = _targets[i];
            }
        }
        return firstTarget;  //������ ���� return
    }

    /// <summary>
    /// ���� �������� ����� �����Ǿ��� ��
    /// </summary>
    private void OnTriggerStay(Collider c) {
        if (!c.CompareTag(Define.TAG_ENEMY))  //������ ����� �ֳʹ̰� �ƴϸ� return
            return;

        if (c.transform.parent.position.x > LIMIT_RIGHTPOS ||  //������ �ֳʹ��� ��ġ�� ���Ѱ� ���̸� return
            c.transform.parent.position.x < LIMIT_LEFTPOS)
            return;

        if (_targets.Contains(c.gameObject))  //������ �ֳʹ̰� �̹� ����Ʈ�� ���� �� return
            return;

        _targets.Add(c.gameObject);  //����Ʈ�� ������ �� ���
    }

    /// <summary>
    /// ���� �������� ����� ����� ��
    /// </summary>
    private void OnTriggerExit(Collider c) {
        if (!c.CompareTag(Define.TAG_ENEMY))  //��� ����� �ֳʹ̰� �ƴϸ� return;
            return;

        if (!_targets.Contains(c.gameObject))  //��� ����� ����Ʈ�� �������� ���� �� return;
            return;

        _targets.Remove(c.gameObject);  //����� ����Ʈ���� ����
    }
}
