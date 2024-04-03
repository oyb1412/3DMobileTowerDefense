using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
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
    }

    public enum SceneType
    {
        None,
        InGame,
    }
}
