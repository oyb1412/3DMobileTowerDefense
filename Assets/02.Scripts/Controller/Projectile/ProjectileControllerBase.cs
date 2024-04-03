using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ProjectileControllerBase : MonoBehaviour {

    protected Rigidbody _rigidbody;
    private const float _destroyTime = 5f;
    protected int _damage;
    protected GameObject _targetEnemy;

    public GameObject muzzlePrefab;
    public GameObject hitPrefab;
    public List<GameObject> trails;

    private bool collided;
    public virtual void Init(Vector3 pos, Vector3 dir, int damage, GameObject target) {
        _rigidbody = GetComponent<Rigidbody>();
        transform.position = pos;
        _damage = damage;
        _targetEnemy = target;
        StartCoroutine(CoDestroy());

        if (muzzlePrefab != null) {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;
            var ps = muzzleVFX.GetComponent<ParticleSystem>();
            if (ps != null)
                Destroy(muzzleVFX, ps.main.duration);
            else {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }
    }

    IEnumerator CoDestroy() {
        yield return new WaitForSeconds(_destroyTime);
        Managers.Resources.Destroy(gameObject);
    }

 

    protected void Crash(GameObject go) {
        EnemyController enemy = go.GetComponentInParent<EnemyController>();
        enemy.TakeDamage(-_damage);
    }

    public IEnumerator DestroyParticle(float waitTime) {

        if (transform.childCount > 0 && waitTime != 0) {
            List<Transform> tList = new List<Transform>();

            foreach (Transform t in transform.GetChild(0).transform) {
                tList.Add(t);
            }

            while (transform.GetChild(0).localScale.x > 0) {
                yield return new WaitForSeconds(0.01f);
                transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                for (int i = 0; i < tList.Count; i++) {
                    tList[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
        }

        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    protected void Crash(Collision co) {
            if (co.gameObject.tag != "Projectile" && !collided) {
                collided = true;

                if (trails.Count > 0) {
                    for (int i = 0; i < trails.Count; i++) {
                        trails[i].transform.parent = null;
                        var ps = trails[i].GetComponent<ParticleSystem>();
                        if (ps != null) {
                            ps.Stop();
                            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                        }
                    }
                }

                GetComponent<Rigidbody>().isKinematic = true;

                ContactPoint contact = co.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;

                if (hitPrefab != null) {
                    var hitVFX = Instantiate(hitPrefab, pos, rot) as GameObject;
                    hitVFX.transform.LookAt(Vector3.up);
                    var ps = hitVFX.GetComponent<ParticleSystem>();
                    if (ps == null) {
                        var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                        Destroy(hitVFX, psChild.main.duration);
                    } else
                        Destroy(hitVFX, ps.main.duration);
                }

                Crash(co.gameObject);
                StartCoroutine(DestroyParticle(0f));
            }
    }
}
