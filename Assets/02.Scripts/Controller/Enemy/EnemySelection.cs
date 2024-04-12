using UnityEngine;

/// <summary>
/// �ֳʹ� ������ ���� ������Ʈ
/// </summary>
public class EnemySelection : MonoBehaviour, ISelectedObject {
    public Transform MyTransform => transform;  //�������̽� Get�� �ֳʹ� ��ġ ȣ���

    private EnemyStatus _enemyStatus;  //�ֳʹ� �ɷ�ġ
    private GameObject _mark;  //�ֳʹ� ���� �� ����� ��������Ʈ ������Ʈ

    public EnemyStatus EnemyStatus => _enemyStatus;  //�ɷ�ġ ������Ƽ

    /// <summary>
    /// �⺻ ���� �ʱ�ȭ �� Get,Find
    /// �ֳʹ� Mark��������Ʈ ��Ȱ��ȭ
    /// </summary>
    private void Start() {
        _enemyStatus = GetComponent<EnemyStatus>();  //������Ʈ���� �ֳʹ� �ɷ�ġ Get
        _mark = Util.FindChild(gameObject, "Mark", false);  //������Ʈ ���Ͽ��� Mark������Ʈ Find
        _mark.SetActive(false);  //Mark ��Ȱ��ȭ
    }

    /// <summary>
    /// ������Ʈ�� ���� �����Ǿ����� ȣ��
    /// </summary>
    public void OnDeSelect() {
        UIEnemyInfo.Instance.SetEnemyInfoUI(false, this);  //�ֳʹ�UI ��Ȱ��ȭ
        _mark.SetActive(false);  //Mark ��Ȱ��ȭ
    }

    /// <summary>
    /// ������Ʈ�� ���õǾ����� ȣ��
    /// </summary>
    /// <returns>this</returns>
    public ISelectedObject OnSelect() {
        UIEnemyInfo.Instance.SetEnemyInfoUI(true, this);  //�ֳʹ�UI Ȱ��ȭ
        _mark.SetActive(true);  //Mark Ȱ��ȭ
        return this;  //this ����
    }

    /// <summary>
    /// ������Ʈ�� active��Ȳ üũ
    /// </summary>
    /// <returns>this</returns>
    public bool IsValid() {
        return this;  //this ����
    }
}
