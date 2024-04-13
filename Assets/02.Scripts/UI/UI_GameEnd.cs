using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// ���� 
/// </summary>
public class UI_GameEnd : MonoBehaviour
{
    private Text _restartText;  //����� �ؽ�Ʈ
    private Text _mainText;  //���� �ؽ�Ʈ
    private Text _roundText;  //�ִ� ���� 
    private Text _logoText;  //���� ���� �ΰ�
    private Text _scoreText;  //�ִ� ����
    private Button _restartBtn;  //����� ��ư
    private Button _mainBtn;  //���� ��ư

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
    /// ���� ���� �� UI Ȱ��ȭ
    /// </summary>
    /// <param name="round">�ְ� ����</param>
    /// <param name="score">�ְ� ����</param>
    /// <param name="victory">�¸�, �й�</param>
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
