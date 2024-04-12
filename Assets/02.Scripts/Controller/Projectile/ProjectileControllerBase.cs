using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� �߻�ü ����
/// </summary>
public class ProjectileControllerBase : MonoBehaviour {

    protected const float LIMITPOS_Y = -0.4f;  //�߻�ü ���� ��ġ
    private const float DESTORY_TIME = 3f;  //�߻�ü �ڵ� �ı� �ð�
    //TODO
    public GameObject muzzlePrefab;  //�߻� ȿ�� ����Ʈ
    public GameObject hitPrefab;  //�ǰ� ȿ�� ����Ʈ
    private GameObject _shooter;  //�߻��� Ÿ���� ���ӿ�����Ʈ
    protected GameObject _targetEnemy;  //�߻� ����� ���ӿ�����Ʈ
    protected Collider _collider;  //�߻�ü�� �ݶ��̴� 
    protected Rigidbody _rigidbody;  //�߻�ü�� ������ٵ�

    protected int _damage;  //�߻�ü�� ������
    private bool collided;  //�߻�ü�� ���� ������ ���� �Ҹ���

    /// <summary>
    /// ��� �߻�ü�� �⺻ �ʱ�ȭ
    /// </summary>
    /// <param name="pos">�߻�ü ���� ��ġ</param>
    /// <param name="damage">�߻�ü ������</param>
    /// <param name="target">�߻� ���</param>
    /// <param name="shooter">�߻��� Ÿ��</param>
    public virtual void Init(Vector3 pos, int damage, GameObject target, GameObject shooter) {
        _collider = GetComponent<Collider>();  //�ݶ��̴� GET
        _rigidbody = GetComponent<Rigidbody>();  //������ٵ� GET
        transform.position = pos;  //�߻�ü ��ġ ����
        _shooter = shooter;  //�߻��� Ÿ�� ����
        _damage = damage;  //�߻�ü ������ ����
        _targetEnemy = target;  //�߻� ��� ����

        StartCoroutine(Util.CoDestroy(gameObject, DESTORY_TIME));  //�߻�ü �ڵ� ����

        if (muzzlePrefab == null)  //�߻� ȿ���� ������ return
            return;

        var muzzleVFX = Managers.Resources.Instantiate(muzzlePrefab, null);  //�߻� ȿ�� ����
        muzzleVFX.transform.position = transform.position;  //�߻� ȿ�� ��ġ ����
        muzzleVFX.transform.rotation = Quaternion.identity;  //�߻� ȿ�� ȸ�� ����
        muzzleVFX.transform.forward = gameObject.transform.forward;  //�߻� ȿ���� forward�� �߻�ü�� �����ϰ� ����

        var ps = muzzleVFX.GetComponent<ParticleSystem>();  //�߻� ȿ���� ��ƼŬ �ý��� ����

        if(ps == null) {  //�߻� ȿ���� ��ƼŬ �ý����� ���� ��
            var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();  //�ڽĿ��� ����
            StartCoroutine(Util.CoDestroy(muzzleVFX, psChild.main.duration));  //�߻� ȿ�� �ڵ� ����
            return;
        }
        
        StartCoroutine(Util.CoDestroy(muzzleVFX, ps.main.duration));  //�߻� ȿ�� �ڵ� ����
    }

    /// <summary>
    /// Ǯ������ ���Ӱ� active�� �ݶ��̴�,rigid�ʱ�ȭ
    /// </summary>
    private void OnEnable() {
        collided = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    /// <summary>
    /// Ÿ�� ��󿡰� ������
    /// </summary>
    /// <param name="go"></param>
    protected void Damage(GameObject go) {
        EnemyController enemy = go.GetComponentInParent<EnemyController>();
        enemy.TakeDamage(-_damage, _shooter);  //�浹 �ֳʹ̿� �������� ����
    }

    /// <summary>
    /// �߻�ü �⺻ �浹
    /// </summary>
    /// <param name="co"></param>
    protected void Crash(Collision co) {
        if (collided)  //�θ� �̻��� �� ���� �Ұ�
            return;

        collided = true;

        GetComponent<Rigidbody>().isKinematic = true;  //������ٵ� ����

        if (hitPrefab != null) {  //Ÿ�� ����Ʈ�� ������ ��
            var hitVFX = Managers.Resources.Instantiate(hitPrefab, null);  //Ÿ�� ����Ʈ ����
            hitVFX.transform.LookAt(Vector3.up);  //Ÿ�� ����Ʈ ȸ���� ���� ����
            hitVFX.transform.position = co.contacts[0].point;  //Ÿ�� ����Ʈ�� ��ġ�� �浹 ��ġ�� ����
            var ps = hitVFX.GetComponent<ParticleSystem>();  //Ÿ�� ����Ʈ�� ��ƼŬ �ý��� ����
            if (ps == null) {  //��ƼŬ �ý����� ���� ��, �ڽ��� ��ƼŬ �ý��� ȣ��
                var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                StartCoroutine(Util.CoDestroy(hitVFX, psChild.main.duration));  //Ÿ�� ����Ʈ �ڵ�����

            } else
                StartCoroutine(Util.CoDestroy(hitVFX, ps.main.duration));  //Ÿ�� ����Ʈ �ڵ�����
        }
        Damage(co.gameObject);  //�浹 ��󿡰� �������� ����

        Managers.Resources.Destroy(gameObject);  //�߻�ü ����
    }

    /// <summary>
    /// ĳ�� Ÿ���� �浹 �Լ�
    /// </summary>
    protected void Crash() {
        if (collided)  //�θ� �̻��� �� ���� �Ұ�
            return;

        collided = true;

        GetComponent<Rigidbody>().isKinematic = true;  //������ٵ� ����

        if (hitPrefab != null) {
            var hitVFX = Managers.Resources.Instantiate(hitPrefab, null);  //���� ����Ʈ ����
            hitVFX.transform.position = transform.position;  //���� ����Ʈ�� ��ġ�� �浹 ��ġ�� ����
            hitVFX.transform.LookAt(Vector3.up);  //���� ����Ʈ ȸ���� ���� ����
            hitVFX.GetComponent<ExplosionHit>().Init(_damage, _shooter);  //���� ����Ʈ �ʱ�ȭ

            var ps = hitVFX.GetComponent<ParticleSystem>();  //Ÿ�� ����Ʈ�� ��ƼŬ �ý��� ����
            if (ps == null) {  //��ƼŬ �ý����� ���� ��, �ڽ��� ��ƼŬ �ý��� ȣ��
                var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                StartCoroutine(Util.CoDestroy(hitVFX, psChild.main.duration));  //Ÿ�� ����Ʈ �ڵ�����
            } else
                StartCoroutine(Util.CoDestroy(hitVFX, ps.main.duration));  //Ÿ�� ����Ʈ �ڵ�����
        }

        Managers.Resources.Destroy(gameObject);  //�߻�ü ����
    }
}
