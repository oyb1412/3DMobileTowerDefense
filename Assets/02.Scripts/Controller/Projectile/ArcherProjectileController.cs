using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherProjectileController : ProjectileControllerBase {
    private const float _projectileVelocity = 80f;
    [SerializeField]private EnemyController _target;

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

        //if (collision.transform.parent.gameObject != _target.gameObject)
        //    return;

        Crash(collision);
    }
}