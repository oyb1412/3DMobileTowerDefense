using UnityEngine;

/// <summary>
/// ���� ������ ��� ����
/// </summary>
public class Node : MonoBehaviour, ISelectedObject {
    private MeshRenderer _mesh;  //����� mesh ����

    public Transform MyTransform { get {return transform; }  }

    private void Start() =>  _mesh = GetComponent<MeshRenderer>();

    /// <summary>
    /// ��� ���� ��
    /// </summary>
    /// <returns>this</returns>
    public ISelectedObject OnSelect() {
        Managers.Instance.creator.SelectNode(true, transform.position, this);  //Ÿ�� ���� UI Ȱ��ȭ
        _mesh.material = Managers.Data.RedMaterial;  //���׸����� �� ����
        return this;
    }

    /// <summary>
    /// ��� ���� ���� ��
    /// </summary>
    public void OnDeSelect() {
        Managers.Instance.creator.SelectNode(false, transform.position);  //Ÿ�� ���� UI ��Ȱ��ȭ
        _mesh.material = Managers.Data.DefaultMaterial;  //���׸����� �� ����
    }

    public bool IsValid() => this;
}
