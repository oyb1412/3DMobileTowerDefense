using System;
using UnityEngine;

/// <summary>
/// �ֳʹ� �ɷ�ġ
/// </summary>
public class EnemyStatus : MonoBehaviour {
    [SerializeField] private int _level;
    [SerializeField] protected Define.EnemyType _enemyType;

    private int _currentHp;
    private int _maxHp;
    private int _rewardGold;
    private int _rewardScore;
    private int _number;  //��ȯ ����

    private int _physicsDefense;
    private int _magicDefense;
    private float _moveSpeed;
    public Action OnDeadEvent;
    private Sprite _icon;

    public int CurrentHp { get { return _currentHp; } set { _currentHp = value; } }
    public int RewardGold => _rewardGold;
    public int Number => _number;
    public int RewardScore => _rewardScore;
    public int MaxHp => _maxHp;
    public int PhysicsDefense => _physicsDefense;
    public int MagicDefense => _magicDefense;
    public float MoveSpeed => _moveSpeed;
    public Sprite Icon => _icon;


    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    /// <param name="number">���� ����</param>
    public void Init(int number) {
        Data data = Managers.Data;
        _icon = data.GetEnemyIcon((int)_enemyType, _level);
        _maxHp = data.GetEnemyMaxHp((int)_enemyType, _level);
        _currentHp = _maxHp;
        _moveSpeed = data.GetEnemyMoveSpeed((int)_enemyType, _level);
        _physicsDefense = data.GetEnemyPhysicsDefense((int)_enemyType, _level);
        _magicDefense = data.GetEnemyMagicDefense((int)_enemyType, _level);
        _rewardGold = data.GetEnemyProvideGold((int)_enemyType, _level);
        _rewardScore = data.GetEnemyProvideScore((int)_enemyType, _level);
        _number = number;
    }

    /// <summary>
    /// ����� ȣ�� �̺�Ʈ
    /// </summary>
    public void OnDieEvent() {
        OnDeadEvent?.Invoke();
        Managers.Resources.Destroy(transform.parent.gameObject);
    }
}