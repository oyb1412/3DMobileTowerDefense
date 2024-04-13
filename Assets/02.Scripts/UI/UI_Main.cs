using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 메인 씬 UI
/// </summary>
public class UI_Main : MonoBehaviour
{
    private Button _startBtn;
    private Button _settingBtn;
    private Button _exitBtn;
    private Button _continueBtn;
    private GameObject _uiSetting;

    private Text _startText;
    private Text _continueText;
    private Text _settingText;
    private Text _exitText;

    private void Start() {
        _startBtn = Util.FindChild(gameObject, "StartBtn", false).GetComponent<Button>();
        _continueBtn = Util.FindChild(gameObject, "ContinueBtn", false).GetComponent<Button>();
        _settingBtn = Util.FindChild(gameObject, "SettingBtn", false).GetComponent<Button>();
        _exitBtn = Util.FindChild(gameObject, "ExitBtn", false).GetComponent<Button>();
        _uiSetting = GameObject.Find("UI_Setting_Main");

        _startText = Util.FindChild(gameObject, "StartText", true).GetComponent<Text>();
        _continueText = Util.FindChild(gameObject, "ContinueText", true).GetComponent<Text>();
        _settingText = Util.FindChild(gameObject, "SettingText", true).GetComponent<Text>();
        _exitText = Util.FindChild(gameObject, "ExitText", true).GetComponent<Text>();

        Managers.Language.SetText(_startText, Define.TextKey.GameStart);
        Managers.Language.SetText(_continueText, Define.TextKey.Continue);
        Managers.Language.SetText(_settingText, Define.TextKey.Setting);
        Managers.Language.SetText(_exitText, Define.TextKey.GameExit);

        string Path = string.Format("{0}/{1}.json", Application.persistentDataPath, "SaveData");

        if (File.Exists(Path))  //저장 데이터가 존재할 시 이어하기 가능
            _continueBtn.interactable = true;
        else
            _continueBtn.interactable = false;

        Util.SetButtonEvent(_settingBtn, null, () => _uiSetting.transform.GetChild(0).gameObject.SetActive(true));

        UnityAction[] startBtnAction = new UnityAction[] {
           () => Managers.Scene.LoadScene(Define.SceneType.InGame),() => _startBtn.interactable = false,
           () => _settingBtn.interactable = false, () => _exitBtn.interactable = false, () => _continueBtn.interactable = false,
           () => Managers.Scene.isContinue = false
        };
        Util.SetButtonEvent(_startBtn, startBtnAction);

        UnityAction[] continueBtnAction = new UnityAction[] {
           () => Managers.Scene.LoadScene(Define.SceneType.InGame),() => _startBtn.interactable = false,
           () => _settingBtn.interactable = false, () => _exitBtn.interactable = false, () => _continueBtn.interactable = false,
           () => Managers.Scene.isContinue = true
        };
        Util.SetButtonEvent(_continueBtn, continueBtnAction);

        UnityAction[] exitBtnAction = new UnityAction[] {
           () => Managers.Scene.LoadScene(Define.SceneType.Exit),() => _startBtn.interactable = false,
           () => _settingBtn.interactable = false, () => _exitBtn.interactable = false, () => _continueBtn.interactable = false
        };
        Util.SetButtonEvent(_exitBtn, exitBtnAction);
    }
}
