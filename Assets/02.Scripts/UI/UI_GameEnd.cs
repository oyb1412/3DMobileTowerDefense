using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 게임 
/// </summary>
public class UI_GameEnd : MonoBehaviour
{
    private Text _restartText;  //재시작 텍스트
    private Text _mainText;  //메인 텍스트
    private Text _roundText;  //최대 라운드 
    private Text _logoText;  //게임 종료 로고
    private Text _scoreText;  //최대 점수
    private Button _restartBtn;  //재시작 버튼
    private Button _mainBtn;  //메인 버튼

    void Start()
    {
        _roundText = Util.FindChild(gameObject, "RoundText", true).GetComponent<Text>();
        _logoText = Util.FindChild(gameObject, "Logo", true).GetComponent<Text>();
        _scoreText = Util.FindChild(gameObject, "ScoreText", true).GetComponent<Text>();
        _restartText = Util.FindChild(gameObject, "RestartText", true).GetComponent<Text>();
        _mainText = Util.FindChild(gameObject, "MainText", true).GetComponent<Text>();
        _restartBtn = Util.FindChild(gameObject, "RestartBtn", true).GetComponent<Button>();
        _mainBtn = Util.FindChild(gameObject, "MainBtn", true).GetComponent<Button>();

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

    /// <summary>
    /// 게임 종료 시 UI 활성화
    /// </summary>
    /// <param name="round">최고 라운드</param>
    /// <param name="score">최고 점수</param>
    /// <param name="victory">승리, 패배</param>
    public void SetGameEndUI(int round, int score, bool victory) {
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
