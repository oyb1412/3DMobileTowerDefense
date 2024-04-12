using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    private GameObject _panel;
    private Text _roundText;
    private Text _logoText;
    private Text _scoreText;
    private Button _restartBtn;
    private Button _mainBtn;

    private Text _restartText;
    private Text _mainText;


    void Start()
    {
        _panel = Util.FindChild(gameObject, "Panel", false);
        _roundText = Util.FindChild(_panel, "RoundText", true).GetComponent<Text>();
        _logoText = Util.FindChild(_panel, "Logo", true).GetComponent<Text>();
        _scoreText = Util.FindChild(_panel, "ScoreText", true).GetComponent<Text>();
        _restartText = Util.FindChild(_panel, "RestartText", true).GetComponent<Text>();
        _mainText = Util.FindChild(_panel, "MainText", true).GetComponent<Text>();
        _restartBtn = Util.FindChild(_panel, "RestartBtn", false).GetComponent<Button>();
        _mainBtn = Util.FindChild(_panel, "MainBtn", false).GetComponent<Button>();

        Managers.Language.SetText(_restartText, Define.TextKey.Restart);
        Managers.Language.SetText(_mainText, Define.TextKey.Main);

        UnityAction[] restart = new UnityAction[] { () => Managers.Scene.LoadScene(Define.SceneType.InGame) , () => _restartBtn.interactable = false ,
        () => _mainBtn.interactable = false, () => Managers.Pool.Clear()};

        Util.SetButtonEvent(_restartBtn, restart);
    
        UnityAction[] exit = new UnityAction[] { () => Managers.Scene.LoadScene(Define.SceneType.Main) , () => _restartBtn.interactable = false ,
        () => _mainBtn.interactable = false};

        Util.SetButtonEvent(_mainBtn, exit);

        gameObject.SetActive(false);
    }

    public void SetGameOverUI(int round, int score, bool victory) {
        if (victory)
            Managers.Language.SetText(_logoText, Define.TextKey.Victory);
        else
            Managers.Language.SetText(_logoText, Define.TextKey.GameOver);

        string bestRound = string.Format(Managers.Data.GetLanguage((int)Define.TextKey.HighRound, (int)Managers.Language.CurrentLanguage), round);
       Managers.Language.SetText(_roundText, Define.TextKey.HighRound, true,
            bestRound);

        string bestScore = string.Format(Managers.Data.GetLanguage((int)Define.TextKey.HighScore, (int)Managers.Language.CurrentLanguage), score);
        Managers.Language.SetText(_scoreText, Define.TextKey.HighScore, true,
            bestScore);
    }
}
