using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerEX
{
    public BaseScene CurrentScene => GameObject.FindFirstObjectByType(typeof(BaseScene)).GetComponent<BaseScene>();
    private UI_Fade _fade;

    public void Init() {
        _fade = GameObject.Find("UI_Fade").GetComponent<UI_Fade>();
    }
    public void LoadScene(Define.SceneType type)
    {
        var tween = _fade.SetFade(true);
        tween.OnComplete(() => SceneManager.LoadScene(type.ToString()));
    }
}
