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

    private float _moveSpeed;
    private float _physicsDefense;
    private float _magicDefense;

    public int CurrentHp { get { return _currentHp; } set { _currentHp = value; } }
        
    public int ProvideGold => _provideGold;
    public int MaxHp => _maxHp;
    public float MoveSpeed => _moveSpeed;
    public float PhysicsDefense => _physicsDefense;
    public float MagicDefense => _magicDefense;
    public int Level => _level;

    public void Init() {
        Data data = Managers.Data;
        _maxHp = data.GetEnemyMaxHp((int)_enemyType, _level);
        _currentHp = _maxHp;
        _moveSpeed = data.GetEnemyMoveSpeed((int)_enemyType, _level);
        _physicsDefense = data.GetEnemyPhysicsDefense((int)_enemyType, _level);
        _magicDefense = data.GetEnemyMagicDefense((int)_enemyType, _level);
        _provideGold = data.GetEnemyProvideGold((int)_enemyType, _level);
    }

    public void OnDieEvent() {
        Managers.Resources.Destroy(transform.root.gameObject);
    }


}