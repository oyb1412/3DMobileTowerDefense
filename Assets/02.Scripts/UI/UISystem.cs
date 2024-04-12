using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Data;

public class UISystem : UIBase {
    private GameObject _uiSetting;
    private Text _goldText;
    private Text _waveText;
    private Text _scoreText;
    private Text _hpText;
    private Text _timeText;
    private Text _startText;
    private Slider _hpSlider;
    private Button _startBtn;
    private Button _settingBtn;
    private GameSystem _gameSystem;
    private Data.GameSystemData _saveData;

    void Start() {
        Init();
    }

    private void Init() {

        _uiSetting = GameObject.Find("UI_Setting_Ingame");
        _goldText = Util.FindChild(gameObject, "GoldText", true).GetComponent<Text>();
        _waveText = Util.FindChild(gameObject, "WaveText", true).GetComponent<Text>();
        _scoreText = Util.FindChild(gameObject, "ScoreText", true).GetComponent<Text>();
        _hpText = Util.FindChild(gameObject, "HpText", true).GetComponent<Text>();
        _timeText = Util.FindChild(gameObject, "TimeText", true).GetComponent<Text>();
        _startText = Util.FindChild(gameObject, "StartText", true).GetComponent<Text>();
        _hpSlider = Util.FindChild(gameObject, "HpSlider", true).GetComponent<Slider>();
        _startBtn = Util.FindChild(gameObject, "StartBtn", true).GetComponent<Button>();
        _settingBtn = Util.FindChild(gameObject, "SettingBtn", false).GetComponent<Button>();
        _gameSystem = GameSystem.Instance;

        if (!Managers.Scene.isContinue) {
            StartInit();
        }
        else {
            Continue();
        }
    }

    private void Continue() {
        _saveData = Managers.Data.GetSaveData();

        _goldText.text = $"{_saveData.CurrentGold.ToString()}g";
        _gameSystem.OnGoldEvent += ((currentGold) => _goldText.text = $"{currentGold.ToString()}g");

        string wave = string.Format(Managers.Data.GetLanguage((int)Define.TextKey.Wave, (int)Managers.Language.CurrentLanguage), _saveData.CurrentRound, GameSystem.MaxGameLevel);

        Managers.Language.SetText(_waveText, Define.TextKey.Wave, true, wave);
        _gameSystem.OnGameLevelEvent += ((level) => _waveText.text = string.Format(Managers.Data.GetLanguage((int)Define.TextKey.Wave, (int)Managers.Language.CurrentLanguage), level, GameSystem.MaxGameLevel));

        string score = string.Format(Managers.Data.GetLanguage((int)Define.TextKey.Score, (int)Managers.Language.CurrentLanguage), _saveData.CurrentScore);

        Managers.Language.SetText(_scoreText, Define.TextKey.Score, true, score);
        _gameSystem.OnScoreEvent += ((score) => _scoreText.text = string.Format(Managers.Data.GetLanguage((int)Define.TextKey.Score, (int)Managers.Language.CurrentLanguage), score));

        _hpText.text = $"{_saveData.CurrentHp}";
        _gameSystem.OnGameHpEvent += ((hp) => _hpText.text = $"{hp}");

        Managers.Language.SetText(_startText, Define.TextKey.StartNextRound);

        _startBtn.interactable = false;
        Util.SetButtonEvent(_startBtn, null, GameStart);
        _gameSystem.OnStartEvent += (() => _startBtn.interactable = true);

        Util.SetButtonEvent(_settingBtn, null, PanelEnable);

        _hpSlider.maxValue = GameSystem.MaxGameHp;
        _hpSlider.value = _saveData.CurrentHp;
        _gameSystem.OnGameHpEvent += ((hp) => _hpSlider.value = hp);

        Managers.Language.SetText(_timeText, Define.TextKey.ToNextRound, true, $"{Managers.Data.GetLanguage((int)Define.TextKey.ToNextRound, (int)Managers.Language.CurrentLanguage)} 60s");
        GameSystem.Instance.OnTimeEvent += ((time) => _timeText.text = $"{Managers.Data.GetLanguage((int)Define.TextKey.ToNextRound, (int)Managers.Language.CurrentLanguage)} {time}s");
    }

    private void StartInit() {

        _goldText.text = $"{_gameSystem.CurrentGold.ToString()}g";
        _gameSystem.OnGoldEvent += ((currentGold) => _goldText.text = $"{currentGold.ToString()}g");
        var qw = Managers.Data.GetLanguage((int)Define.TextKey.Wave, (int)Managers.Language.CurrentLanguage);
        string wave = string.Format(Managers.Data.GetLanguage((int)Define.TextKey.Wave, (int)Managers.Language.CurrentLanguage), 0,  GameSystem.MaxGameLevel);
        Managers.Language.SetText(_waveText, Define.TextKey.Wave, true, wave);
        _gameSystem.OnGameLevelEvent += ((level) => _waveText.text = string.Format(Managers.Data.GetLanguage((int)Define.TextKey.Wave, (int)Managers.Language.CurrentLanguage), level, GameSystem.MaxGameLevel));

        string score = string.Format(Managers.Data.GetLanguage((int)Define.TextKey.Score, (int)Managers.Language.CurrentLanguage), 0);

        Managers.Language.SetText(_scoreText, Define.TextKey.Score, true, score);
        _gameSystem.OnScoreEvent += ((score) => _scoreText.text = string.Format(Managers.Data.GetLanguage((int)Define.TextKey.Score, (int)Managers.Language.CurrentLanguage), score));

        _hpText.text = $"{GameSystem.MaxGameHp}";
        _gameSystem.OnGameHpEvent += ((hp) => _hpText.text = $"{hp}");

        Managers.Language.SetText(_startText, Define.TextKey.StartNextRound);

        _startBtn.interactable = false;
        Util.SetButtonEvent(_startBtn, null, GameStart);
        _gameSystem.OnStartEvent += (() => _startBtn.interactable = true);

        Util.SetButtonEvent(_settingBtn, null, PanelEnable);

        _hpSlider.maxValue = GameSystem.MaxGameHp;
        _hpSlider.value = _hpSlider.maxValue;
        _gameSystem.OnGameHpEvent += ((hp) => _hpSlider.value = hp);

        Managers.Language.SetText(_timeText, Define.TextKey.ToNextRound, true, $"{Managers.Data.GetLanguage((int)Define.TextKey.ToNextRound, (int)Managers.Language.CurrentLanguage)} 60s");
        GameSystem.Instance.OnTimeEvent += ((time) => _timeText.text = $"{Managers.Data.GetLanguage((int)Define.TextKey.ToNextRound, (int)Managers.Language.CurrentLanguage)} {time}s");
    }



    private void PanelEnable() {
        if (Managers.Scene.CurrentScene is GameScene) {
            Time.timeScale = 0f;
        }
        _uiSetting.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void GameStart() {
        _startBtn.interactable = false;
        _gameSystem.GameStart();
    }
}