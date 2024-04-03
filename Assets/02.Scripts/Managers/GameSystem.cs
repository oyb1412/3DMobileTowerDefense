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
    private int _gameLevel = 1;
    private float _currentTime;
    private const float _maxTime = 30f;
    [SerializeField]private int _currentGold;
    public Action<int> OnGoldEvent;
    public int GameLevel => _gameLevel;

    public int CurrentGold { get { return _currentGold; } set { _currentGold = value; } }
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Managers.Spawn.SpawnEnemy(_gameLevel);
    }

    private void Update() {
        _currentTime += Time.deltaTime;

        if (_currentTime < _maxTime)
            return;

        _gameLevel++;
        _currentTime = 0f;
        Managers.Spawn.SpawnEnemy(_gameLevel);
    }

    public bool EnoughGold(int gold) {
        return gold <= _currentGold;
    }

    public void SetGold(int gold) {
        _currentGold += gold;
        OnGoldEvent?.Invoke(_currentGold);
        if (CurrentGold < 0)
            CurrentGold = 0;
    }
}
