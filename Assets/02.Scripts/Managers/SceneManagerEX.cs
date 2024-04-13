using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ¾À ÀüÈ¯ °ü¸®
/// </summary>
public class SceneManagerEX
{
    public BaseScene CurrentScene => GameObject.FindFirstObjectByType(typeof(BaseScene)).GetComponent<BaseScene>();
    public bool isContinue { get; set; } = false;

    private UI_Fade _fade;

    public void Init() {
        _fade = GameObject.Find("UI_Fade").GetComponent<UI_Fade>();
    }
    public void LoadScene(Define.SceneType type)
    {
        var tween = _fade.SetFade(true);
        if(type == Define.SceneType.Exit) {
            tween.OnComplete(DoNextGame);
        }
        else
            tween.OnComplete(() => DoNextScene(type));
    }

    private void DoNextGame() {
        Managers.Instance.Clear();
        Util.ExitGame();
    }

    private void DoNextScene(Define.SceneType type) {
        Managers.Instance.Clear();
        SceneManager.LoadScene(type.ToString());
    }
}
