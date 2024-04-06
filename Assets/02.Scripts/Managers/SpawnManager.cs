using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{
    private const float _spawnDelay = 1.5f;
    private Transform _movePoints;
    private Data _data;
    private Transform _spawnPoint;
    private Transform _arrivalPoint;
  
    public void Init() {
        _movePoints = GameObject.Find("MovePoints").transform;

        _spawnPoint = _movePoints.GetChild(0).transform;
        _arrivalPoint = _movePoints.GetChild(_movePoints.childCount -1).transform;

        _data = Managers.Data;
    }

   
    public void SpawnEnemy(int currentGameLevel) {
        GameSystem.Instance.StartCoroutine(CoSpwan(currentGameLevel));
    }


    IEnumerator CoSpwan(int currentGameLevel) {

        List<Data.EnemySpawnData> spawnData = _data.GetEnemySpawnData(currentGameLevel);

        foreach(var data in spawnData) {
            for(int i = 0; i<data.Count; i++) {
                GameObject go = Managers.Resources.Instantiate($"Enemy/{data.EnemyLevel.ToString()}/{data.EnemyType.ToString()}_{data.EnemyLevel.ToString()}", null);
                go.transform.position = _spawnPoint.position;
                yield return new WaitForSeconds(_spawnDelay);
            }
        }
    }

}
