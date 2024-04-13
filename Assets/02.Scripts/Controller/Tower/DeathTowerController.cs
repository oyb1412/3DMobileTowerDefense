using UnityEngine;

/// <summary>
/// Death타워 관리
/// </summary>
public class DeathTowerController : TowerControllerBase {
    /// <summary>
    /// 초기화
    /// </summary>
    protected override void Init() {
        base.Init();
        _projectileObject = (GameObject)Managers.Resources.Load<GameObject>(_projectilePath);
    }

    /// <summary>
    /// 타워 공격 시 호출
    /// 애니메이션 이벤트에서 호출
    /// </summary>
    protected override void OnAttackEvent() {
        base.OnAttackEvent();  //타워 기본 공격 이벤트 호출
        if (!_targetEnemy) {  //타겟이 존재하지 않을 시 상태 변경 및 공격 취소
            ChangeState(Define.TowerState.Idle);  //Idle상태로 변경
            return;
        }

        DeathProjectileController projectile = Managers.Resources.Instantiate(_projectileObject, null).GetComponent<DeathProjectileController>();  //발사체 생성
        projectile.Init(_firePoint.position, _status.AttackDamage, _targetEnemy, _base);  //발사체 초기화
        Managers.Audio.PlaySfx(Define.SfxType.BeamProjectile);  //발사 효과음 출력
        _currentAttackDelay = 0;  //발사 딜레이 초기화
    }
}