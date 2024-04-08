using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTowerController : TowerControllerBase {

    protected override void Init() {
        base.Init();
    }
   

    protected override void OnAttackEvent() {
        base.OnAttackEvent();
        if (!_targetEnemy) {
            ChangeState(false);
            return;
        }
            
        ArcherProjectileController projectile = Managers.Resources.Instantiate(_projectilePath, null).GetComponent<ArcherProjectileController>();
        projectile.Init(_firePoint.position, _targetEnemy.transform.position, _status.AttackDamage, _targetEnemy, _base);
        _currentAttackDelay = 0;
    }
}
