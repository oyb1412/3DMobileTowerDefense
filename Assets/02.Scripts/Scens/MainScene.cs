using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene {
    public override void Clear() {
        
    }

    public override void Init() {
        base.Init();
        SceneType = Define.SceneType.Main;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        Cursor.lockState = CursorLockMode.Confined;
#endif
    }
}
