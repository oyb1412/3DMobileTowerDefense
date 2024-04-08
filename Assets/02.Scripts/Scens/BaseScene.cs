using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.SceneType SceneType { get; protected set; } = Define.SceneType.None;
    private UI_Fade _fade;

    public abstract void Clear();

    public virtual void Init()
    {
        var q = Managers.Instance;
        var obj = GameObject.FindFirstObjectByType(typeof(EventSystem));
        if (obj == null)
            Managers.Resources.Instantiate("UI/EventSystem", null).name = "@EventSystem";

        if(_fade == null)
            _fade = GameObject.Find("UI_Fade").GetComponent<UI_Fade>();

    }

    private void Awake() {
        Init();
    }

    private void Start() {
        _fade.SetFade(false);


    }
}
