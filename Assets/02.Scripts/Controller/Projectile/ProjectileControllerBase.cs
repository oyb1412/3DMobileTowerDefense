using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 발사체 관리
/// </summary>
public class ProjectileControllerBase : MonoBehaviour {

    protected const float LIMITPOS_Y = -0.4f;  //발사체 제한 위치
    private const float DESTORY_TIME = 3f;  //발사체 자동 파괴 시간
    //TODO
    public GameObject muzzlePrefab;  //발사 효과 이펙트
    public GameObject hitPrefab;  //피격 효과 이펙트
    private GameObject _shooter;  //발사한 타워의 게임오브젝트
    protected GameObject _targetEnemy;  //발사 대상의 게임오브젝트
    protected Collider _collider;  //발사체의 콜라이더 
    protected Rigidbody _rigidbody;  //발사체의 리지드바디

    protected int _damage;  //발사체의 데미지
    private bool collided;  //발사체의 단일 공격을 위한 불리언

    /// <summary>
    /// 모든 발사체의 기본 초기화
    /// </summary>
    /// <param name="pos">발사체 생성 위치</param>
    /// <param name="damage">발사체 데미지</param>
    /// <param name="target">발사 대상</param>
    /// <param name="shooter">발사한 타워</param>
    public virtual void Init(Vector3 pos, int damage, GameObject target, GameObject shooter) {
        _collider = GetComponent<Collider>();  //콜라이더 GET
        _rigidbody = GetComponent<Rigidbody>();  //리지드바디 GET
        transform.position = pos;  //발사체 위치 지정
        _shooter = shooter;  //발사한 타워 지정
        _damage = damage;  //발사체 데미지 지정
        _targetEnemy = target;  //발사 대상 지정

        StartCoroutine(Util.CoDestroy(gameObject, DESTORY_TIME));  //발사체 자동 삭제

        if (muzzlePrefab == null)  //발사 효과가 없을시 return
            return;

        var muzzleVFX = Managers.Resources.Instantiate(muzzlePrefab, null);  //발사 효과 생성
        muzzleVFX.transform.position = transform.position;  //발사 효과 위치 지정
        muzzleVFX.transform.rotation = Quaternion.identity;  //발사 효과 회전 지정
        muzzleVFX.transform.forward = gameObject.transform.forward;  //발사 효과의 forward를 발사체와 동일하게 지정

        var ps = muzzleVFX.GetComponent<ParticleSystem>();  //발사 효과의 파티클 시스템 추출

        if(ps == null) {  //발사 효과에 파티클 시스템이 없을 시
            var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();  //자식에서 추출
            StartCoroutine(Util.CoDestroy(muzzleVFX, psChild.main.duration));  //발사 효과 자동 삭제
            return;
        }
        
        StartCoroutine(Util.CoDestroy(muzzleVFX, ps.main.duration));  //발사 효과 자동 삭제
    }

    /// <summary>
    /// 풀링으로 새롭게 active시 콜라이더,rigid초기화
    /// </summary>
    private void OnEnable() {
        collided = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    /// <summary>
    /// 타겟 대상에게 데미지
    /// </summary>
    /// <param name="go"></param>
    protected void Damage(GameObject go) {
        EnemyController enemy = go.GetComponentInParent<EnemyController>();
        enemy.TakeDamage(-_damage, _shooter);  //충돌 애너미에 데미지를 가함
    }

    /// <summary>
    /// 발사체 기본 충돌
    /// </summary>
    /// <param name="co"></param>
    protected void Crash(Collision co) {
        if (collided)  //두명 이상의 적 공격 불가
            return;

        collided = true;

        GetComponent<Rigidbody>().isKinematic = true;  //리지드바디 중지

        if (hitPrefab != null) {  //타격 이펙트가 존재할 시
            var hitVFX = Managers.Resources.Instantiate(hitPrefab, null);  //타격 이펙트 생성
            hitVFX.transform.LookAt(Vector3.up);  //타격 이펙트 회전값 강제 조정
            hitVFX.transform.position = co.contacts[0].point;  //타격 이펙트의 위치를 충돌 위치로 지정
            var ps = hitVFX.GetComponent<ParticleSystem>();  //타격 이펙트의 파티클 시스템 추출
            if (ps == null) {  //파티클 시스템이 없을 시, 자식의 파티클 시스템 호출
                var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                StartCoroutine(Util.CoDestroy(hitVFX, psChild.main.duration));  //타격 이펙트 자동삭제

            } else
                StartCoroutine(Util.CoDestroy(hitVFX, ps.main.duration));  //타격 이펙트 자동삭제
        }
        Damage(co.gameObject);  //충돌 대상에게 데미지를 가함

        Managers.Resources.Destroy(gameObject);  //발사체 삭제
    }

    /// <summary>
    /// 캐논 타워용 충돌 함수
    /// </summary>
    protected void Crash() {
        if (collided)  //두명 이상의 적 공격 불가
            return;

        collided = true;

        GetComponent<Rigidbody>().isKinematic = true;  //리지드바디 중지

        if (hitPrefab != null) {
            var hitVFX = Managers.Resources.Instantiate(hitPrefab, null);  //폭발 이펙트 생성
            hitVFX.transform.position = transform.position;  //폭발 이펙트의 위치를 충돌 위치로 지정
            hitVFX.transform.LookAt(Vector3.up);  //폭발 이펙트 회전값 강제 조정
            hitVFX.GetComponent<ExplosionHit>().Init(_damage, _shooter);  //폭발 이펙트 초기화

            var ps = hitVFX.GetComponent<ParticleSystem>();  //타격 이펙트의 파티클 시스템 추출
            if (ps == null) {  //파티클 시스템이 없을 시, 자식의 파티클 시스템 호출
                var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                StartCoroutine(Util.CoDestroy(hitVFX, psChild.main.duration));  //타격 이펙트 자동삭제
            } else
                StartCoroutine(Util.CoDestroy(hitVFX, ps.main.duration));  //타격 이펙트 자동삭제
        }

        Managers.Resources.Destroy(gameObject);  //발사체 삭제
    }
}
