using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherProjectileController : ProjectileControllerBase {
    private const float _projectileVelocity = 50f;

    public override void Init(Vector3 pos, Vector3 dir, int damage, GameObject target, GameObject shooter) {
        base.Init(pos, dir, damage, target, shooter);
    }

    private void FixedUpdate() {
        if (!GameSystem.Instance.IsPlay())
            return;

        if (_targetEnemy) {
            transform.LookAt(_targetEnemy.transform);
            _rigidbody.velocity = transform.forward * _projectileVelocity;

        } else {
            _collider.enabled = false;
            _rigidbody.velocity = Vector3.zero;
            StartCoroutine(DestroyParticle(0f));
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (!collision.collider.CompareTag("Enemy"))
            return;

        Crash(collision);
    }
}