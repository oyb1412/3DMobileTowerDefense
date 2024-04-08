using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DeathProjectileController : ProjectileControllerBase {
    [SerializeField] private EnemyController _target;
    private const float _projectileVelocity = 100f;
    public override void Init(Vector3 pos, Vector3 dir, int damage, GameObject target, GameObject shooter) {
        base.Init(pos, dir, damage, target, shooter);
        _target = target.GetComponentInParent<EnemyController>();

    }

    private void FixedUpdate() {
        if (!GameSystem.Instance.IsPlay())
            return;

        if (!Util.NullCheck(_target.gameObject) && _target.Status.CurrentHp > 0)
            transform.LookAt(_target.transform);

        _rigidbody.velocity = transform.forward * _projectileVelocity;
    }

    private void OnCollisionEnter(Collision collision) {
        if (!collision.collider.CompareTag("Enemy"))
            return;

            Crash(collision);
    }
}