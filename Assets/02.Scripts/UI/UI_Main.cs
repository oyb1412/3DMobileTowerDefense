using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    private Button _startBtn;
    private Button _settingBtn;
    private Button _exitBtn;

    private void Start() {
        _startBtn = Util.FindChild(gameObject, "StartBtn", false).GetComponent<Button>();
        _settingBtn = Util.FindChild(gameObject, "SettingBtn", false).GetComponent<Button>();
        _exitBtn = Util.FindChild(gameObject, "ExitBtn", false).GetComponent<Button>();

        _startBtn.onClick.AddListener(() => Managers.Scene.LoadScene(Define.SceneType.InGame));
        //_settingBtn.onClick.AddListener
        _exitBtn.onClick.AddListener(() => Util.ExitGame());
    }
}
