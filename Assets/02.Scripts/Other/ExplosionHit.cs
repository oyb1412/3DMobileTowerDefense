using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ExplosionHit : MonoBehaviour
{
    private int _damage;
    private GameObject _attacker;
    private const float _disalbeTimer = .1f;
    private const float _destroyTimer = 1f;
    [SerializeField]private SphereCollider _collider;

    public void Init(int damage, GameObject shooter) {
        _attacker = shooter;
        _damage = damage;
        StartCoroutine(CoDestroy());
    }

    private void OnEnable() {
        _collider.enabled = true;
    }

    IEnumerator CoDestroy() {
        yield return new WaitForSeconds(_disalbeTimer);
        _collider.enabled = false;
        yield return new WaitForSeconds(_destroyTimer);
        Managers.Resources.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider c) {
        if (!c.CompareTag("Enemy"))
            return;

        c.GetComponentInParent<EnemyController>().TakeDamage(-_damage, _attacker);
    }
}
