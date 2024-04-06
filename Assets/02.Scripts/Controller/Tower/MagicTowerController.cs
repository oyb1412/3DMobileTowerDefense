using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTowerController : TowerControllerBase{
    protected override void Init() {
        base.Init();
    }
    protected override void OnIdleUpdate() {

    }

    protected override void OnAttackEvent() {
        base.OnAttackEvent();
        if (!_targetEnemy) {
            ChangeState(null);
            return;
        }

        MagicProjectileController projectile = Managers.Resources.Instantiate(_projectilePath, null).GetComponent<MagicProjectileController>();
        projectile.Init(_firePoint.position, _targetEnemy.transform.position, _status.AttackDamage, _targetEnemy, _base);
        _currentAttackDelay = 0;
    }
}
