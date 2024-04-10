using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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


    void Start() {
        Init();
    }

    private void Init() {
        _uiSetting = GameObject.Find("UI_Setting");
        _goldText = Util.FindChild(gameObject, "GoldText", true).GetComponent<Text>();
        _waveText = Util.FindChild(gameObject, "WaveText", true).GetComponent<Text>();
        _scoreText = Util.FindChild(gameObject, "ScoreText", true).GetComponent<Text>();
        _hpText = Util.FindChild(gameObject, "HpText", true).GetComponent<Text>();
        _timeText = Util.FindChild(gameObject, "TimeText", true).GetComponent<Text>();
        _startText = Util.FindChild(gameObject, "StartText", true).GetComponent<Text>();
        _hpSlider = Util.FindChild(gameObject, "HpSlider", true).GetComponent<Slider>();
        _startBtn = Util.FindChild(gameObject, "StartBtn", true).GetComponent<Button>();
        _settingBtn = Util.FindChild(gameObject, "SettingBtn", true).GetComponent<Button>();

        _gameSystem = GameSystem.Instance;

        _goldText.text = $"{_gameSystem.CurrentGold.ToString()}g";
        _gameSystem.OnGoldEvent += ((currentGold) => _goldText.text = $"{currentGold.ToString()}g");

        Managers.Language.SetText(_waveText, Define.TextKey.Wave, true, $"{Managers.Data.GetLanguage((int)Define.TextKey.Wave, (int)Managers.Language.CurrentLanguage)} 0 / {GameSystem.MaxGameLevel}");
        _gameSystem.OnGameLevelEvent += ((level) => _waveText.text = $"{Managers.Data.GetLanguage((int)Define.TextKey.Wave, (int)Managers.Language.CurrentLanguage)} {level} / {GameSystem.MaxGameLevel}");

        Managers.Language.SetText(_scoreText, Define.TextKey.Score, true, $"{Managers.Data.GetLanguage((int)Define.TextKey.Score, (int)Managers.Language.CurrentLanguage)} : 0");
        _gameSystem.OnScoreEvent += ((score) => _scoreText.text = $"{Managers.Data.GetLanguage((int)Define.TextKey.Score, (int)Managers.Language.CurrentLanguage)} : {score}");

        _hpText.text = $"{GameSystem.MaxGameHp}";
        _gameSystem.OnGameHpEvent += ((hp) => _hpText.text = $"{hp}");

        Managers.Language.SetText(_startText, Define.TextKey.StartNextRound);

        _startBtn.interactable = false;
        Util.SetButtonEvent( _startBtn, null, GameStart);
        _gameSystem.OnStartEvent += (() => _startBtn.interactable = true);

        Util.SetButtonEvent(_settingBtn, null, () => _uiSetting.transform.GetChild(0).gameObject.SetActive(true));

        _hpSlider.maxValue = GameSystem.MaxGameHp;
        _hpSlider.value = _hpSlider.maxValue;
        _gameSystem.OnGameHpEvent += ((hp) => _hpSlider.value = hp );

        Managers.Language.SetText(_timeText, Define.TextKey.ToNextRound, true, $"{Managers.Data.GetLanguage((int)Define.TextKey.ToNextRound, (int)Managers.Language.CurrentLanguage)} 60s");
        GameSystem.Instance.OnTimeEvent += ((time) => _timeText.text = $"{Managers.Data.GetLanguage((int)Define.TextKey.ToNextRound, (int)Managers.Language.CurrentLanguage)} {time}s");
        
    }

    private void GameStart() {
        _startBtn.interactable = false;
        _gameSystem.GameStart();
    }
}