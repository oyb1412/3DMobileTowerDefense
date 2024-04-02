using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data {
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
        { 5f, 10f, 15f, 20f },
        { 6f,12f,18f,24f },
        { 7f,14f,21f,28f},
        { 10f,20f,30f,40f}
    };

    private int[,] _towerDamage = new int[,] {
        { 3,6,9,12},
        { 4,8,12,16 },
        { 5,15,20,25},
        { 6,12,18,24}
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



    private static Material _defaultMaterial;
    private static Material _greenMaterial;
    public Material DefaultMaterial => _defaultMaterial;
    public Material GreenMaterial => _greenMaterial;

    public void Init() {
        _defaultMaterial = (Material)Resources.Load<Material>("Material/Default");
        _greenMaterial = (Material)Resources.Load<Material>("Material/Green");
    }
    public int GetTowerCost(int level, int type) => _towerCost[type, level - 1];
    public int GetTowerAttackDamage(int level, int type) => _towerDamage[type, level - 1];
    public float GetTowerAttacmRange(int level, int type) => _towerAttackRange[type, level - 1];
    public float GetTowerAttacnDelay(int level, int type) => _towerAttackDelay[type, level - 1];
    public int GetSellCost(int level, int type) => _sellCost[type, level - 1];
    public float GetTowerCreateTime(int level, int type) => _towerCreateTime[type, level - 1];
}
