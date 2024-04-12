using UnityEngine;
using DG.Tweening;

/// <summary>
/// 캐논타워 발사체 관리
/// </summary>
public class CanonProjectileController : ProjectileControllerBase {
    private Vector3 _targetPos;  //공격 타겟의 위치
    private const float PROJECTILE_HEIGHT_POS = 10f;  //발사체가 이동할 고점
    private const float PROJECTILE_DURATION = 0.5f;  //발사체 이동완료까지 걸리는 시간

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
        _targetPos = target.transform.position;  //타겟의 위치 추출

        Vector3 controlPoint = (pos + _targetPos) / 2 + Vector3.up * PROJECTILE_HEIGHT_POS;  //발사체가 상승할 최대위치 계산
        Vector3[] path = new Vector3[] { pos, controlPoint, _targetPos };  //발사체의 생성 위치, 최대 위치, 타겟의 위치를 기반으로 path 계산

        transform.DOPath(path, PROJECTILE_DURATION, PathType.Linear)  //계산된 path대로 발사체 이동
            .SetOptions(false)
            .SetEase(Ease.InOutQuad);
    }

    private void FixedUpdate() {
        if (!GameSystem.Instance.IsPlay())  //게임이 종료되면 return
            return;

        if (transform.position.y <= LIMITPOS_Y ||  //발사체가 제한위치를 벗어나면 제거
            transform.position.y <= _targetPos.y)
            Crash();
    }


}