using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherProjectileController : ProjectileControllerBase {

    private const float _projectileVelocity = 100f;
    public override void Init(Vector3 pos, Vector3 dir, int damage, GameObject target) {
        base.Init(pos, dir, damage, target);
    }

    private void FixedUpdate() {
        if(_targetEnemy)
            transform.LookAt(_targetEnemy.transform);

        _rigidbody.velocity = transform.forward * _projectileVelocity;

    }
    private void OnTriggerEnter(Collider c) {
        if (!c.CompareTag("Enemy"))
            return;

        Crash(c.gameObject);
    }
}