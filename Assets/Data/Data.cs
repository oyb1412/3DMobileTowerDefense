using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;
using static Define;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;
using static GameSystem;

public class Data {
    [System.Serializable]
    public class TowerData {
        public TowerData() {
            for (int i = 0; i < (int)Define.TowerLevel.Count; i++) {
                TowerIconPath[(int)Define.TowerType.ArcherTower, i] = $"Sprites/Towers/ArcherTower/ArcherTower_Lvl{i + 1}";
            }

            for (int i = 0; i < (int)Define.TowerLevel.Count; i++) {
                TowerIconPath[(int)Define.TowerType.CanonTower, i] = $"Sprites/Towers/CanonTower/CanonTower_Lvl{i + 1}";
            }

            for (int i = 0; i < (int)Define.TowerLevel.Count; i++) {
                TowerIconPath[(int)Define.TowerType.MagicTower, i] = $"Sprites/Towers/MagicTower/MagicTower_Lvl{i + 1}";
            }

            for (int i = 0; i < (int)Define.TowerLevel.Count; i++) {
                TowerIconPath[(int)Define.TowerType.DeathTower, i] = $"Sprites/Towers/DeathTower/DeathTower_Lvl{i + 1}";
            }
        }
        public string[,] TowerIconPath = new string[(int)Define.TowerType.Count, (int)Define.TowerLevel.Count];

        public int[,] _towerCost = new int[,] {
        { 70,140,210,280},
        { 80,160,240,320 },
        { 75,150,230,300},
        { 100,170,240,310}
        };

        public int[,] _sellCost = new int[,] {
        { 35,75,110,150},
        { 40,85,125,175 },
        { 33,80,115,160},
        { 50,90,150,210}
    };

        public float[,] _towerCreateTime = new float[,] {
        { 3f, 5f, 7f, 9f },
        { 4f, 6f, 8f, 10f },
        { 3f, 5f, 7f, 9f},
        { 6f, 8f, 10f, 10f}
    };

        public int[,] _towerDamage = new int[,] {
        { 8,20,50,70},
        { 12,30,55,80 },
        { 15,40,65,90},
        { 7,18,40,65}
    };

        public float[,] _towerAttackDelay = new float[,] {
        { 1.2f, 1.1f, 1.0f, 1.0f },
        { 2.5f, 2.4f, 2.3f, 2.2f },
        { 1.6f, 1.5f, 1.4f, 1.4f },
        { 0.8f, 0.7f, 0.6f, 0.5f}
    };

        public float[,] _towerAttackRange = new float[,] {
        { 10f, 10f, 12f, 12f },
        { 6f, 6f, 8f, 8f },
        { 8f, 8f, 10f, 10f},
        { 10f, 10f, 12f, 12f },
    };
    }

    private Sprite[,] _towerIcon;

    [System.Serializable]
    public class EnemyData {
        public EnemyData() {
            _enemyCurrentHp = _enemyMaxHp;

            for (int i = 0; i < (int)Define.EnemyLevel.Count; i++) {
                EnemyIconPath[(int)Define.EnemyType.Archer, i] = $"Sprites/Units/Level{i + 1}/Archer_Level{i + 1}";
            }

            for (int i = 0; i < (int)Define.EnemyLevel.Count; i++) {
                EnemyIconPath[(int)Define.EnemyType.Swordman, i] = $"Sprites/Units/Level{i + 1}/Swordman_Level{i + 1}";
            }

            for (int i = 0; i < (int)Define.EnemyLevel.Count; i++) {
                EnemyIconPath[(int)Define.EnemyType.Mage, i] = $"Sprites/Units/Level{i + 1}/Mage_Level{i + 1}";
            }

            for (int i = 0; i < (int)Define.EnemyLevel.Count; i++) {
                EnemyIconPath[(int)Define.EnemyType.Speaman, i] = $"Sprites/Units/Level{i + 1}/Speaman_Level{i + 1}";
            }
        }

