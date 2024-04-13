using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    #region Enum
    public enum ObjectType {
        Enemy,
        Tower,
        Con,
    }
    public enum GameState {
        Play,
        GameOver,
    }
    public enum CursorType {
        DefaultCursor,
        NodeCursor,
        ButtonCursor,
        EnemyCursor,
        Count,
    }
    public enum EnemyState {
        Idle,
        Move,
        Die,
    }  
    
    public enum TowerState {
        Idle,
        Attack,
    }
    public enum TowerType {
        ArcherTower,
        CanonTower,
        MagicTower,
        DeathTower,
        Count,
    } 
    
    public enum EnemyType {
        Archer,
        Mage,
        Swordman,
        Speaman,
        Count,
    }

    public enum TowerLevel { 
        Level1,
        Level2,
        Level3,
        Level4,
        Count,
    }

    public enum EnemyLevel {
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Count,
    }

    public enum MouseEventType
    {
        None,
        LeftMouseDown,
        RightMouseDown,
        LeftMouseUp,
        RightMouseUp,
        LeftMouse,
        RightMouse,
        Enter,
        Drag,
        Exit,
    }

    public enum SceneType
    {
        None,
        Loading,
        Main,
        InGame,
        Exit,
    }

    public enum SfxType {
        BeamProjectile,
        CanonProjectile,
        MagicProjectile,
        Build,
        BuildCompleted,
        Demolition,
        Victory,
        Lose,
        RoundStart,
        EnemyArrive,
        BtnSelect,
        ObjectSelect,
        Count
    }
    public enum BgmType {
        Main,
        Ingame,
        Count,
    }

    public enum Language {
        Korean,
        English,
        Count,
    }

    public enum TextKey {
        GameStart,
        Continue,
        Setting,
        GameExit,
        LanguageSetting,
        BgmVolumeSetting,
        SfxVolumeSetting,
        SensitivitySetting,
        ToNextRound,
        StartNextRound,
        Wave,
        Score,
        Build,
        Demolition,
        ArcherTowerDescription,
        CanonTowerDescription,
        MagicTowerDescription,
        DeathTowerDescription,
        HighRound,
        HighScore,
        Restart,
        Main,
        Victory,
        GameOver,
    }
    #endregion
    #region string
    public const string MOVE_POINT = "MovePoints";
    public const string SENSITYVITY = "Sensitivity";
    public const string TAG_ENEMY = "Enemy";
    public const string FIREPOINT = "FirePoint";
    public const string LOADING = "Loading...";
    public const string LOADING_COMPLETE = "Completed!!";
    #endregion
}
