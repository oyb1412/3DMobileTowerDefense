using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour {
    [SerializeField] private int _level;
    [SerializeField] protected Define.EnemyType _enemyType;
    public Define.EnemyType EnemyType => _enemyType;
    private int _currentHp;
    private int _maxHp;
    private int _provideGold;
    private int _provideScore;
    private Sprite _icon;

    private float _moveSpeed;
    private int _physicsDefense;
    private int _magicDefense;
    public Action OnDeadEvent;
    public int CurrentHp { get { return _currentHp; } set { _currentHp = value; } }
        
    public Sprite Icon => _icon;
    public int ProvideGold => _provideGold;
    public int ProvideScore => _provideScore;
    public int MaxHp => _maxHp;
    public float MoveSpeed => _moveSpeed;
    public int PhysicsDefense => _physicsDefense;
    public int MagicDefense => _magicDefense;
    public int Level => _level;

    public void Init() {
        Data data = Managers.Data;
        _icon = data.GetEnemyIcon((int)_enemyType, _level);
        _maxHp = data.GetEnemyMaxHp((int)_enemyType, _level);
        _currentHp = _maxHp;
        _moveSpeed = data.GetEnemyMoveSpeed((int)_enemyType, _level);
        _physicsDefense = data.GetEnemyPhysicsDefense((int)_enemyType, _level);
        _magicDefense = data.GetEnemyMagicDefense((int)_enemyType, _level);
        _provideGold = data.GetEnemyProvideGold((int)_enemyType, _level);
        _provideScore = data.GetEnemyProvideScore((int)_enemyType, _level);
    }

    public void OnDieEvent() {
        OnDeadEvent?.Invoke();
        Managers.Resources.Destroy(transform.parent.gameObject);
    }
}