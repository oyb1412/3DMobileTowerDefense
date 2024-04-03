using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathProjectileController : ProjectileControllerBase {

    private const float _projectileVelocity = 50f;
    public override void Init(Vector3 pos, Vector3 dir, int damage, GameObject target) {
        base.Init(pos, dir, damage, target);
    }

    private void FixedUpdate() {
        if (_targetEnemy) 
            transform.LookAt(_targetEnemy.transform);

        _rigidbody.velocity = transform.forward * _projectileVelocity;

    }

    private void OnCollisionEnter(Collision collision) {
        if (!collision.collider.CompareTag("Enemy"))
            return;

            Crash(collision);
    }
}