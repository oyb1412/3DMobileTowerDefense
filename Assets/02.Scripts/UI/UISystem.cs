using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISystem : UIBase {
    private Text _goldText;
    private Text _waveText;
    private Text _scoreText;
    private Text _hpText;
    private Text _timeText;
    private Slider _hpSlider;
    private GameSystem _gameSystem;
    void Start() {
        Init();
    }

    private void Init() {
        _goldText = Util.FindChild(gameObject, "GoldText", true).GetComponent<Text>();
        _waveText = Util.FindChild(gameObject, "WaveText", true).GetComponent<Text>();
        _scoreText = Util.FindChild(gameObject, "ScoreText", true).GetComponent<Text>();
        _hpText = Util.FindChild(gameObject, "HpText", true).GetComponent<Text>();
        _timeText = Util.FindChild(gameObject, "TimeText", true).GetComponent<Text>();
        _hpSlider = Util.FindChild(gameObject, "HpSlider", true).GetComponent<Slider>();

        _gameSystem = GameSystem.Instance;

        _goldText.text = $"{_gameSystem.CurrentGold.ToString()}g";
        _gameSystem.OnGoldEvent += ((currentGold) => _goldText.text = $"{currentGold.ToString()}g");

        _waveText.text = $"웨이브 0 / {GameSystem.MaxGameLevel}";
        _gameSystem.OnGameLevelEvent += ((level) => _waveText.text = $"웨이브 {level} / {GameSystem.MaxGameLevel}");

        _scoreText.text = $"점수 : 0";
        _gameSystem.OnScoreEvent += ((score) => _scoreText.text = $"점수 : {score}");

        _hpText.text = $"{GameSystem.MaxGameHp}";
        _gameSystem.OnGameHpEvent += ((hp) => _hpText.text = $"{hp}");

        _hpSlider.maxValue = GameSystem.MaxGameHp;
        _hpSlider.value = _hpSlider.maxValue;
        _gameSystem.OnGameHpEvent += ((hp) => _hpSlider.value = hp );

        GameSystem.Instance.OnTimeEvent += ((time) => _timeText.text = $"다음 라운드까지 {time}s");
        
    }
}