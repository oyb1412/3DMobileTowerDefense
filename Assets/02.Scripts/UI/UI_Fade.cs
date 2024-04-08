using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UI_Fade : MonoBehaviour
{
    private Image _fadeImage;
    private const float _fadeTime = 1f;

    private void Awake() {
        _fadeImage = GetComponentInChildren<Image>();
        _fadeImage.color = Color.black;
    }

    
    public Tween SetFade(bool trigger) {
        if(trigger) {
            return _fadeImage.DOFade(1f, _fadeTime);
        } else
            return _fadeImage.DOFade(0, _fadeTime);
    }
}
