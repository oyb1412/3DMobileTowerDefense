using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 애너미 유닛 컨트롤러
/// </summary>
public class EnemyController : MonoBehaviour
{
    private const int ENEMY_DAMAGE = 10;  //애너미가 도착지점에 도착 시 플레이어가 받을 데미지
    private const float ENEMY_ANIMATION_FADETIME = .3f;  //애니메이션 전환 시간
    private const float ENEMY_ROTATETIME = .3f;  //애너미 회전 시간

    private Animator _animator;  //모델 애니메이터
    private EnemyStatus _status;  //능력치 
    private Define.EnemyState _state;  //상태
    private CapsuleCollider _collider;  //모델 콜라이더
    private Transform[] _movePoint;  //유닛이 이동할 각 포인트
    private Transform _movePoints;  //이동할 포인트들의 부모
    public Action OnRewardEvent;  //사망시 호출되는 보상을 위한 이벤트
    public Action<int, int> OnHpEvent;  //사망시 호출되는 UI변경을 위한 이벤트
    private Tween _moveTween;  //현재 이동중인 트윈
    private Tween _rotateTween;  //현재 회전중인 트윈

    private int _lastMoveIndex;  //도착 지점 인덱스 번호

    public int MoveIndex { get; set; }  //유닛이 현재 이동중인 포인트 인덱스
    public EnemyStatus Status => _status;  //능력치 프로퍼티
    public int CurrentHp { get { return _status.CurrentHp; } }  //현재 체력 프로퍼티

    /// <summary>
    /// 상태 전환 프로퍼티
    /// 사망시 콜라이더 해제 및 모든 행동 종료
    /// 상태 전환 시 애니메이터 전환
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
    /// 기본 초기화
    /// 매개변수가 필요 없는 고정 초기화 값들을 초기화
    /// </summary>
    private void Awake() {
        _animator = GetComponentInChildren<Animator>();  //모델 애니메이터 Get
        _collider = GetComponentInChildren<CapsuleCollider>();  //모델 콜라이더 Get
        _status = GetComponentInChildren<EnemyStatus>();  //모델 능력치 Get
        _movePoints = GameObject.Find(Define.MOVE_POINT).transform;  //하이어라키 창에서 이동 포인트 부모를 Find
        _movePoint = new Transform[_movePoints.childCount];  //하이어라키 창의 이동 포인트 갯수만큼 이동 포인트 변수 크기를 초기화
        for (int i = 0; i < _movePoints.childCount; i++) {
            _movePoint[i] = _movePoints.GetChild(i).transform;  //각 이동 포인트의 위치를 이동 포인트 변수에 부여
        }
        _lastMoveIndex = _movePoint.Length;  //마지막 이동 포인트를 부여
    }

    /// <summary>
    /// 애너미 초기화 함수
    /// 애너미 스폰시 호출
    /// 위치, 콜라이더 설정
    /// </summary>
    /// <param name="pos">스폰시킬 애너미 위치</param>
    /// <param name="number">애너미 핸들 인덱스</param>
    public void Init(Vector3 pos, int number) {
        transform.position = pos;  //위치 초기화
        _status.Init(number);  //능력치 초기화 및 고유 핸들 부여
        State = Define.EnemyState.Move;  //기본 상태 Move로 설정
        MoveIndex = 0;  //첫 이동 포인트를 0번 포인트로 설정
        transform.LookAt(_movePoint[MoveIndex].position);  //이동 포인트로 회전
        StartCoroutine(CoMove());  //이동 코루틴 실행
        GetComponentInChildren<UIEnemy>().Init();  //각 유닛에 부여된 UI 초기화
    }

    /// <summary>
    /// 업데이트 함수
    /// 애너미의 상태와 게임 상태만 체크
    /// </summary>
    private void Update() {
        if (State != Define.EnemyState.Move ||
            !GameSystem.Instance.IsPlay())  //애너미가 이동 상태가 아니거나, 게임이 종료되면 애너미의 모든 행동 종료
            StopAllBehaivoir();
    }

    /// <summary>
    /// 모든 행동 종료
    /// </summary>
    private void StopAllBehaivoir() {
        StopAllCoroutines();  //모든 코루틴 종료
        _moveTween?.Kill(false);  //이동 트윈 종료
        _rotateTween?.Kill(false);  //회전 트윈 종료
        DOTween.Kill(gameObject, false);  //남아있을 수 있는 모든 트윈 강제 종료
    }

