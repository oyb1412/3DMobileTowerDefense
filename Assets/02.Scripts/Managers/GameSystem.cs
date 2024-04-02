using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;
    public const int TowerMaxLevel = 4;
    public const float TowerAttackRangeImageSize = 10f;
    [SerializeField]private int _currentGold;
    public Action<int> OnGoldEvent;

    public int CurrentGold { get { return _currentGold; } set { _currentGold = value; } }
    private void Awake() {
        Instance = this;
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
