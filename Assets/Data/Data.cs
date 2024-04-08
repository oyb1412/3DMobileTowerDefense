using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data {
    private Sprite[,] _towerIcon;

    private string[] _towerInfo = new string[] {
         "빠른 공격속도와 긴 사거리를 지닌 물리공격 타워입니다." ,
         "넓은범위의 적을 동시에 공격하는 물리공격 타워입니다." ,
         "강력한 공격력을 지닌 마법공격 타워입니다.",
         "빠른 공격속도를 지닌 적의 방어타입을 무시하는 타워입니다." 
    };

    private int[,] _towerCost = new int[,] {
        { 70,140,210,280},
        { 80,160,240,320 },
        { 75,150,230,300},
        { 100,170,240,310}
    };
    
    private int[,] _sellCost = new int[,] {
        { 35,75,110,150},
        { 40,85,125,175 },
        { 33,80,115,160},
        { 50,90,150,210}
    };

    private float[,] _towerCreateTime = new float[,] {
        { 3f, 5f, 7f, 9f },
        { 4f, 6f, 8f, 10f },
        { 3f, 5f, 7f, 9f},
        { 6f, 8f, 10f, 10f}
    };

    private int[,] _towerDamage = new int[,] {
        { 8,20,50,70},
        { 12,30,55,80 },
        { 15,40,65,90},
        { 7,18,40,65}
    };

    private float[,] _towerAttackDelay = new float[,] {
        { 1.2f, 1.1f, 1.0f, 1.0f },
        { 2.5f, 2.4f, 2.3f, 2.2f },
        { 1.6f, 1.5f, 1.4f, 1.4f },
        { 0.8f, 0.7f, 0.6f, 0.5f}
    };

    private float[,] _towerAttackRange = new float[,] {
        { 10f, 10f, 12f, 12f },
        { 6f, 6f, 8f, 8f },
        { 8f, 8f, 10f, 10f},
        { 10f, 10f, 12f, 12f },
    };

    
    private int[,] _enemyMaxHp = new int[,] {
        {60,140,400,550,700,900 },
        {50,130,350,470,600,800 },
        {90,250,500,700,900,1100 },
        {100,300,600,900,1200,1400 }
    };

    private int[,] _enemyCurrentHp;

    private float[,] _enemyMoveSpeed = new float[,] {
        {10f,11f,11f,12f,12f,13f },
        {12f,13f,13f,14f,15f,16f },
        {14f,15f,15f,16f,16f,17f },
        {14f,15f,15f,16f,16f,17f },
    };
    
    private int[,] _enemyPhysicsDefense = new int[,] {
        {1,2,3,4,5,6 },
        {0,0,1,1,2,2 },
        {5,7,7,9,9,10 },
        {6,8,8,10,10,12 }
    };
    
    private int[,] _enemyMagicDefense = new int[,] {
        {0,0,1,1,2,2 },
        {2,4,6,8,10,12 },
        {1,3,5,5,7,7 },
        {5,7,7,9,9,11 }
    };

    private int[,] _enemyProvideGold = new int[,] {
        {8,11,14,17,20,26 },
        {10,14,18,22,26,31 },
        {14,19,24,29,34,39 },
        {21,26,31,36,41,46 }
    }; 
    
    private int[,] _enemyProvideScore = new int[,] {
        {5,6,7,8,9,10 },
        {7,8,9,10,11,12 },
        {10,12,14,16,18,20 },
        {20,25,30,35,40,45 }
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

        #region EnemySpawnData

        if (!_enemySpawnData.ContainsKey(1)) {
            _enemySpawnData.Add(1, new List<EnemySpawnData>
            {
                new EnemySpawnData(10, Define.EnemyType.Archer, Define.EnemyLevel.Level1),
                new EnemySpawnData(10, Define.EnemyType.Mage, Define.EnemyLevel.Level1)
            });
        }

        if (!_enemySpawnData.ContainsKey(2)) {
            _enemySpawnData.Add(2, new List<EnemySpawnData>
            {
                new EnemySpawnData(10, Define.EnemyType.Archer, Define.EnemyLevel.Level1),
                new EnemySpawnData(10, Define.EnemyType.Mage, Define.EnemyLevel.Level1)
            });
        }

        if (!_enemySpawnData.ContainsKey(3)) {
            _enemySpawnData.Add(3, new List<EnemySpawnData>
            {
                new EnemySpawnData(7, Define.EnemyType.Archer, Define.EnemyLevel.Level1),
                new EnemySpawnData(7, Define.EnemyType.Mage, Define.EnemyLevel.Level1),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level1),
            });
        }

        if (!_enemySpawnData.ContainsKey(4)) {
            _enemySpawnData.Add(4, new List<EnemySpawnData>
            {
                new EnemySpawnData(7, Define.EnemyType.Archer, Define.EnemyLevel.Level1),
                new EnemySpawnData(7, Define.EnemyType.Mage, Define.EnemyLevel.Level1),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level1),
            });
        }

        if (!_enemySpawnData.ContainsKey(5)) {
            _enemySpawnData.Add(5, new List<EnemySpawnData>
            {
                new EnemySpawnData(10, Define.EnemyType.Archer, Define.EnemyLevel.Level2),
                new EnemySpawnData(10, Define.EnemyType.Mage, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnData.ContainsKey(6)) {
            _enemySpawnData.Add(6, new List<EnemySpawnData>
            {
                 new EnemySpawnData(10, Define.EnemyType.Archer, Define.EnemyLevel.Level2),
                 new EnemySpawnData(10, Define.EnemyType.Mage, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnData.ContainsKey(7)) {
            _enemySpawnData.Add(7, new List<EnemySpawnData>
            {
                new EnemySpawnData(7, Define.EnemyType.Archer, Define.EnemyLevel.Level2),
                new EnemySpawnData(7, Define.EnemyType.Mage, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnData.ContainsKey(8)) {
            _enemySpawnData.Add(8, new List<EnemySpawnData>
            {
                new EnemySpawnData(7, Define.EnemyType.Archer, Define.EnemyLevel.Level2),
                new EnemySpawnData(7, Define.EnemyType.Mage, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnData.ContainsKey(9)) {
            _enemySpawnData.Add(9, new List<EnemySpawnData>
            {
                new EnemySpawnData(5, Define.EnemyType.Archer, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Mage, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Speaman, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnData.ContainsKey(10)) {
            _enemySpawnData.Add(10, new List<EnemySpawnData>
            {
                new EnemySpawnData(5, Define.EnemyType.Archer, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Mage, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Speaman, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnData.ContainsKey(11)) {
            _enemySpawnData.Add(11, new List<EnemySpawnData>
            {
                new EnemySpawnData(8, Define.EnemyType.Archer, Define.EnemyLevel.Level3),
                new EnemySpawnData(8, Define.EnemyType.Mage, Define.EnemyLevel.Level3),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Speaman, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnData.ContainsKey(12)) {
            _enemySpawnData.Add(12, new List<EnemySpawnData>
            {
                new EnemySpawnData(8, Define.EnemyType.Archer, Define.EnemyLevel.Level3),
                new EnemySpawnData(8, Define.EnemyType.Mage, Define.EnemyLevel.Level3),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level3),
                new EnemySpawnData(5, Define.EnemyType.Speaman, Define.EnemyLevel.Level3),
            });
        }

        if (!_enemySpawnData.ContainsKey(13)) {
            _enemySpawnData.Add(13, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Swordman, Define.EnemyLevel.Level3),
                new EnemySpawnData(15, Define.EnemyType.Speaman, Define.EnemyLevel.Level3),
            });
        }

        if (!_enemySpawnData.ContainsKey(14)) {
            _enemySpawnData.Add(14, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level3),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level3),
                new EnemySpawnData(15, Define.EnemyType.Swordman, Define.EnemyLevel.Level3),
                new EnemySpawnData(15, Define.EnemyType.Speaman, Define.EnemyLevel.Level3),
            });
        }

        if (!_enemySpawnData.ContainsKey(15)) {
            _enemySpawnData.Add(15, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level4),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level4),
            });
        }

        if (!_enemySpawnData.ContainsKey(16)) {
            _enemySpawnData.Add(16, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level4),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level4),
                new EnemySpawnData(15, Define.EnemyType.Swordman, Define.EnemyLevel.Level4),
            });
        }

        if (!_enemySpawnData.ContainsKey(17)) {
            _enemySpawnData.Add(17, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level4),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level4),
                new EnemySpawnData(15, Define.EnemyType.Swordman, Define.EnemyLevel.Level4),
                new EnemySpawnData(15, Define.EnemyType.Speaman, Define.EnemyLevel.Level4),
            });
        }

        if (!_enemySpawnData.ContainsKey(18)) {
            _enemySpawnData.Add(18, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level5),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level5),
                new EnemySpawnData(10, Define.EnemyType.Swordman, Define.EnemyLevel.Level5),
            });
        }

        if (!_enemySpawnData.ContainsKey(19)) {
            _enemySpawnData.Add(19, new List<EnemySpawnData>
            {
                new EnemySpawnData(10, Define.EnemyType.Archer, Define.EnemyLevel.Level5),
                new EnemySpawnData(10, Define.EnemyType.Mage, Define.EnemyLevel.Level5),
                new EnemySpawnData(10, Define.EnemyType.Swordman, Define.EnemyLevel.Level5),
                new EnemySpawnData(10, Define.EnemyType.Speaman, Define.EnemyLevel.Level5),
            });
        }

        if (!_enemySpawnData.ContainsKey(20)) {
            _enemySpawnData.Add(20, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level5),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level5),
                new EnemySpawnData(15, Define.EnemyType.Swordman, Define.EnemyLevel.Level5),
                new EnemySpawnData(15, Define.EnemyType.Speaman, Define.EnemyLevel.Level5),
            });
        }

        if (!_enemySpawnData.ContainsKey(21)) {
            _enemySpawnData.Add(21, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level6),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level6),
                new EnemySpawnData(10, Define.EnemyType.Swordman, Define.EnemyLevel.Level6),
            });
        }

        if (!_enemySpawnData.ContainsKey(22)) {
            _enemySpawnData.Add(22, new List<EnemySpawnData>
            {
                new EnemySpawnData(10, Define.EnemyType.Archer, Define.EnemyLevel.Level6),
                new EnemySpawnData(10, Define.EnemyType.Mage, Define.EnemyLevel.Level6),
                new EnemySpawnData(10, Define.EnemyType.Swordman, Define.EnemyLevel.Level6),
                new EnemySpawnData(10, Define.EnemyType.Speaman, Define.EnemyLevel.Level6),
            });
        }

        if (!_enemySpawnData.ContainsKey(23)) {
            _enemySpawnData.Add(23, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level6),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level6),
                new EnemySpawnData(15, Define.EnemyType.Swordman, Define.EnemyLevel.Level6),
                new EnemySpawnData(15, Define.EnemyType.Speaman, Define.EnemyLevel.Level6),
            });
        }

        if (!_enemySpawnData.ContainsKey(24)) {
            _enemySpawnData.Add(24, new List<EnemySpawnData>
            {
                new EnemySpawnData(20, Define.EnemyType.Archer, Define.EnemyLevel.Level6),
                new EnemySpawnData(20, Define.EnemyType.Mage, Define.EnemyLevel.Level6),
                new EnemySpawnData(20, Define.EnemyType.Swordman, Define.EnemyLevel.Level6),
                new EnemySpawnData(20, Define.EnemyType.Speaman, Define.EnemyLevel.Level6),
            });
        }

        if (!_enemySpawnData.ContainsKey(25)) {
            _enemySpawnData.Add(25, new List<EnemySpawnData>
            {
                new EnemySpawnData(25, Define.EnemyType.Archer, Define.EnemyLevel.Level6),
                new EnemySpawnData(25, Define.EnemyType.Mage, Define.EnemyLevel.Level6),
                new EnemySpawnData(25, Define.EnemyType.Swordman, Define.EnemyLevel.Level6),
                new EnemySpawnData(25, Define.EnemyType.Speaman, Define.EnemyLevel.Level6),
            });
        }

        #endregion
    }
    public List<EnemySpawnData> GetEnemySpawnData(int level) {
        return _enemySpawnData[level];
    }

    public string GetTowerInfo(int type) => _towerInfo[type];
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
