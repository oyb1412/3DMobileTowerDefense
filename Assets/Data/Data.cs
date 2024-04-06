using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data {
    private Sprite[,] _towerIcon;

    private int[,] _towerCost = new int[,] {
        { 5,10,15,20},
        { 6,12,18,24 },
        { 7,14,21,28},
        { 10,20,30,40}
    };
    
    private int[,] _sellCost = new int[,] {
        { 3,6,9,12},
        { 4,8,12,16 },
        { 5,15,20,25},
        { 6,12,18,24}
    };

    private float[,] _towerCreateTime = new float[,] {
        { 3f, 10f, 15f, 20f },
        { 3f,12f,18f,24f },
        { 3f,14f,21f,28f},
        { 3f,20f,30f,40f}
    };

    private int[,] _towerDamage = new int[,] {
        { 3,6,9,12},
        { 4,8,12,16 },
        { 5,15,20,25},
        { 3,12,18,24}
    };

    private float[,] _towerAttackDelay = new float[,] {
        { 1f, 0.9f, 0.8f, 0.7f },
        { 3f,2.5f,2.5f,2f },
        { 2f,1.8f,1.6f,1.4f},
        { 0.5f,0.5f,0.5f,0.5f}
    };

    private float[,] _towerAttackRange = new float[,] {
        { 8f, 9f, 10f, 11f },
        { 6f,7f,8f,9f },
        { 7f,8f,9f,10f},
        { 10f,11f,12f,12f}
    };

    
    private int[,] _enemyMaxHp = new int[,] {
        {10,20,30,40,50,60 },
        {15,30,45,60,75,90 },
        {20,40,60,80,100,120 },
        {25,50,75,100,125,150 }
    };

    private int[,] _enemyCurrentHp;

    private float[,] _enemyMoveSpeed = new float[,] {
        {10f,10.1f,10.2f,10.3f,10.4f,10.5f },
        {10.1f,10.2f,10.3f,10.4f,10.5f,10.6f },
        {10.2f,10.3f,10.4f,10.5f,10.6f,10.7f },
        {10.3f,10.4f,10.5f,10.6f,10.7f,10.8f }
    };
    
    private int[,] _enemyPhysicsDefense = new int[,] {
        {1,1,1,1,1,1 },
        {1,1,1,1,1,1 },
        {1,1,1,1,1,1 },
        {1,1,1,1,1,1 }
    };
    
    private int[,] _enemyMagicDefense = new int[,] {
        {1,1,1,1,1,1 },
        {1,1,1,1,1,1 },
        {1,1,1,1,1,1 },
        {1,1,1,1,1,1 }
    };

    private int[,] _enemyProvideGold = new int[,] {
        {5,5,5,5,5,5 },
        {5,5,5,5,5,5 },
        {5,5,5,5,5,5 },
        {5,5,5,5,5,5 }
    }; 
    
    private int[,] _enemyProvideScore = new int[,] {
        {5,5,5,5,5,5 },
        {5,5,5,5,5,5 },
        {5,5,5,5,5,5 },
        {5,5,5,5,5,5 }
    };

    private Sprite[,] _enemyIcon;

    public class EnemySpawnData {
        public int Count;
        public Define.EnemyType EnemyType;
        public Define.EnemyLevel EnemyLevel;

        public EnemySpawnData(int count, Define.EnemyType type, Define.EnemyLevel enemyLevel) {
            Count = count;
            EnemyType = type;
            EnemyLevel = enemyLevel;
        }
    }

    private Dictionary<int, List<EnemySpawnData>> _enemySpawnData = new Dictionary<int, List<EnemySpawnData>>();

    private static Material _defaultMaterial;
    private static Material _greenMaterial;
    public Material DefaultMaterial => _defaultMaterial;
    public Material GreenMaterial => _greenMaterial;

    public void Init() {
        _defaultMaterial = (Material)Resources.Load<Material>("Material/Default");
        _greenMaterial = (Material)Resources.Load<Material>("Material/Green");
        _enemyCurrentHp = new int[(int)Define.EnemyType.Count, (int)Define.EnemyLevel.Count];
        _towerIcon = new Sprite[(int)Define.TowerType.Count, (int)Define.TowerLevel.Count];
        _enemyIcon = new Sprite[(int)Define.EnemyType.Count, (int)Define.EnemyLevel.Count];

        #region IconSpriteInit

        for (int i = 0; i< (int)Define.TowerLevel.Count; i++) {
            _towerIcon[(int)Define.TowerType.ArcherTower, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Towers/ArcherTower/ArcherTower_Lvl{i + 1}");
        }

        for (int i = 0; i < (int)Define.TowerLevel.Count; i++) {
            _towerIcon[(int)Define.TowerType.CanonTower, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Towers/CanonTower/CanonTower_Lvl{i + 1}");
        }

        for (int i = 0; i < (int)Define.TowerLevel.Count; i++) {
            _towerIcon[(int)Define.TowerType.MagicTower, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Towers/MagicTower/MagicTower_Lvl{i + 1}");
        }

        for (int i = 0; i < (int)Define.TowerLevel.Count; i++) {
            _towerIcon[(int)Define.TowerType.DeathTower, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Towers/DeathTower/DeathTower_Lvl{i + 1}");
        }

        for(int i = 0; i< (int)Define.EnemyLevel.Count; i++) {
            _enemyIcon[(int)Define.EnemyType.Archer, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Units/Level{i + 1}/Archer_Level{i + 1}");
        }

        for (int i = 0; i < (int)Define.EnemyLevel.Count; i++) {
            _enemyIcon[(int)Define.EnemyType.Swordman, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Units/Level{i + 1}/Swordman_Level{i + 1}");
        }

        for (int i = 0; i < (int)Define.EnemyLevel.Count; i++) {
            _enemyIcon[(int)Define.EnemyType.Mage, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Units/Level{i + 1}/Mage_Level{i + 1}");
        }

        for (int i = 0; i < (int)Define.EnemyLevel.Count; i++) {
            _enemyIcon[(int)Define.EnemyType.Speaman, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Units/Level{i + 1}/Speaman_Level{i + 1}");
        }
        #endregion

        if(!_enemySpawnData.ContainsKey(1)) {
            _enemySpawnData.Add(1, new List<EnemySpawnData>
            {
                new EnemySpawnData(10, Define.EnemyType.Archer, Define.EnemyLevel.Level1),
                new EnemySpawnData(10, Define.EnemyType.Swordman, Define.EnemyLevel.Level1)
            });
        }
       
    }
    public List<EnemySpawnData> GetEnemySpawnData(int level) {
        return _enemySpawnData[level];
    }
    public int GetTowerCost(int type, int level) => _towerCost[type, level - 1];
    public int GetTowerAttackDamage(int type, int level) => _towerDamage[type, level - 1];
    public float GetTowerAttacmRange(int type, int level) => _towerAttackRange[type, level - 1];
    public float GetTowerAttacnDelay(int type, int level) => _towerAttackDelay[type, level - 1];
    public int GetSellCost(int type, int level) => _sellCost[type, level - 1];
    public float GetTowerCreateTime(int type, int level) => _towerCreateTime[type, level - 1];
    public int GetEnemyMaxHp(int type, int level) => _enemyMaxHp[type, level - 1];
    public float GetEnemyMoveSpeed(int type, int level) => _enemyMoveSpeed[type, level - 1];
    public int GetEnemyPhysicsDefense(int type, int level) => _enemyPhysicsDefense[type, level - 1];
    public int GetEnemyMagicDefense(int type, int level) => _enemyMagicDefense[type, level - 1];
    public int GetEnemyProvideGold(int type, int level) => _enemyProvideGold[type, level - 1];
    public int GetEnemyProvideScore(int type, int level) => _enemyProvideScore[type, level - 1];

    public Sprite GetTowerIcon(int type, int level) => _towerIcon[type, level - 1];
    public Sprite GetEnemyIcon(int type, int level) => _enemyIcon[type, level - 1];
}
