using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{

    public override void Clear()
    {
    }

    public override void Init() {
        base.Init();
        SceneType = Define.SceneType.InGame;
        Managers.Spawn.Init();
        Managers.Audio.PlayBgm(true, Define.BgmType.Ingame);
    }
}
