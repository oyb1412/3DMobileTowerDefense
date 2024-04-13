using System.Collections;
using UnityEngine;

/// <summary>
/// ĳ��Ÿ�� ���� ���� 
/// </summary>
public class ExplosionHit : MonoBehaviour
{
    private const float DISABLE_TIME = .1f;  //���� ���� ��Ȱ��ȭ �ð�
    private const float DESTROY_TIME = 1f;  //����ü ���� �ð�
    private int _damage;  //������
    private GameObject _attacker;  //�߻��� ��ü
    private SphereCollider _collider;

    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="shooter"></param>
    public void Init(int damage, GameObject shooter) {
        if (_collider == null)
            _collider = GetComponent<SphereCollider>();

        _attacker = shooter;
        _damage = damage;
        StartCoroutine(CoDestroy());
    }

    /// <summary>
    /// Ȱ��ȭ �� �ݶ��̴� ���� �ʱ�ȭ
    /// </summary>
    private void OnEnable() {
        if(_collider == null)
            _collider = GetComponent<SphereCollider>();

        _collider.enabled = true;
    }

    /// <summary>
    /// ������Ʈ ���� ��Ȱ��ȭ �� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator CoDestroy() {
        yield return new WaitForSeconds(DISABLE_TIME);
        _collider.enabled = false;
        yield return new WaitForSeconds(DESTROY_TIME);
        Managers.Resources.Destroy(gameObject);
    }

    /// <summary>
    /// �ֳʹ̿� �浹 �� ������
    /// </summary>
    private void OnTriggerEnter(Collider c) {
        if (!c.CompareTag("Enemy"))
            return;

        c.GetComponentInParent<EnemyController>().TakeDamage(-_damage, _attacker);
    }
}
