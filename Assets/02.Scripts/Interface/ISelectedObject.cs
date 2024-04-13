using UnityEngine;

/// <summary>
/// 선택 가능한 오브젝트 관리
/// </summary>
public interface ISelectedObject
{
    ISelectedObject OnSelect();  //선택시 호출
    void OnDeSelect();  //선택 해제시 호출

    Transform MyTransform { get; }  //트랜스폼 return

    bool IsValid();  //오브젝트 NULL 체크
}
