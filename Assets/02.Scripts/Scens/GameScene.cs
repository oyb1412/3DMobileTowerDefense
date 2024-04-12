using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Data;
using static GameSystem;

public class GameScene : BaseScene
{

    public override void Clear()
    {
    }

    public override void Init() {
        base.Init();
        Managers.Init();

        SceneType = Define.SceneType.InGame;
        Managers.Spawn.Init();
        Managers.Audio.PlayBgm(true, Define.BgmType.Ingame);

        if(Managers.Scene.isContinue) {
            Dictionary<int, ConData> conData = Managers.Data.LoadJson<GameSystemData>(Application.persistentDataPath, "SaveData").ConData;
            Dictionary<int, GameSystem.TowerData> towerData = Managers.Data.LoadJson<GameSystemData>(Application.persistentDataPath, "SaveData").TowerData;

            foreach(var item in conData) {
                ConData loadData = item.Value;
                int level = loadData.Level;
                Vector3 pos = new Vector3(loadData.PosX, -0.5f, loadData.PosZ);

                Define.TowerType type = (Define.TowerType)loadData.Type;
                string path = $"Towers/{type.ToString()}/{type.ToString()}_Lvl{level}Cons";
                BuildingTower go = Managers.Resources.Instantiate(path, null).GetComponent<BuildingTower>();
                go.Init(pos, loadData.KillNumber);
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

                int handle = go.TowerHandle += 1;
                GameSystem.Instance.SetObjectHandle(handle);
            }

            GameSystem.Instance.Continue();
            return;
        }

        GameSystem.Instance.Init();

    }
}
