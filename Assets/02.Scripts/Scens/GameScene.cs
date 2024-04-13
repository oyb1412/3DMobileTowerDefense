using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 씬
/// </summary>
public class GameScene : BaseScene
{
    public override void Clear() { }

    /// <summary>
    /// 게임 씬 초기화
    /// </summary>
    public override void Init() {
        base.Init();
        Managers.Init();

        SceneType = Define.SceneType.InGame;
        Managers.Spawn.Init();
        Managers.Audio.SetBgm(true, Define.BgmType.Ingame);

        if(Managers.Scene.isContinue) {  //이어하기를 했을 경우, 데이터를 불러옴
            Dictionary<int, GameSystem.ConData> conData = Managers.Data.LoadJson<Data.GameSystemData>(Application.persistentDataPath, "SaveData").ConData;
            Dictionary<int, GameSystem.TowerData> towerData = Managers.Data.LoadJson<Data.GameSystemData>(Application.persistentDataPath, "SaveData").TowerData;

            foreach(var item in conData) {
                GameSystem.ConData loadData = item.Value;
                int level = loadData.Level;
                Vector3 pos = new Vector3(loadData.PosX, -0.5f, loadData.PosZ);

                Define.TowerType type = (Define.TowerType)loadData.Type;
                string path = $"Towers/{type.ToString()}/{type.ToString()}_Lvl{level}Cons";
                ConBase go = Managers.Resources.Instantiate(path, null).GetComponent<ConBase>();
                go.Init(pos, loadData.KillNumber);
                GameSystem.Instance.AddConObject(go, type, level, loadData.KillNumber);
            }

            foreach(var item in towerData) {
                GameSystem.TowerData loadData = item.Value;
                int level = loadData.Level;
                int kill = loadData.KillNumber;
                Vector3 pos = new Vector3(loadData.PosX, -0.5f, loadData.PosZ);

                Define.TowerType type = (Define.TowerType)loadData.Type;
                var towerdata = loadData as GameSystem.TowerData;
                string path = $"Towers/{type.ToString()}/{type.ToString()}_Lvl{level}";
                TowerBase go = Managers.Resources.Instantiate(path, null).GetComponent<TowerBase>();
                go.Init();
                go.TowerStatus.Init(kill, level, pos, type);
                GameSystem.Instance.AddTowerObject(go, type, level);
            }
            GameSystem.Instance.Continue();
            return;
        }
        GameSystem.Instance.Init();
    }
}
