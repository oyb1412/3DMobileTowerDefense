using UnityEngine;

/// <summary>
/// 아처타워의 발사체 관리
/// </summary>
public class ArcherProjectileController : ProjectileControllerBase {
    private const float PROJECTILE_VELOCITY = 80f;  //발사체 속도
    private EnemyController _target;  //타겟 애너미

    /// <summary>
    /// 발사체 초기화
    /// </summary>
    /// <param name="pos">발사체 초기 위치</param>
    /// <param name="dir">발사체 초기 회전값</param>
    /// <param name="damage">발사체 데미지</param>
    /// <param name="target">타겟 애너미 게임오브젝트</param>
    /// <param name="shooter">발사한 타워 게임오브젝트</param>
    public override void Init(Vector3 pos, int damage, GameObject target, GameObject shooter) {
        base.Init(pos, damage, target, shooter);  //발사체 기본 설정 초기화
        _target = target.GetComponentInParent<EnemyController>();  //타겟 게임오브젝트에서 애너미컨트롤러 추출
    }

    /// <summary>
    /// 발사체 이동
    /// </summary>
    private void FixedUpdate() {
        if (!GameSystem.Instance.IsPlay())  //게임 종료시 발사체 중지
            return;

        if(transform.position.y < LIMITPOS_Y)  //제한 위치를 벗어날 시 발사체 삭제
            Managers.Resources.Destroy(gameObject);

        if (!Util.NullCheck(_target.gameObject) && _target.Status.CurrentHp > 0)  //발사체가 NULL이 아니고, 체력도 충분하다면
            transform.LookAt(_target.transform);  //발사체 방향으로 회전

        _rigidbody.velocity = transform.forward * PROJECTILE_VELOCITY;  //발사체 이동
    }

    private void OnCollisionEnter(Collision collision) {
        if (!collision.collider.CompareTag(Define.TAG_ENEMY))  //애너미가 아닌 대상과 충돌시 무시
            return;

        Crash(collision);  //애너미와 충돌
    }
}