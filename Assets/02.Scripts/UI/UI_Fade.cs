using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 페이드 UI
/// </summary>
public class UI_Fade : MonoBehaviour
{
    private const float FADE_TIME = 1f;
    private Image _fadeImage;

    private void Awake() {
        _fadeImage = GetComponentInChildren<Image>();
        _fadeImage.color = Color.black;
    }

    /// <summary>
    /// trigger판정에 맞춰 페이드 실행
    /// </summary>
    public Tween SetFade(bool trigger) {
        if(trigger) {
            return _fadeImage.DOFade(1f, FADE_TIME);
        } else
            return _fadeImage.DOFade(0, FADE_TIME);
    }
}
