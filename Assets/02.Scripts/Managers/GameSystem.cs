using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;
    public const int EnemyMaxLevel = 6;
    public const int TowerMaxLevel = 4;
    public const float TowerAttackRangeImageSize = 10f;
    public const int MaxGameLevel = 25;
    public const int MaxGameHp = 100;

    private int _currentGameScore;
    private int _currentGameHp;
    private int _gameLevel = 1;
    private float _currentTime;
    private const float _maxTime = 60f;
    [SerializeField]private int _currentGold;

    public Action<int> OnGoldEvent;
    public Action<int> OnGameLevelEvent;
    public Action<int> OnGameHpEvent;
    public Action<int> OnScoreEvent;
    public int GameLevel => _gameLevel;

    public int CurrentGold { get { return _currentGold; } set { _currentGold = value; } }
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Managers.Spawn.SpawnEnemy(_gameLevel);
        _currentGameHp = MaxGameHp;
    }

    private void Update() {
        _currentTime += Time.deltaTime;

        if (_currentTime < _maxTime)
            return;

        OnGameLevelEvent?.Invoke(++_gameLevel);
        _currentTime = 0f;
        Managers.Spawn.SpawnEnemy(_gameLevel);
    }

    public bool EnoughGold(int gold) {
        return gold <= _currentGold;
    }


    public void SetScore(int score) {
        _currentGameScore += score;
        OnScoreEvent?.Invoke(score);
    }

    public void SetGold(int gold) {
        _currentGold += gold;
        OnGoldEvent?.Invoke(_currentGold);
        if (CurrentGold < 0)
            CurrentGold = 0;
    }

    public void SetGameHp(int value) {
        _currentGameHp += value;
        OnGameHpEvent?.Invoke(value);

        if(_currentGameHp <= 0) {
            //todo
            //게임종료
        }
    }
}
