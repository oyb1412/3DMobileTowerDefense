using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Loading : MonoBehaviour
{
    private const float MaxLoadingTime = 6f;
    private const string LoadingText = "Loading...";
    private const string CompleteText = "Completed!!";
    private Text _loadingText;
    private Tween _textTween;
    private Slider _loadingSlider;
    private float _loadingTime = 0f;
    private bool _loadingComplete;
    private Coroutine _textCoroutine;

    private void Start() {
        Init();
        _textCoroutine = StartCoroutine(CoText());
        StartCoroutine(SceneLoad(Define.SceneType.Main));
    }

    private void Init() {
        _loadingSlider = GetComponentInChildren<Slider>();
        _loadingSlider.maxValue = MaxLoadingTime;
        _loadingSlider.value = 0;
        _loadingText = GetComponentInChildren<Text>();
        _loadingText.text = string.Empty;
    }

    private IEnumerator CoText() {
        _textTween = _loadingText.DOText(LoadingText, 2f);
        yield return _textTween.WaitForCompletion();
        _loadingText.text = string.Empty;
        _textCoroutine = StartCoroutine(CoText());
    }

    

    private IEnumerator SceneLoad(Define.SceneType type) {
        AsyncOperation async = SceneManager.LoadSceneAsync(type.ToString());
        async.allowSceneActivation = false;
        while(!async.isDone) {
            _loadingTime += Time.deltaTime;
            _loadingSlider.value = _loadingTime;
            if(_loadingTime > MaxLoadingTime) {
                StopCoroutine(_textCoroutine);
                _textTween.Kill(false);
                _loadingText.text = CompleteText;
                async.allowSceneActivation = true;
            }
            yield return null;
        }

    }
}
