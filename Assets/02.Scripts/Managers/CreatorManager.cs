using UnityEngine;

/// <summary>
/// Ÿ�� ���� ����
/// </summary>
public class CreatorManager {
    private const int CONNAME_LENGTH = 5;  //Ÿ���� Ÿ�Կ��� path�� �����ϱ� ���� ������ ���ڿ� ����
    /// <summary>
    /// Ÿ�� ù ����
    /// </summary>
    /// <param name="name">������ Ÿ���� �̸�</param>
    /// <param name="createPos">������ ��ġ</param>
    public void CreateTower(string name, Vector3 createPos) {
        string path = name.Substring(0, name.Length - CONNAME_LENGTH);  //type���� �� ���ڿ� ���� 
        ConBase tower = Managers.Resources.Instantiate($"Towers/{path}/{path}_Lvl1Cons", null).GetComponent<ConBase>();  //Ÿ��con ����
        tower.Init(createPos);  //�ʱ�ȭ
        int handle = GameSystem.Instance.AddConObject(tower, tower.Status.TowerType, tower.Status.Level, tower.Status.KillNumber);  //con�� �ο��� ���� �ڵ� ����
        tower.ConHandle = handle;  //con�� ���� �ڵ� �ο�
    }
}
