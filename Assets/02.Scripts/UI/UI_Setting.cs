using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : MonoBehaviour
{
    private Button _exitBtn;
    private Slider _bgmVolumeSlider;
    private Text _bgmVolumeText; 
    private Slider _sfxVolumeSlider;
    private Text _sfxVolumeText;  
    private Slider _sensitivitySlider;
    private Text _sensitivityText;
    private GameObject _panels;

    private Text _languageText;
    private Text _bgmText;
    private Text _sfxText;
    private Text _sensText;

    private float _saveSens;

    private Dropdown _languageDropdown;
    private void Awake() {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        _exitBtn = Util.FindChild(gameObject, "ExitBtn", true).GetComponent<Button>();
        _panels = Util.FindChild(gameObject, "Panels", false);

        Util.SetButtonEvent(_exitBtn, null, PanelDisable);

        _bgmVolumeSlider = Util.FindChild(gameObject, "BgmVolumeSlider", true).GetComponent<Slider>();
        _bgmVolumeText = Util.FindChild(gameObject, "CurrentBgmVolumeText", true).GetComponent<Text>();

        _sfxVolumeSlider = Util.FindChild(gameObject, "SfxVolumeSlider", true).GetComponent<Slider>();
        _sfxVolumeText = Util.FindChild(gameObject, "CurrentSfxVolumeText", true).GetComponent<Text>();

        _sensitivitySlider = Util.FindChild(gameObject, "SensitivitySlider", true).GetComponent<Slider>();
        _sensitivityText = Util.FindChild(gameObject, "CurrentSensitivityText", true).GetComponent<Text>();

        _languageDropdown = Util.FindChild(gameObject, "LanguageDropdown", true).GetComponent<Dropdown>();

        _languageText = Util.FindChild(gameObject, "LanguageText", true).GetComponent<Text>();
        _bgmText = Util.FindChild(gameObject, "BgmVolumeText", true).GetComponent<Text>();
        _sfxText = Util.FindChild(gameObject, "SfxVolumeText", true).GetComponent<Text>();
        _sensText = Util.FindChild(gameObject, "SensitivityText", true).GetComponent<Text>();

        _languageDropdown.onValueChanged.AddListener((amount) => Managers.Language.ChangeLanguage((Define.Language)amount));

        Managers.Language.SetText(_languageText, Define.TextKey.LanguageSetting);
        Managers.Language.SetText(_bgmText, Define.TextKey.BgmVolumeSetting);
        Managers.Language.SetText(_sfxText, Define.TextKey.SfxVolumeSetting);
        Managers.Language.SetText(_sensText, Define.TextKey.SensitivitySetting);

        _bgmVolumeSlider.onValueChanged.AddListener(SetBgmVolume);
        _sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
        _sensitivitySlider.onValueChanged.AddListener(SetSensitivity);

        PanelDisable();
    }


    private void PanelDisable() {
        if (Managers.Scene.CurrentScene is GameScene) {
            Time.timeScale = 1f;
        }
        _panels.SetActive(false);
    }

    private void SetBgmVolume(float volume) {
        Managers.Audio.SetBgmVolume(volume);
        _bgmVolumeText.text = ((int)(volume * 100)).ToString();
    }

    private void SetSfxVolume(float volume) {
        Managers.Audio.SetSfxVolume(volume);
        _sfxVolumeText.text = ((int)(volume * 100)).ToString();
    }

    private void SetSensitivity(float sens) {
        Managers.MainCamera.SetCameraSens(sens);
        _sensitivityText.text = ((int)(sens * 100)).ToString();
    }
}
