using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum TowerType {
        ArcherTower,
        CanonTower,
        MagicTower,
        DeathTower,
        Count,
    }

    public enum TowerLevel { 
        Level1,
        Level2,
        Level3,
        Level4,
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