        public string[,] EnemyIconPath = new string[(int)Define.EnemyType.Count, (int)Define.EnemyLevel.Count];

        public int[,] _enemyMaxHp = new int[,] {
        {60,140,400,550,700,900 },
        {50,130,350,470,600,800 },
        {90,250,500,700,900,1100 },
        {100,300,600,900,1200,1400 }
    };

        public int[,] _enemyCurrentHp = new int[(int)Define.EnemyType.Count, (int)Define.EnemyLevel.Count];

        public float[,] _enemyMoveSpeed = new float[,] {
        {10f,11f,11f,12f,12f,13f },
        {12f,13f,13f,14f,15f,16f },
        {14f,15f,15f,16f,16f,17f },
        {14f,15f,15f,16f,16f,17f },
    };

        public int[,] _enemyPhysicsDefense = new int[,] {
        {1,2,3,4,5,6 },
        {0,0,1,1,2,2 },
        {5,7,7,9,9,10 },
        {6,8,8,10,10,12 }
    };

        public int[,] _enemyMagicDefense = new int[,] {
        {0,0,1,1,2,2 },
        {2,4,6,8,10,12 },
        {1,3,5,5,7,7 },
        {5,7,7,9,9,11 }
    };

        public int[,] _enemyProvideGold = new int[,] {
        {8,11,14,17,20,26 },
        {10,14,18,22,26,31 },
        {14,19,24,29,34,39 },
        {21,26,31,36,41,46 }
    };

        public int[,] _enemyProvideScore = new int[,] {
        {5,6,7,8,9,10 },
        {7,8,9,10,11,12 },
        {10,12,14,16,18,20 },
        {20,25,30,35,40,45 }
    };
    }

    private Sprite[,] _enemyIcon;

    [System.Serializable]
    public class OtherData {
        public string DefaultMaterialPath = "Material/Default";
        public string RedMaterialPath = "Material/Red";

        public string[] BgmPath = new string[] {
            $"Sound/BGM/{Define.BgmType.Main.ToString()}",
            $"Sound/BGM/{Define.BgmType.Ingame.ToString()}",
        };

        public string[] SfxPath = new string[] {
            $"Sound/SFX/{Define.SfxType.BeamProjectile.ToString()}",
            $"Sound/SFX/{Define.SfxType.CanonProjectile.ToString()}",
            $"Sound/SFX/{Define.SfxType.MagicProjectile.ToString()}",
            $"Sound/SFX/{Define.SfxType.Build.ToString()}",
            $"Sound/SFX/{Define.SfxType.BuildCompleted.ToString()}",
            $"Sound/SFX/{Define.SfxType.Demolition.ToString()}",
            $"Sound/SFX/{Define.SfxType.Victory.ToString()}",
            $"Sound/SFX/{Define.SfxType.Lose.ToString()}",
            $"Sound/SFX/{Define.SfxType.RoundStart.ToString()}",
            $"Sound/SFX/{Define.SfxType.EnemyArrive.ToString()}",
            $"Sound/SFX/{Define.SfxType.BtnSelect.ToString()}",
            $"Sound/SFX/{Define.SfxType.ObjectSelect.ToString()}",
        };
    }

    [System.Serializable]
    public class GameSystemData {
        public GameSystemData(int round, int gold, int hp, int score,  Dictionary<int, GameSystem.ConData> conDatas = null,
            Dictionary<int, GameSystem.TowerData> towerDatas = null) {
            CurrentRound = round;
            CurrentScore = score;
            CurrentGold = gold;
            CurrentHp = hp;
            ConData = conDatas;
            TowerData = towerDatas;
        }
        public int CurrentRound;
        public int CurrentGold;
        public int CurrentHp;
        public int CurrentScore;

        public Dictionary<int, GameSystem.ConData> ConData = new Dictionary<int, GameSystem.ConData>();
        public Dictionary<int, GameSystem.TowerData> TowerData = new Dictionary<int, GameSystem.TowerData>();
    }

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
    public class EnemySpawnDataDictionary {
        public Dictionary<int, List<EnemySpawnData>> _enemySpawnData = new Dictionary<int, List<EnemySpawnData>>();
    }

