using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 타워 관리
/// </summary>
public abstract class TowerControllerBase : MonoBehaviour
{
    private const float LIMIT_RIGHTPOS = 75f;  //해당 위치보다 x값이 높은 적은 타겟이 되지 않음
    private const float LIMIT_LEFTPOS = -105f;  //해당 위치보다 x값이 낮은 적은 타겟이 되지 않음

    private SphereCollider _attackRangeCollider;  //타겟 감지용 콜라이더
    private List<GameObject> _targets = new List<GameObject>();  //공격 범위에 들어온 적 리스트

    protected float _currentAttackDelay;  //현재 공격 딜레이
    protected string _projectilePath;  //생성할 발사체가 위치한 path
    //TODO 발사체 미리 ROAD해둔 후 생성하는 방식으로 변경
    //발사체 패스도, string.format으로 모조리 json에 저장

    protected GameObject _base; //타워 베이스 
    protected Transform _firePoint;  //발사체 생성 위치

    protected GameObject _targetEnemy;  //공격 대상
    protected TowerStatus _status;  //타워 능력치
    private Define.TowerState _state = Define.TowerState.Idle;  //타워의 상태
    protected GameObject _projectileObject;  //발사체 오브젝트

    private void Awake() {
        _status = GetComponentInParent<TowerStatus>();  //타워의 능력치 추출
        _attackRangeCollider = GetComponent<SphereCollider>();  //타워의 콜라이더 GET
        _base = GetComponentInParent<TowerBase>().gameObject;  //타워 베이스 추출
    }

    private void Start() => Init();  //기본 설정 초기화

    /// <summary>
    /// 타워 기본 설정 초기화
    /// </summary>
    protected virtual void Init() {
        _firePoint = Util.FindChild(transform.parent.gameObject, Define.FIREPOINT, false).transform;  //타워 발사 위치 FIND
        _attackRangeCollider.radius = _status.AttackRange * (GameSystem.TowerAttackRangeImageSize * .5f);  //타워 공격 사거리 시각적으로 표현하기 위한 크기 설정
        _projectilePath = $"Projectile/{_status.TowerType.ToString()}/{_status.TowerType.ToString()}ProjectileLvl{_status.Level}";  //타워 발사체 패스 지정
    }

    /// <summary>
    /// 타워 상태 변경
    /// </summary>
    /// <param name="state">변경할 상태</param>
    protected void ChangeState(Define.TowerState state) => _state = state;

    /// <summary>
    /// POOL로 인해 새롭게 Active됐을 때 딜레이 초기화
    /// </summary>
    private void OnEnable() => _currentAttackDelay = _status.AttackDelay;

    private void Update() {
        if (!GameSystem.Instance.IsPlay())  //게임 종료 시 정지
            return;

        switch (_state) {  //각 상태에 맞는 행동 호출
            case Define.TowerState.Idle:
                OnIdleUpdate();
                break;

            case Define.TowerState.Attack:
                OnAttackUpdate();
                break;
        }
    }

    /// <summary>
    /// Idle상태
    /// </summary>
    protected void OnIdleUpdate() {
        if (_targets.Count > 0)  //감지된 적이 있으면 Attack 상태로 전환
            ChangeState(Define.TowerState.Attack);
    }

    /// <summary>
    /// Attack상태
    /// </summary>
    protected void OnAttackUpdate() {
        _currentAttackDelay += Time.deltaTime;  //공격 딜레이 계산

        if (_status.AttackDelay > _currentAttackDelay)  //공격 조건에 적합되기 전까지 return
            return;

        if (Vector2.Distance(transform.position, _targetEnemy.transform.position) >= _status.AttackRange &&
            !Util.NullCheck(_targetEnemy)) {  //현재 공격 대상 적이 NULL상태거나, 공격 범위에서 벗어나면
            _targetEnemy = GetFirstEnemy();  //새롭게 공격 대상을 지정
        }

        if (Util.NullCheck(_targetEnemy)) {  //현재 공격 대상 적이 NULL상태면
            _targetEnemy = GetFirstEnemy();  //새롭게 공격 대상을 지정
        }

        if(Util.NullCheck(_targetEnemy)) {  //현재 공격 대상 적이 NULL상태면
            ChangeState(Define.TowerState.Idle);  //상태 변환
            return;
        }

        OnAttackEvent();  //공격 이벤트 호출
    }

    /// <summary>
    /// 타워 기본 공격 이벤트
    /// </summary>
    protected virtual void OnAttackEvent() {
        if (_targets.Count <= 0 || Util.NullCheck(_targetEnemy)) {  //감지된 적이 없거나, 공격 대상이 NULL상태면
            ChangeState(Define.TowerState.Idle);  //Idle상태로 전환
            return;
        }

        if (_targetEnemy.GetComponentInParent<EnemyController>().CurrentHp <= 0) {  //공격 대상의 체력이 충분히 낮다면
            _targetEnemy = null;  //공격 대상에서 제거
            ChangeState(Define.TowerState.Idle);  //Idle상태로 전환
            return;
        }
    }

    /// <summary>
    /// 가장 앞서나가는 적을 추출
    /// </summary>
    /// <returns></returns>
    private GameObject GetFirstEnemy() {
        if (_targets.Count <= 0) {  //감지된 적이 없다면
            ChangeState(Define.TowerState.Idle);  //Idle상태로 전환
            return null;
        }

        GameObject firstTarget = _targets[0];  //첫번째 적을 저장

        for (int i = 0; i < _targets.Count; i++) {  //삽입정렬로 가장 선두에 있는 적을 추출
            if (Util.NullCheck(_targets[i])) {  //TODO 애너미 전용 널체크를 만들어서, 체력도 동시에 체크
                _targets.RemoveAt(i);  //애너미가 NULL상태면, 리스트에서 제거 후 건너뜀
                continue;
            }
            if (_targets[i].GetComponent<EnemyStatus>().Number < firstTarget.GetComponent<EnemyStatus>().Number &&
                _targets[i].GetComponentInParent<EnemyController>().CurrentHp > 0) { 
                firstTarget = _targets[i];
            }
        }
        return firstTarget;  //선두의 적을 return
    }

    /// <summary>
    /// 공격 범위내에 대상이 감지되었을 시
    /// </summary>
    private void OnTriggerStay(Collider c) {
        if (!c.CompareTag(Define.TAG_ENEMY))  //감지된 대상이 애너미가 아니면 return
            return;

        if (c.transform.parent.position.x > LIMIT_RIGHTPOS ||  //감지된 애너미의 위치가 제한값 밖이면 return
            c.transform.parent.position.x < LIMIT_LEFTPOS)
            return;

        if (_targets.Contains(c.gameObject))  //감지된 애너미가 이미 리스트에 존재 시 return
            return;

        _targets.Add(c.gameObject);  //리스트에 감지된 적 등록
    }

    /// <summary>
    /// 공격 범위에서 대상이 벗어났을 시
    /// </summary>
    private void OnTriggerExit(Collider c) {
        if (!c.CompareTag(Define.TAG_ENEMY))  //벗어난 대상이 애너미가 아니면 return;
            return;

        if (!_targets.Contains(c.gameObject))  //벗어난 대상이 리스트에 존재하지 않을 시 return;
            return;

        _targets.Remove(c.gameObject);  //대상을 리스트에서 제거
    }
}
