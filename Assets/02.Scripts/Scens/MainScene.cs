using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene {
    public override void Clear() {
        
    }

    public override void Init() {
        base.Init();
        Managers.Init();
        SceneType = Define.SceneType.Main;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        Cursor.lockState = CursorLockMode.Confined;
#endif
    }

    protected override void Start() {
        base.Start();
        Managers.Audio.PlayBgm(true, Define.BgmType.Main);
    }
}
