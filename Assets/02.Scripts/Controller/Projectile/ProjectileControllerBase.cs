using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControllerBase : MonoBehaviour {

    protected Rigidbody _rigidbody;
    protected const float _limitYPos = -0.4f;
    private const float _destroyTime = 3f;
    private GameObject _shooter;
    protected int _damage;
    protected GameObject _targetEnemy;
    protected Collider _collider;
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;
    public List<GameObject> trails;

    private bool collided = false;
    public virtual void Init(Vector3 pos, Vector3 dir, int damage, GameObject target, GameObject shooter) {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        transform.position = pos;
        _shooter = shooter;
        _damage = damage;
        _targetEnemy = target;
        StartCoroutine(Util.CoDestroy(gameObject, _destroyTime));

        if (muzzlePrefab != null) {
            var muzzleVFX = Managers.Resources.Instantiate(muzzlePrefab, null);
            muzzleVFX.transform.position = transform.position;
            muzzleVFX.transform.rotation = Quaternion.identity;
            muzzleVFX.transform.forward = gameObject.transform.forward;
            var ps = muzzleVFX.GetComponent<ParticleSystem>();
            if (ps != null)
                StartCoroutine(Util.CoDestroy(muzzleVFX, ps.main.duration));
            else {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                StartCoroutine(Util.CoDestroy(muzzleVFX, psChild.main.duration));

            }
        }
    }

    private void OnEnable() {
        collided = false;
        GetComponent<Rigidbody>().isKinematic = false;
        //foreach(var item in trails)
        //    item.SetActive(true);
    }



    protected void Crash(GameObject go) {
        EnemyController enemy = go.GetComponentInParent<EnemyController>();
        enemy.TakeDamage(-_damage, _shooter);
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
        Managers.Resources.Destroy(gameObject);
    }

    protected void Crash(Collision co) {
        if (collided)
            return;

        collided = true;

        //if (trails.Count > 0) {
        //    for (int i = 0; i < trails.Count; i++) {
        //        var ps = trails[i].GetComponent<ParticleSystem>();
        //        if (ps != null) {
        //            ps.Stop();
        //            Util.CoActive(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        //        }
        //    }
        //}

        GetComponent<Rigidbody>().isKinematic = true;


        if (hitPrefab != null) {
            var hitVFX = Managers.Resources.Instantiate(hitPrefab, null);
            hitVFX.transform.LookAt(Vector3.up);
            hitVFX.transform.position = co.contacts[0].point;
            var ps = hitVFX.GetComponent<ParticleSystem>();
            if (ps == null) {
                var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                StartCoroutine(Util.CoDestroy(hitVFX, psChild.main.duration));

            } else
                StartCoroutine(Util.CoDestroy(hitVFX, ps.main.duration));

        }
        Crash(co.gameObject);

        StartCoroutine(DestroyParticle(0f));
    }

    protected void Crash() {
        if (collided)
            return;

        collided = true;

        //if (trails.Count > 0) {
        //    for (int i = 0; i < trails.Count; i++) {
        //        var ps = trails[i].GetComponent<ParticleSystem>();
        //        if (ps != null) {
        //            ps.Stop();
        //            Util.CoActive(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        //        }
        //    }
        //}

        GetComponent<Rigidbody>().isKinematic = true;

        Vector3 pos = transform.position;

        if (hitPrefab != null) {
            var hitVFX = Managers.Resources.Instantiate(hitPrefab, null) as GameObject;
            hitVFX.transform.position = pos;
            hitVFX.transform.LookAt(Vector3.up);
            hitVFX.GetComponent<ExplosionHit>().Init(_damage, _shooter);

            var ps = hitVFX.GetComponent<ParticleSystem>();
            if (ps == null) {
                var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                StartCoroutine(Util.CoDestroy(hitVFX, psChild.main.duration));
            } else
                StartCoroutine(Util.CoDestroy(hitVFX, ps.main.duration));

        }

        StartCoroutine(DestroyParticle(0f));
    }
}