    /// <summary>
    /// 피해 받았을 시 호출
    /// </summary>
    /// <param name="damage">받은 데미지</param>
    /// <param name="attacker">공격한 대상의 gameobject</param>
    public void TakeDamage(int damage, GameObject attacker) {
        TowerBase tower = attacker.GetComponent<TowerBase>();  //공격한 대상의 타워 정보를 추출
        if (tower == null)
            return;

        int physicsDefense = _status.PhysicsDefense;  //물리 방어력 
        int magicDefense = _status.MagicDefense;  //마법 방어력
        var type = tower.TowerStatus.TowerType;  //공격한 타워의 타입

        if (type == Define.TowerType.ArcherTower || type == Define.TowerType.CanonTower)  //공격한 타워의 타입에 따라 데미지 계산
            damage += physicsDefense;
        else if (type == Define.TowerType.MagicTower)
            damage += magicDefense;
        else {
            damage += (physicsDefense / 5);
            damage += (magicDefense / 5);
        }

        damage = Mathf.Min(damage, -1);  //데미지는 항상 1이상

        _status.CurrentHp += damage;  //체력 조절

        OnHpEvent?.Invoke(_status.CurrentHp , _status.MaxHp);  //현재체력, 최대체력을 기반으로 UI이벤트에 연결된 이벤트 호출

        if (_status.CurrentHp <= 0) {  //애너미 사망 시
            OnRewardEvent?.Invoke();  //화면에 골드 보상 UI 생성
            tower.SetKill();  //처치한 타워의 이벤트 호출 
            GameSystem.Instance.SetGold(_status.RewardGold);  //골드 보상
            GameSystem.Instance.SetScore(_status.RewardScore);  //점수 보상
            State = Define.EnemyState.Die;  //상태 변환
        }
    }

    /// <summary>
    /// 이동 코루틴
    /// 마지막 위치에 도착할 때 까지 재귀호출
    /// </summary>
    IEnumerator CoMove() {

        Vector3 dir = _movePoint[MoveIndex].position - transform.position;  //다음 이동 포인트까지의 벡터

        float moveTime = Mathf.Max(dir.magnitude / _status.MoveSpeed, 1f);  //다음 이동 포인트까지의 이동 시간을 계산

        _moveTween = transform.DOMove(_movePoint[MoveIndex].position, moveTime).SetEase(Ease.Linear);  //다음 이동 포인트까지 균일한 속도로 이동

        yield return _moveTween.WaitForCompletion();  //다음 이동 포인트까지 이동할 때 까지 대기

        MoveIndex++;  //이동 완료 시 이동 포인트 변경을 위해 Index를 1 추가

        if (gameObject.activeInHierarchy) {  //애너미 오브젝트 존재 체크
            if (MoveIndex >= _lastMoveIndex) {  //도착 지점에 도착
                Managers.MainCamera.CameraShake();  //카메라 셰이크 효과
                Managers.Audio.PlaySfx(Define.SfxType.EnemyArrive);  //애너미 도착시 경고 효과음 출력
                GameSystem.Instance.SetGameHp(ENEMY_DAMAGE);  //애너미 도착 시 플레이어 Hp 조절
                State = Define.EnemyState.Die;  //상태 변경
            }
            else {  //아직 도착하지 않았다면
                Rotate(_movePoint[MoveIndex - 1].position, _movePoint[MoveIndex].position);  //다음 위치로 회전
                StartCoroutine(CoMove());  //다음 위치로 이동
            }
        }
    }

    /// <summary>
    /// 애너미의 위치와 다음 이동 예정 위치를 기반으로 회전값 계산
    /// </summary>
    /// <param name="currentPos">현재 위치</param>
    /// <param name="targetPos">다음 이동 예정 위치</param>
    private void Rotate(Vector3 currentPos, Vector3 targetPos) {
        float x = targetPos.x - currentPos.x;
        float z = targetPos.z - currentPos.z;

        float angle = 0f;

        if (x > 0 && z == 0) angle = 90f;  // 동쪽으로 회전
        else if (x < 0 && z == 0) angle = -90f;  // 서쪽으로 회전
        else if (z > 0 && x == 0) angle = 0f;  // 북쪽으로 회전
        else if (z < 0 && x == 0) angle = 180f;  // 남쪽으로 회전

        _rotateTween = transform.DORotateQuaternion(Quaternion.Euler(0f, angle, 0f), ENEMY_ROTATETIME).SetEase(Ease.Linear);
    }
}
