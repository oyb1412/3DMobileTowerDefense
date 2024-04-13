using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애너미 스폰 관리
/// </summary>
public class SpawnManager
{
    private const float SPAWN_DELAY = 1f;  //각 스폰 사이 딜레이
    private int _enemyNumber;  //애너미 생성 순서

    private Transform _movePoints;  //애너미 이동 위치
    private Transform _spawnPoint;  //애너미 생성 위치

    private Data _data;

    private string[,] _enemyPath = new string[(int)Define.EnemyType.Count, (int)Define.EnemyLevel.Count];
    private GameObject[,] _enemyObject = new GameObject[(int)Define.EnemyType.Count, (int)Define.EnemyLevel.Count];


    /// <summary>
    /// 초기화
    /// </summary>
    public void Init() {
        _movePoints = GameObject.Find("MovePoints").transform;
        _spawnPoint = _movePoints.GetChild(0).transform;
        _data = Managers.Data;

        for(int j = 0; j < _enemyPath.GetLength(1); j++) {
            _enemyPath[(int)Define.EnemyType.Archer, j] = $"Prefabs/Enemy/Level{j + 1}/Archer_Level{j + 1}";
            _enemyObject[(int)Define.EnemyType.Archer, j] = (GameObject)Managers.Resources.Load<GameObject>(_enemyPath[(int)Define.EnemyType.Archer, j]);
        }

        for (int j = 0; j < _enemyPath.GetLength(1); j++) {
            _enemyPath[(int)Define.EnemyType.Mage, j] = $"Prefabs/Enemy/Level{j + 1}/Mage_Level{j + 1}";
            _enemyObject[(int)Define.EnemyType.Mage, j] = (GameObject)Managers.Resources.Load<GameObject>(_enemyPath[(int)Define.EnemyType.Mage, j]);
        }

        for (int j = 0; j < _enemyPath.GetLength(1); j++) {
            _enemyPath[(int)Define.EnemyType.Swordman, j] = $"Prefabs/Enemy/Level{j + 1}/Swordman_Level{j + 1}";
            _enemyObject[(int)Define.EnemyType.Swordman, j] = (GameObject)Managers.Resources.Load<GameObject>(_enemyPath[(int)Define.EnemyType.Swordman, j]);
        }

        for (int j = 0; j < _enemyPath.GetLength(1); j++) {
            _enemyPath[(int)Define.EnemyType.Speaman, j] = $"Prefabs/Enemy/Level{j + 1}/Speaman_Level{j + 1}";
            _enemyObject[(int)Define.EnemyType.Speaman, j] = (GameObject)Managers.Resources.Load<GameObject>(_enemyPath[(int)Define.EnemyType.Speaman, j]);
        }
    }

    /// <summary>
    /// 애너미 소환
    /// </summary>
    /// <param name="currentGameLevel">현재 게임 레벨</param>
    /// <param name="effect">소환 이펙트</param>
    public void SpawnEnemy(int currentGameLevel, ParticleSystem effect) {
        GameSystem.Instance.SaveGameData();
        Managers.Audio.PlaySfx(Define.SfxType.RoundStart);
        GameSystem.Instance.StartCoroutine(CoSpwan(currentGameLevel, effect));
    }

    /// <summary>
    /// 애너미 소환 코루틴
    /// </summary>
    /// <param name="currentGameLevel">현재 게임 레벨</param>
    /// <param name="effect">소환 이펙트</param>
    /// <returns></returns>
    IEnumerator CoSpwan(int currentGameLevel, ParticleSystem effect) {
        List<Data.EnemySpawnData> spawnData = _data.GetEnemySpawnData(currentGameLevel);  //현재 레벨에 맞는 데이터 추출
        effect.Play();

        foreach (var data in spawnData) {
            for(int i = 0; i<data.Count; i++) {
                if (!GameSystem.Instance.IsPlay())  //게임 종료시 정지
                    GameSystem.Instance.StopAllCoroutines();

                _enemyNumber++;

                int level = (int)data.EnemyLevel;
                int type = (int)data.EnemyType;
                EnemyController enemy = Managers.Resources.Instantiate(_enemyObject[type, level], null).GetComponent<EnemyController>();
                //EnemyController enemy = Managers.Resources.Instantiate($"Enemy/{data.EnemyLevel.ToString()}/{data.EnemyType.ToString()}_{data.EnemyLevel.ToString()}", null)
                //    .GetComponent<EnemyController>();
                enemy.Init(_spawnPoint.position, _enemyNumber);
                yield return new WaitForSeconds(SPAWN_DELAY);
            }
        }
        effect.Stop();
    }
}
