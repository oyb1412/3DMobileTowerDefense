using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    private UI_Fade _fade;
    public Define.SceneType SceneType { get; protected set; } = Define.SceneType.None;

    public abstract void Clear();

    public virtual void Init()
    {
        var obj = GameObject.FindFirstObjectByType(typeof(EventSystem));
        if (obj == null)
            Managers.Resources.Instantiate("UI/EventSystem", null).name = "@EventSystem";

        if(_fade == null)
            _fade = GameObject.Find("UI_Fade").GetComponent<UI_Fade>();
    }

    private void Awake() {
        Init();
    }

    protected virtual void Start() {
        _fade.SetFade(false);
    }
}