    public class LanguagePack {
        public Dictionary<Define.TextKey, Dictionary<Define.Language, string>> _languagePack = new Dictionary<Define.TextKey, Dictionary<Define.Language, string>>();
    }

   

    private TowerData _towerData;
    private EnemyData _enemyData;
    private OtherData _otherData;
    private EnemySpawnDataDictionary _enemySpawnData = new EnemySpawnDataDictionary();
    private LanguagePack _languageData = new LanguagePack();
    

    private Material _defaultMaterial;
    private Material _redMaterial;
    public Material DefaultMaterial => _defaultMaterial;
    public Material RedMaterial => _redMaterial; 
    
    public void Init() {
        #region EnemySpawnData

        EnemySpawnDataDictionary _enemySpawnDic = new EnemySpawnDataDictionary();

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(1)) {
            _enemySpawnDic._enemySpawnData.Add(1, new List<EnemySpawnData>
            {
                new EnemySpawnData(10, Define.EnemyType.Archer, Define.EnemyLevel.Level1),
                new EnemySpawnData(10, Define.EnemyType.Mage, Define.EnemyLevel.Level1)
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(2)) {
            _enemySpawnDic._enemySpawnData.Add(2, new List<EnemySpawnData>
            {
                new EnemySpawnData(10, Define.EnemyType.Archer, Define.EnemyLevel.Level1),
                new EnemySpawnData(10, Define.EnemyType.Mage, Define.EnemyLevel.Level1)
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(3)) {
            _enemySpawnDic._enemySpawnData.Add(3, new List<EnemySpawnData>
            {
                new EnemySpawnData(7, Define.EnemyType.Archer, Define.EnemyLevel.Level1),
                new EnemySpawnData(7, Define.EnemyType.Mage, Define.EnemyLevel.Level1),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level1),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(4)) {
            _enemySpawnDic._enemySpawnData.Add(4, new List<EnemySpawnData>
            {
                new EnemySpawnData(7, Define.EnemyType.Archer, Define.EnemyLevel.Level1),
                new EnemySpawnData(7, Define.EnemyType.Mage, Define.EnemyLevel.Level1),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level1),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(5)) {
            _enemySpawnDic._enemySpawnData.Add(5, new List<EnemySpawnData>
            {
                new EnemySpawnData(10, Define.EnemyType.Archer, Define.EnemyLevel.Level2),
                new EnemySpawnData(10, Define.EnemyType.Mage, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(6)) {
            _enemySpawnDic._enemySpawnData.Add(6, new List<EnemySpawnData>
            {
                 new EnemySpawnData(10, Define.EnemyType.Archer, Define.EnemyLevel.Level2),
                 new EnemySpawnData(10, Define.EnemyType.Mage, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(7)) {
            _enemySpawnDic._enemySpawnData.Add(7, new List<EnemySpawnData>
            {
                new EnemySpawnData(7, Define.EnemyType.Archer, Define.EnemyLevel.Level2),
                new EnemySpawnData(7, Define.EnemyType.Mage, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(8)) {
            _enemySpawnDic._enemySpawnData.Add(8, new List<EnemySpawnData>
            {
                new EnemySpawnData(7, Define.EnemyType.Archer, Define.EnemyLevel.Level2),
                new EnemySpawnData(7, Define.EnemyType.Mage, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(9)) {
            _enemySpawnDic._enemySpawnData.Add(9, new List<EnemySpawnData>
            {
                new EnemySpawnData(5, Define.EnemyType.Archer, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Mage, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Speaman, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(10)) {
            _enemySpawnDic._enemySpawnData.Add(10, new List<EnemySpawnData>
            {
                new EnemySpawnData(5, Define.EnemyType.Archer, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Mage, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Speaman, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(11)) {
            _enemySpawnDic._enemySpawnData.Add(11, new List<EnemySpawnData>
            {
                new EnemySpawnData(8, Define.EnemyType.Archer, Define.EnemyLevel.Level3),
                new EnemySpawnData(8, Define.EnemyType.Mage, Define.EnemyLevel.Level3),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level2),
                new EnemySpawnData(5, Define.EnemyType.Speaman, Define.EnemyLevel.Level2),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(12)) {
            _enemySpawnDic._enemySpawnData.Add(12, new List<EnemySpawnData>
            {
                new EnemySpawnData(8, Define.EnemyType.Archer, Define.EnemyLevel.Level3),
                new EnemySpawnData(8, Define.EnemyType.Mage, Define.EnemyLevel.Level3),
                new EnemySpawnData(5, Define.EnemyType.Swordman, Define.EnemyLevel.Level3),
                new EnemySpawnData(5, Define.EnemyType.Speaman, Define.EnemyLevel.Level3),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(13)) {
            _enemySpawnDic._enemySpawnData.Add(13, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Swordman, Define.EnemyLevel.Level3),
                new EnemySpawnData(15, Define.EnemyType.Speaman, Define.EnemyLevel.Level3),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(14)) {
            _enemySpawnDic._enemySpawnData.Add(14, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level3),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level3),
                new EnemySpawnData(15, Define.EnemyType.Swordman, Define.EnemyLevel.Level3),
                new EnemySpawnData(15, Define.EnemyType.Speaman, Define.EnemyLevel.Level3),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(15)) {
            _enemySpawnDic._enemySpawnData.Add(15, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level4),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level4),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(16)) {
            _enemySpawnDic._enemySpawnData.Add(16, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level4),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level4),
                new EnemySpawnData(15, Define.EnemyType.Swordman, Define.EnemyLevel.Level4),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(17)) {
            _enemySpawnDic._enemySpawnData.Add(17, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level4),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level4),
                new EnemySpawnData(15, Define.EnemyType.Swordman, Define.EnemyLevel.Level4),
                new EnemySpawnData(15, Define.EnemyType.Speaman, Define.EnemyLevel.Level4),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(18)) {
            _enemySpawnDic._enemySpawnData.Add(18, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level5),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level5),
                new EnemySpawnData(10, Define.EnemyType.Swordman, Define.EnemyLevel.Level5),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(19)) {
            _enemySpawnDic._enemySpawnData.Add(19, new List<EnemySpawnData>
            {
                new EnemySpawnData(10, Define.EnemyType.Archer, Define.EnemyLevel.Level5),
                new EnemySpawnData(10, Define.EnemyType.Mage, Define.EnemyLevel.Level5),
                new EnemySpawnData(10, Define.EnemyType.Swordman, Define.EnemyLevel.Level5),
                new EnemySpawnData(10, Define.EnemyType.Speaman, Define.EnemyLevel.Level5),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(20)) {
            _enemySpawnDic._enemySpawnData.Add(20, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level5),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level5),
                new EnemySpawnData(15, Define.EnemyType.Swordman, Define.EnemyLevel.Level5),
                new EnemySpawnData(15, Define.EnemyType.Speaman, Define.EnemyLevel.Level5),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(21)) {
            _enemySpawnDic._enemySpawnData.Add(21, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level6),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level6),
                new EnemySpawnData(10, Define.EnemyType.Swordman, Define.EnemyLevel.Level6),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(22)) {
            _enemySpawnDic._enemySpawnData.Add(22, new List<EnemySpawnData>
            {
                new EnemySpawnData(10, Define.EnemyType.Archer, Define.EnemyLevel.Level6),
                new EnemySpawnData(10, Define.EnemyType.Mage, Define.EnemyLevel.Level6),
                new EnemySpawnData(10, Define.EnemyType.Swordman, Define.EnemyLevel.Level6),
                new EnemySpawnData(10, Define.EnemyType.Speaman, Define.EnemyLevel.Level6),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(23)) {
            _enemySpawnDic._enemySpawnData.Add(23, new List<EnemySpawnData>
            {
                new EnemySpawnData(15, Define.EnemyType.Archer, Define.EnemyLevel.Level6),
                new EnemySpawnData(15, Define.EnemyType.Mage, Define.EnemyLevel.Level6),
                new EnemySpawnData(15, Define.EnemyType.Swordman, Define.EnemyLevel.Level6),
                new EnemySpawnData(15, Define.EnemyType.Speaman, Define.EnemyLevel.Level6),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(24)) {
            _enemySpawnDic._enemySpawnData.Add(24, new List<EnemySpawnData>
            {
                new EnemySpawnData(20, Define.EnemyType.Archer, Define.EnemyLevel.Level6),
                new EnemySpawnData(20, Define.EnemyType.Mage, Define.EnemyLevel.Level6),
                new EnemySpawnData(20, Define.EnemyType.Swordman, Define.EnemyLevel.Level6),
                new EnemySpawnData(20, Define.EnemyType.Speaman, Define.EnemyLevel.Level6),
            });
        }

        if (!_enemySpawnDic._enemySpawnData.ContainsKey(25)) {
            _enemySpawnDic._enemySpawnData.Add(25, new List<EnemySpawnData>
            {
                new EnemySpawnData(25, Define.EnemyType.Archer, Define.EnemyLevel.Level6),
                new EnemySpawnData(25, Define.EnemyType.Mage, Define.EnemyLevel.Level6),
                new EnemySpawnData(25, Define.EnemyType.Swordman, Define.EnemyLevel.Level6),
                new EnemySpawnData(25, Define.EnemyType.Speaman, Define.EnemyLevel.Level6),
            });
        }

        #endregion


        #region LanguageData

        LanguagePack _pack = new LanguagePack();
        AddLanguageData(_pack, Define.TextKey.GameStart, Define.Language.Korean, "���� ����");
        AddLanguageData(_pack, Define.TextKey.Continue, Define.Language.Korean, "�̾��ϱ�");
        AddLanguageData(_pack, Define.TextKey.Setting, Define.Language.Korean, "���� ����");
        AddLanguageData(_pack, Define.TextKey.GameExit, Define.Language.Korean, "���� ����");
        AddLanguageData(_pack, Define.TextKey.LanguageSetting, Define.Language.Korean, "��� ����");
        AddLanguageData(_pack, Define.TextKey.BgmVolumeSetting, Define.Language.Korean, "����� ����");
        AddLanguageData(_pack, Define.TextKey.SfxVolumeSetting, Define.Language.Korean, "ȿ���� ����");
        AddLanguageData(_pack, Define.TextKey.SensitivitySetting, Define.Language.Korean, "���� ����");
        AddLanguageData(_pack, Define.TextKey.ToNextRound, Define.Language.Korean, "���� �������");
        AddLanguageData(_pack, Define.TextKey.StartNextRound, Define.Language.Korean, "���� ����");
        AddLanguageData(_pack, Define.TextKey.Wave, Define.Language.Korean, "���̺�");
        AddLanguageData(_pack, Define.TextKey.Score, Define.Language.Korean, "����");
        AddLanguageData(_pack, Define.TextKey.Build, Define.Language.Korean, "�Ǽ�");
        AddLanguageData(_pack, Define.TextKey.Demolition, Define.Language.Korean, "�Ǹ�");
        AddLanguageData(_pack, Define.TextKey.ArcherTowerDescription, Define.Language.Korean, "���� ���ݼӵ��� �� ��Ÿ��� ���� �������� Ÿ���Դϴ�.");
        AddLanguageData(_pack, Define.TextKey.CanonTowerDescription, Define.Language.Korean, "���������� ���� ���ÿ� �����ϴ� �������� Ÿ���Դϴ�.");
        AddLanguageData(_pack, Define.TextKey.MagicTowerDescription, Define.Language.Korean, "������ ���ݷ��� ���� �������� Ÿ���Դϴ�.");
        AddLanguageData(_pack, Define.TextKey.DeathTowerDescription, Define.Language.Korean, "���� ���ݼӵ��� ���� ���� ���Ÿ���� �����ϴ� Ÿ���Դϴ�.");
        AddLanguageData(_pack, Define.TextKey.HighRound, Define.Language.Korean, "�ְ� ����");
        AddLanguageData(_pack, Define.TextKey.HighScore, Define.Language.Korean, "�ְ� ����");
        AddLanguageData(_pack, Define.TextKey.Restart, Define.Language.Korean, "�����");
        AddLanguageData(_pack, Define.TextKey.Main, Define.Language.Korean, "���� ȭ��");
        AddLanguageData(_pack, Define.TextKey.Victory, Define.Language.Korean, "�¸�!!");
        AddLanguageData(_pack, Define.TextKey.GameOver, Define.Language.Korean, "�й�...");


        AddLanguageData(_pack, Define.TextKey.GameStart, Define.Language.English, "GameStart");
        AddLanguageData(_pack, Define.TextKey.Continue, Define.Language.English, "Continue");
        AddLanguageData(_pack, Define.TextKey.Setting, Define.Language.English, "GameSetting");
        AddLanguageData(_pack, Define.TextKey.GameExit, Define.Language.English, "ExitGame");
        AddLanguageData(_pack, Define.TextKey.LanguageSetting, Define.Language.English, "Language Setting");
        AddLanguageData(_pack, Define.TextKey.BgmVolumeSetting, Define.Language.English, "Bgm Setting");
        AddLanguageData(_pack, Define.TextKey.SfxVolumeSetting, Define.Language.English, "Sfx Setting");
        AddLanguageData(_pack, Define.TextKey.SensitivitySetting, Define.Language.English, "Sensitivity Setting");
        AddLanguageData(_pack, Define.TextKey.ToNextRound, Define.Language.English, "ToNextRound");
        AddLanguageData(_pack, Define.TextKey.StartNextRound, Define.Language.English, "Start");
        AddLanguageData(_pack, Define.TextKey.Wave, Define.Language.English, "Wave");
        AddLanguageData(_pack, Define.TextKey.Score, Define.Language.English, "Score");
        AddLanguageData(_pack, Define.TextKey.Build, Define.Language.English, "Build");
        AddLanguageData(_pack, Define.TextKey.Demolition, Define.Language.English, "Sell");
        AddLanguageData(_pack, Define.TextKey.ArcherTowerDescription, Define.Language.English, "It is a physical attack tower with fast attack speed and long range.");
        AddLanguageData(_pack, Define.TextKey.CanonTowerDescription, Define.Language.English, "It is a physical attack tower that simultaneously attacks a wide range of enemies.");
        AddLanguageData(_pack, Define.TextKey.MagicTowerDescription, Define.Language.English, "It is a magic attack tower with strong attack power.");
        AddLanguageData(_pack, Define.TextKey.DeathTowerDescription, Define.Language.English, "It is a tower that ignores the enemy's defensive type with fast attack speed.");
        AddLanguageData(_pack, Define.TextKey.HighRound, Define.Language.English, "High Round");
        AddLanguageData(_pack, Define.TextKey.HighScore, Define.Language.English, "High Score");
        AddLanguageData(_pack, Define.TextKey.Restart, Define.Language.English, "Restart");
        AddLanguageData(_pack, Define.TextKey.Main, Define.Language.English, "Main");
        AddLanguageData(_pack, Define.TextKey.Victory, Define.Language.English, "Victory!!");
        AddLanguageData(_pack, Define.TextKey.GameOver, Define.Language.English, "Gameover..");
        #endregion

        #region SaveJson
        TowerData towerData = new TowerData();
        string data = JsonConvert.SerializeObject(towerData, Formatting.Indented);
        SaveJson(Application.dataPath, "TowerData", data);

        EnemyData enemyData = new EnemyData();
        string enemydata = JsonConvert.SerializeObject(enemyData, Formatting.Indented);
        SaveJson(Application.dataPath, "EnemyData", enemydata);

        OtherData otherData = new OtherData();
        string otherdata = JsonConvert.SerializeObject(otherData, Formatting.Indented);
        SaveJson(Application.dataPath, "OtherData", otherdata);
        
        string spawnData = JsonConvert.SerializeObject(_enemySpawnDic._enemySpawnData, Formatting.Indented);
        SaveJson(Application.dataPath, "SpawnData", spawnData);

        string languageData = JsonConvert.SerializeObject(_pack._languagePack, Formatting.Indented,
            new JsonSerializerSettings {
                Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter()}
            });
        SaveJson(Application.dataPath, "LanguageData", languageData);
        #endregion

        #region LoadJson
        _towerData = LoadJson<TowerData>(Application.dataPath, "TowerData");
        _enemyData = LoadJson<EnemyData>(Application.dataPath, "EnemyData");
        _otherData = LoadJson<OtherData>(Application.dataPath, "OtherData");
        _enemySpawnData._enemySpawnData = LoadJson<Dictionary<int, List<EnemySpawnData>>>(Application.dataPath, "SpawnData");
        _languageData._languagePack = LoadJson<Dictionary<Define.TextKey, Dictionary<Define.Language, string>>>(Application.dataPath, "LanguageData");
        #endregion

        _defaultMaterial = Resources.Load<Material>(_otherData.DefaultMaterialPath);
        _redMaterial = Resources.Load<Material>(_otherData.RedMaterialPath);
        _towerIcon = new Sprite[(int)Define.TowerType.Count, (int)Define.TowerLevel.Count];
        _enemyIcon = new Sprite[(int)Define.EnemyType.Count, (int)Define.EnemyLevel.Count];

        #region IconSpriteInit

        for (int i = 0;i < (int)Define.TowerType.Count; i++) {
            for(int j = 0;j < (int)Define.TowerLevel.Count; j++) {
                _towerIcon[i, j] = (Sprite)Resources.Load<Sprite>(_towerData.TowerIconPath[i,j]);
            }
        }

        for (int i = 0; i < (int)Define.EnemyType.Count; i++) {
            for (int j = 0; j < (int)Define.EnemyLevel.Count; j++) {
                _enemyIcon[i, j] = (Sprite)Resources.Load<Sprite>(_enemyData.EnemyIconPath[i, j]);
            }
        }

        //for (int i = 0; i< (int)Define.TowerLevel.Count; i++) {
        //    _towerIcon[(int)Define.TowerType.ArcherTower, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Towers/ArcherTower/ArcherTower_Lvl{i + 1}");
        //}

        //for (int i = 0; i < (int)Define.TowerLevel.Count; i++) {
        //    _towerIcon[(int)Define.TowerType.CanonTower, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Towers/CanonTower/CanonTower_Lvl{i + 1}");
        //}

        //for (int i = 0; i < (int)Define.TowerLevel.Count; i++) {
        //    _towerIcon[(int)Define.TowerType.MagicTower, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Towers/MagicTower/MagicTower_Lvl{i + 1}");
        //}

        //for (int i = 0; i < (int)Define.TowerLevel.Count; i++) {
        //    _towerIcon[(int)Define.TowerType.DeathTower, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Towers/DeathTower/DeathTower_Lvl{i + 1}");
        //}

        //for(int i = 0; i< (int)Define.EnemyLevel.Count; i++) {
        //    _enemyIcon[(int)Define.EnemyType.Archer, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Units/Level{i + 1}/Archer_Level{i + 1}");
        //}

        //for (int i = 0; i < (int)Define.EnemyLevel.Count; i++) {
        //    _enemyIcon[(int)Define.EnemyType.Swordman, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Units/Level{i + 1}/Swordman_Level{i + 1}");
        //}

        //for (int i = 0; i < (int)Define.EnemyLevel.Count; i++) {
        //    _enemyIcon[(int)Define.EnemyType.Mage, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Units/Level{i + 1}/Mage_Level{i + 1}");
        //}

        //for (int i = 0; i < (int)Define.EnemyLevel.Count; i++) {
        //    _enemyIcon[(int)Define.EnemyType.Speaman, i] = (Sprite)Resources.Load<Sprite>($"Sprites/Units/Level{i + 1}/Speaman_Level{i + 1}");
        //}
        #endregion
    }

    private void AddLanguageData(LanguagePack pack, Define.TextKey key, Define.Language language, string text) {
        if(!pack._languagePack.ContainsKey(key)) {
            pack._languagePack[key] = new Dictionary<Define.Language, string>();
        }
        pack._languagePack[key][language] = text;
    }

    public GameSystemData GetSaveData() {
        GameSystemData data = LoadJson<GameSystemData>(Application.dataPath, "SaveData");
        if(data == default) {
            return null;
        }
        return data;
    }
    public void SaveJson(string path, string name, string jsonData) {
        string Path = string.Format("{0}/{1}.json", path, name);
        //if(File.Exists(Path)) {
        //    Debug.Log("�̹� ������ �����մϴ�");
        //    return;
        //}
        FileStream stream = new FileStream(Path, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        stream.Write(data, 0, data.Length);
        stream.Close();
        Debug.Log($"{Path}�� {name}�̸��� Json���� ���̺�");
    }
    public T LoadJson<T>(string path, string name) {
        string Path = string.Format("{0}/{1}.json", path, name);
        if (!File.Exists(Path)) {
            Debug.Log($"Load����. {path}�� {name}�̸� ������ �����ϴ�.");
            return default;
        }
        FileStream stream = new FileStream(string.Format("{0}/{1}.json", path, name), FileMode.Open);
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        Debug.Log($"{path}���� {name}�̸��� Json���� �ε�");
        return JsonConvert.DeserializeObject<T>(jsonData);

    }
    public List<EnemySpawnData> GetEnemySpawnData(int level) {
        return _enemySpawnData._enemySpawnData[level];
    }

    public string GetLanguage(int key, int language) => _languageData._languagePack[(Define.TextKey)key][(Define.Language)language];

    public string GetBgmPath(int type) => _otherData.BgmPath[type];
    public string GetSfxPath(int type) => _otherData.SfxPath[type];
    public int GetTowerCost(int type, int level) => _towerData._towerCost[type, level - 1];
    public int GetTowerAttackDamage(int type, int level) => _towerData._towerDamage[type, level - 1];
    public float GetTowerAttacmRange(int type, int level) => _towerData._towerAttackRange[type, level - 1];
    public float GetTowerAttacnDelay(int type, int level) => _towerData._towerAttackDelay[type, level - 1];
    public int GetSellCost(int type, int level) => _towerData._sellCost[type, level - 1];
    public float GetTowerCreateTime(int type, int level) => _towerData._towerCreateTime[type, level - 1];
    public int GetEnemyMaxHp(int type, int level) => _enemyData._enemyMaxHp[type, level - 1];
    public float GetEnemyMoveSpeed(int type, int level) => _enemyData._enemyMoveSpeed[type, level - 1];
    public int GetEnemyPhysicsDefense(int type, int level) => _enemyData._enemyPhysicsDefense[type, level - 1];
    public int GetEnemyMagicDefense(int type, int level) => _enemyData._enemyMagicDefense[type, level - 1];
    public int GetEnemyProvideGold(int type, int level) => _enemyData._enemyProvideGold[type, level - 1];
    public int GetEnemyProvideScore(int type, int level) => _enemyData._enemyProvideScore[type, level - 1];

    public Sprite GetTowerIcon(int type, int level) => _towerIcon[type, level - 1];
    public Sprite GetEnemyIcon(int type, int level) => _enemyIcon[type, level - 1];
}
