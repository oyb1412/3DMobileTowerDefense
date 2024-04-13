using System.Collections;
using UnityEngine;

/// <summary>
/// 캐논타워 폭발 공격 
/// </summary>
public class ExplosionHit : MonoBehaviour
{
    private const float DISABLE_TIME = .1f;  //공격 판정 비활성화 시간
    private const float DESTROY_TIME = 1f;  //폭발체 제거 시간
    private int _damage;  //데미지
    private GameObject _attacker;  //발사한 주체
    private SphereCollider _collider;

    /// <summary>
    /// 초기화
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
    /// 활성화 시 콜라이더 설정 초기화
    /// </summary>
    private void OnEnable() {
        if(_collider == null)
            _collider = GetComponent<SphereCollider>();

        _collider.enabled = true;
    }

    /// <summary>
    /// 오브젝트 판정 비활성화 및 제거
    /// </summary>
    /// <returns></returns>
    IEnumerator CoDestroy() {
        yield return new WaitForSeconds(DISABLE_TIME);
        _collider.enabled = false;
        yield return new WaitForSeconds(DESTROY_TIME);
        Managers.Resources.Destroy(gameObject);
    }

    /// <summary>
    /// 애너미와 충돌 시 데미지
    /// </summary>
    private void OnTriggerEnter(Collider c) {
        if (!c.CompareTag("Enemy"))
            return;

        c.GetComponentInParent<EnemyController>().TakeDamage(-_damage, _attacker);
    }
}
