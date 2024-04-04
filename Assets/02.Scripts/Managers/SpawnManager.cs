using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{
    private const float _spawnDelay = 1f;
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
        int spawnLevel = currentGameLevel % 3;

        Tuple<int, Define.EnemyType> spawnData = _data.GetSpawnData(spawnLevel);
        int maxSpawnCount = spawnData.Item1;
        int currentCount = 0;
        Define.EnemyType type = spawnData.Item2;

        while (true) {
            currentCount++;

            GameObject go = Managers.Resources.Instantiate($"Enemy/Level{spawnLevel}/{type.ToString()}_Level{spawnLevel}", null);
            go.transform.position = _spawnPoint.position;
            if (currentCount >= maxSpawnCount) {
                break;
            }
            yield return new WaitForSeconds(_spawnDelay);

        }
    }

}
