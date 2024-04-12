using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadingScene : BaseScene
{
    public override void Clear() {
        
    }

    public override void Init() {
        Managers.Init();
        base.Init();
    }

    protected override void Start() {
        base.Start();
    }
}
