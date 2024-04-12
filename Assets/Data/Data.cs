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
        
        public string[,] TowerIconPath;

        public int[,] _towerCost;

        public int[,] _sellCost;

        public float[,] _towerCreateTime;

        public int[,] _towerDamage;

        public float[,] _towerAttackDelay;

        public float[,] _towerAttackRange;
    }

    private Sprite[,] _towerIcon;

    [System.Serializable]
    public class EnemyData {


        public string[,] EnemyIconPath;

        public int[,] _enemyMaxHp;

        public int[,] _enemyCurrentHp;

        public float[,] _enemyMoveSpeed;

        public int[,] _enemyPhysicsDefense;

        public int[,] _enemyMagicDefense;

        public int[,] _enemyProvideGold;

        public int[,] _enemyProvideScore;
    }

    private Sprite[,] _enemyIcon;

    [System.Serializable]
    public class OtherData {
        public string DefaultMaterialPath ;
        public string RedMaterialPath;

        public string[] BgmPath;

        public string[] SfxPath;
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
        public Dictionary<int, List<EnemySpawnData>> _enemySpawnData;
    }

    public class LanguagePack {
        public Dictionary<Define.TextKey, Dictionary<Define.Language, string>> _languagePack;
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
        #region LoadJson
        _towerData = LoadJson<TowerData>(Application.persistentDataPath, "TowerData");
        _enemyData = LoadJson<EnemyData>(Application.persistentDataPath, "EnemyData");
        _otherData = LoadJson<OtherData>(Application.persistentDataPath, "OtherData");
        _enemySpawnData._enemySpawnData = LoadJson<Dictionary<int, List<EnemySpawnData>>>(Application.persistentDataPath, "SpawnData");
        _languageData._languagePack = LoadJson<Dictionary<Define.TextKey, Dictionary<Define.Language, string>>>(Application.persistentDataPath, "LanguageData");
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

        
        #endregion
    }

    public GameSystemData GetSaveData() {
        GameSystemData data = LoadJson<GameSystemData>(Application.persistentDataPath, "SaveData");
        if(data == default) {
            return null;
        }
        return data;
    }

    public void SaveJson(string path, string name, string jsonData) {
        string Path = string.Format("{0}/{1}.json", path, name);
        FileStream stream = new FileStream(Path, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        stream.Write(data, 0, data.Length);
        stream.Close();
        Debug.Log($"{Path}에 {name}이름의 Json파일 세이브");
    }
    public T LoadJson<T>(string path, string name) {
        string Path = string.Format("{0}/{1}.json", path, name);
        if (!File.Exists(Path)) {
            Debug.Log($"Load실패. {path}에 {name}이름 파일이 없습니다.");
            return default;
        }
        FileStream stream = new FileStream(string.Format("{0}/{1}.json", path, name), FileMode.Open);
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        Debug.Log($"{path}에서 {name}이름의 Json파일 로드");
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
