using UnityEngine;

/// <summary>
/// 선택 가능한 노드 관리
/// </summary>
public class Node : MonoBehaviour, ISelectedObject {
    private MeshRenderer _mesh;  //노드의 mesh 정보

    public Transform MyTransform { get {return transform; }  }

    private void Start() =>  _mesh = GetComponent<MeshRenderer>();

    /// <summary>
    /// 노드 선택 시
    /// </summary>
    /// <returns>this</returns>
    public ISelectedObject OnSelect() {
        Managers.Instance.creator.SelectNode(true, transform.position, this);  //타워 생성 UI 활성화
        _mesh.material = Managers.Data.RedMaterial;  //마테리얼의 색 변경
        return this;
    }

    /// <summary>
    /// 노드 선택 해제 시
    /// </summary>
    public void OnDeSelect() {
        Managers.Instance.creator.SelectNode(false, transform.position);  //타워 생성 UI 비활성화
        _mesh.material = Managers.Data.DefaultMaterial;  //마테리얼의 색 변경
    }

    public bool IsValid() => this;
}
