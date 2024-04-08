using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;
    private UI_GameOver _gameoverUI;
    private ParticleSystem _summonEffect;
    private Define.GameState _gameState = Define.GameState.Play;
    public const int EnemyMaxLevel = 6;
    public const int TowerMaxLevel = 4;
    public const float TowerAttackRangeImageSize = 15f;
    public const int MaxGameLevel = 25;
    public const int MaxGameHp = 100;

    private int _currentGameScore;
    private int _currentGameHp;
    private int _gameLevel = 0;
    private float _currentTime;
    [SerializeField]private float _maxTime = 30f;
    [SerializeField]private int _currentGold;

    public Action<int> OnGoldEvent;
    public Action<int> OnGameLevelEvent;
    public Action<int> OnGameHpEvent;
    public Action<int> OnScoreEvent;
    public Action<int> OnTimeEvent;
    public int GameLevel => _gameLevel;
    public Define.GameState GameState => _gameState;
    public int CurrentGold { get { return _currentGold; } set { _currentGold = value; } }
    private void Awake() {
        Instance = this;
        _gameoverUI = GameObject.Find("UI_GameOver").GetComponent<UI_GameOver>();
    }

    private void Start() {
        _summonEffect = GetComponentInChildren<ParticleSystem>();
        _currentGameHp = MaxGameHp;
    }

  
    private void Update() {
        if (!IsPlay())
            return;

        _currentTime += Time.deltaTime;
        OnTimeEvent?.Invoke((int)_maxTime - (int)_currentTime);
        if (_currentTime < _maxTime)
            return;

        _gameLevel++;

        if(_gameLevel > MaxGameLevel) {
            _gameoverUI.gameObject.SetActive(true);
            _gameoverUI.SetGameOverUI(_gameLevel, _currentGameScore, true);
            _gameState = Define.GameState.GameOver;
            return;
        }

        OnGameLevelEvent?.Invoke(_gameLevel);
        _currentTime = 0f;
        Managers.Spawn.SpawnEnemy(_gameLevel, _summonEffect);
    }

    public bool IsPlay() {
        return _gameState == Define.GameState.Play;
    }

    public bool EnoughGold(int gold) {
        return gold <= _currentGold;
    }


    public void SetScore(int score) {
        _currentGameScore += score;
        OnScoreEvent?.Invoke(_currentGameScore);
    }

    public void SetGold(int gold) {
        _currentGold += gold;
        OnGoldEvent?.Invoke(_currentGold);
        if (CurrentGold < 0)
            CurrentGold = 0;
    }

    public void SetGameHp(int value) {
        _currentGameHp += value;
        OnGameHpEvent?.Invoke(_currentGameHp);

        if(_currentGameHp <= 0) {
            _gameoverUI.gameObject.SetActive(true);
            _gameoverUI.SetGameOverUI(_gameLevel, _currentGameScore, false);
            _gameState = Define.GameState.GameOver;
        }
    }
}
