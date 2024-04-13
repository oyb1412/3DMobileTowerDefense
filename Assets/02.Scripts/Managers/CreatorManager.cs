using UnityEngine;

/// <summary>
/// 타워 생성 관리
/// </summary>
public class CreatorManager {
    private const int CONNAME_LENGTH = 5;  //타워의 타입에서 path를 추출하기 위해 제거할 문자열 길이
    /// <summary>
    /// 타워 첫 생성
    /// </summary>
    /// <param name="name">생성할 타워의 이름</param>
    /// <param name="createPos">생성할 위치</param>
    public void CreateTower(string name, Vector3 createPos) {
        string path = name.Substring(0, name.Length - CONNAME_LENGTH);  //type명에서 뒷 문자열 제거 
        ConBase tower = Managers.Resources.Instantiate($"Towers/{path}/{path}_Lvl1Cons", null).GetComponent<ConBase>();  //타워con 생성
        tower.Init(createPos);  //초기화
        int handle = GameSystem.Instance.AddConObject(tower, tower.Status.TowerType, tower.Status.Level, tower.Status.KillNumber);  //con에 부여할 고유 핸들 생성
        tower.ConHandle = handle;  //con에 고유 핸들 부여
    }
}
