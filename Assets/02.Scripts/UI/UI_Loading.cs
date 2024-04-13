using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 로딩창 UI
/// </summary>
public class UI_Loading : MonoBehaviour
{
    private const float MAX_LOADING_TIME = 6f;

    private Text _loadingText;
    private Tween _textTween;
    private Slider _loadingSlider;
    private float _loadingTime = 0f;
    private Coroutine _textCoroutine;

    private void Start() {
        Init();
        _textCoroutine = StartCoroutine(CoText());
        StartCoroutine(SceneLoad(Define.SceneType.Main));
    }

    /// <summary>
    /// 초기화
    /// </summary>
    private void Init() {
        _loadingSlider = GetComponentInChildren<Slider>();
        _loadingSlider.maxValue = MAX_LOADING_TIME;
        _loadingSlider.value = 0;
        _loadingText = GetComponentInChildren<Text>();
        _loadingText.text = string.Empty;
    }

    /// <summary>
    /// 로딩 중 텍스트 효과
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoText() {
        _textTween = _loadingText.DOText(Define.LOADING, 2f);
        yield return _textTween.WaitForCompletion();
        _loadingText.text = string.Empty;
        _textCoroutine = StartCoroutine(CoText());
    }
    
    /// <summary>
    /// 비동기 로딩
    /// </summary>
    private IEnumerator SceneLoad(Define.SceneType type) {
        AsyncOperation async = SceneManager.LoadSceneAsync(type.ToString());
        async.allowSceneActivation = false;
        while(!async.isDone) {
            _loadingTime += Time.deltaTime;
            _loadingSlider.value = _loadingTime;
            if(_loadingTime > MAX_LOADING_TIME) {
                StopCoroutine(_textCoroutine);
                _textTween.Kill(false);
                _loadingText.text = Define.LOADING_COMPLETE;
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
