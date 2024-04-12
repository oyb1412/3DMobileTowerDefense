using UnityEngine;

/// <summary>
/// 애너미 선택을 위한 컴포넌트
/// </summary>
public class EnemySelection : MonoBehaviour, ISelectedObject {
    public Transform MyTransform => transform;  //인터페이스 Get시 애너미 위치 호출용

    private EnemyStatus _enemyStatus;  //애너미 능력치
    private GameObject _mark;  //애너미 선택 시 출력할 스프라이트 오브젝트

    public EnemyStatus EnemyStatus => _enemyStatus;  //능력치 프로퍼티

    /// <summary>
    /// 기본 설정 초기화 및 Get,Find
    /// 애너미 Mark스프라이트 비활성화
    /// </summary>
    private void Start() {
        _enemyStatus = GetComponent<EnemyStatus>();  //오브젝트에서 애너미 능력치 Get
        _mark = Util.FindChild(gameObject, "Mark", false);  //오브젝트 산하에서 Mark오브젝트 Find
        _mark.SetActive(false);  //Mark 비활성화
    }

    /// <summary>
    /// 오브젝트가 선택 해제되었을시 호출
    /// </summary>
    public void OnDeSelect() {
        UIEnemyInfo.Instance.SetEnemyInfoUI(false, this);  //애너미UI 비활성화
        _mark.SetActive(false);  //Mark 비활성화
    }

    /// <summary>
    /// 오브젝트가 선택되었을시 호출
    /// </summary>
    /// <returns>this</returns>
    public ISelectedObject OnSelect() {
        UIEnemyInfo.Instance.SetEnemyInfoUI(true, this);  //애너미UI 활성화
        _mark.SetActive(true);  //Mark 활성화
        return this;  //this 리턴
    }

    /// <summary>
    /// 오브젝트의 active현황 체크
    /// </summary>
    /// <returns>this</returns>
    public bool IsValid() {
        return this;  //this 리턴
    }
}
