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
    private int _enemyNumber;
    //TODO
    //게임 재시작 시, 적절한 타이밍에 초기화가 필요함.
    //재시작 시 이 번호가 중복될 가능성이 있음
  
    public void Init() {
        _movePoints = GameObject.Find("MovePoints").transform;

        _spawnPoint = _movePoints.GetChild(0).transform;

        _data = Managers.Data;
    }

    public void Clear() {

    }

   
    public void SpawnEnemy(int currentGameLevel, ParticleSystem effect) {
        GameSystem.Instance.SaveGameData();
        Managers.Audio.PlaySfx(Define.SfxType.RoundStart);
        GameSystem.Instance.StartCoroutine(CoSpwan(currentGameLevel, effect));
    }


    IEnumerator CoSpwan(int currentGameLevel, ParticleSystem effect) {
        List<Data.EnemySpawnData> spawnData = _data.GetEnemySpawnData(currentGameLevel);
        effect.Play();

        foreach (var data in spawnData) {
            for(int i = 0; i<data.Count; i++) {
                if (!GameSystem.Instance.IsPlay())
                    GameSystem.Instance.StopAllCoroutines();

                _enemyNumber++;
                EnemyController enemy = Managers.Resources.Instantiate($"Enemy/{data.EnemyLevel.ToString()}/{data.EnemyType.ToString()}_{data.EnemyLevel.ToString()}", null)
                    .GetComponent<EnemyController>();
                enemy.Init(_spawnPoint.position, _enemyNumber);
                yield return new WaitForSeconds(_spawnDelay);
            }
        }
        effect.Stop();
    }

}
