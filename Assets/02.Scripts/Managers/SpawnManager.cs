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

   
    public void SpawnEnemy(int currentGameLevel, ParticleSystem effect) {
        GameSystem.Instance.StartCoroutine(CoSpwan(currentGameLevel, effect));
    }


    IEnumerator CoSpwan(int currentGameLevel, ParticleSystem effect) {
        List<Data.EnemySpawnData> spawnData = _data.GetEnemySpawnData(currentGameLevel);
        effect.Play();

        foreach (var data in spawnData) {
            for(int i = 0; i<data.Count; i++) {
                if (!GameSystem.Instance.IsPlay())
                    GameSystem.Instance.StopAllCoroutines();

                GameObject go = Managers.Resources.Instantiate($"Enemy/{data.EnemyLevel.ToString()}/{data.EnemyType.ToString()}_{data.EnemyLevel.ToString()}", null);
                go.transform.position = _spawnPoint.position;
                yield return new WaitForSeconds(_spawnDelay);
            }
        }
        effect.Stop();
    }

}
