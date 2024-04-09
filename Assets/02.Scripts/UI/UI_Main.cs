using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    private Button _startBtn;
    private Button _settingBtn;
    private Button _exitBtn;
    private GameObject _uiSetting;

    private void Start() {
        _startBtn = Util.FindChild(gameObject, "StartBtn", false).GetComponent<Button>();
        _settingBtn = Util.FindChild(gameObject, "SettingBtn", false).GetComponent<Button>();
        _exitBtn = Util.FindChild(gameObject, "ExitBtn", false).GetComponent<Button>();
        _uiSetting = GameObject.Find("UI_Setting");  

        _startBtn.onClick.AddListener(() => Managers.Scene.LoadScene(Define.SceneType.InGame));
        _startBtn.onClick.AddListener(() => _startBtn.interactable = false);
        _startBtn.onClick.AddListener(() => _settingBtn.interactable = false);
        _startBtn.onClick.AddListener(() => _exitBtn.interactable = false);

        _settingBtn.onClick.AddListener(() => _uiSetting.SetActive(true)); ;

        _exitBtn.onClick.AddListener(() => Managers.Scene.LoadScene(Define.SceneType.Exit));
        _exitBtn.onClick.AddListener(() => _startBtn.interactable = false);
        _exitBtn.onClick.AddListener(() => _settingBtn.interactable = false);
        _exitBtn.onClick.AddListener(() => _exitBtn.interactable = false);


    }
}
