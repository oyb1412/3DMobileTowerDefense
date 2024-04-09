using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    private GameObject _panel;
    private Text _roundText;
    private Text _logoText;
    private Text _scoreText;
    private Button _restartBtn;
    private Button _exitBtn;

    void Start()
    {
        _panel = Util.FindChild(gameObject, "Panel", false);
        _roundText = Util.FindChild(_panel, "RoundText", true).GetComponent<Text>();
        _logoText = Util.FindChild(_panel, "Logo", true).GetComponent<Text>();
        _scoreText = Util.FindChild(_panel, "ScoreText", true).GetComponent<Text>();
        _restartBtn = Util.FindChild(_panel, "RestartBtn", false).GetComponent<Button>();
        _exitBtn = Util.FindChild(_panel, "ExitBtn", false).GetComponent<Button>();

        _restartBtn.onClick.AddListener(() => Managers.Scene.LoadScene(Define.SceneType.InGame));
        _restartBtn.onClick.AddListener(() => _restartBtn.interactable = false);
        _restartBtn.onClick.AddListener(() => _exitBtn.interactable = false);
        _exitBtn.onClick.AddListener(() => Managers.Scene.LoadScene(Define.SceneType.Main));
        _exitBtn.onClick.AddListener(() => _restartBtn.interactable = false);
        _exitBtn.onClick.AddListener(() => _exitBtn.interactable = false);
        gameObject.SetActive(false);
    }

    public void SetGameOverUI(int round, int score, bool victory) {
        if(victory)
            _logoText.text = "Victory!!!";
        else
            _logoText.text = "Gameover...";

        _roundText.text = $"최고 라운드 : {round}";
        _scoreText.text = $"최고 스코어 : {score}";
    }
}
