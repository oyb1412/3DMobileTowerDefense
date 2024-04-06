using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TowerStatus : MonoBehaviour
{
    private Sprite _icon;
    private int _level;
    private int _attackDamage;
    private float _attackRange;
    private float _attackDelay;
    private int _killNumber;
    private Define.TowerType _towerType;

    public int AttackDamage => _attackDamage;
    public float AttackRange => _attackRange;
    public float AttackDelay => _attackDelay;

    public Sprite Icon => _icon;
    public int KillNumber { get { return _killNumber; } set { _killNumber = value; } }
    public Define.TowerType TowerType { get { return _towerType; } set { _towerType = value; } }

    public int Level { get { return _level; } set { _level = value; } }

    public void Init(int killNumber, int level, Vector3 pos, Define.TowerType type) {
        _killNumber = killNumber;
        _level = level;
        transform.position = pos;
        _towerType = type;
        Data data = Managers.Data;
        _attackDamage = data.GetTowerAttackDamage((int)type, level);
        _attackRange = data.GetTowerAttacmRange((int)type, level);
        _attackDelay = data.GetTowerAttacnDelay((int)type, level);
        _icon = data.GetTowerIcon((int)type, level);
    }
}
