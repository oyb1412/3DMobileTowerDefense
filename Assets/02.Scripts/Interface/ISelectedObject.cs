using UnityEngine;

/// <summary>
/// ���� ������ ������Ʈ ����
/// </summary>
public interface ISelectedObject
{
    ISelectedObject OnSelect();  //���ý� ȣ��
    void OnDeSelect();  //���� ������ ȣ��

    Transform MyTransform { get; }  //Ʈ������ return

    bool IsValid();  //������Ʈ NULL üũ
}
