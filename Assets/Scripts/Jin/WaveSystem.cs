using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo _playerInfo;
    [SerializeField]
    private GameObject _winPanel;
    [SerializeField]
    private Stage[] stages;                   // 현재 스테이지의 모든 웨이브 정보
    [SerializeField]
    private EnemySpawner enemySpawner;
    private int currentWaveIndex = -1;
    private Stage _currentStage;

    private void Start()
    {
        _currentStage = stages[_playerInfo.current_stage - 1];
        StartCoroutine(StartWaveCoroutine());
    }

    private void Update()
    {
        if (currentWaveIndex == _currentStage.waves.Length && enemySpawner.EnemyList.Count == 0)
        {
            StartCoroutine(WaitAndClearCoroutine());
        }
    }
    // 처음 5초 대기 후, 웨이브 시작
    // 웨이브는 15초마다 시작
    // 최대 웨이브이면서 Enemy의 수가 0일 때 Clear 출력
    private IEnumerator StartWaveCoroutine()
    {
        yield return new WaitForSeconds(5f);

        while (currentWaveIndex < _currentStage.waves.Length)
        {
            StartWave();
            yield return new WaitForSeconds(15f); // 15초 대기
        }
    }
    public void StartWave()
    {
        // 인덱스의 시작이 -1이기 때문에 웨이브 인덱스 증가를 제일 먼저 함
        currentWaveIndex++;
        // EnemySpawner의 StartWave() 함수 호출. 현재 웨이브 정보 제공
        if (currentWaveIndex < _currentStage.waves.Length)
        {
            enemySpawner.StartWave(_currentStage.waves[currentWaveIndex]);
        }
    }
    private IEnumerator WaitAndClearCoroutine()
    {
        yield return new WaitForSeconds(3f);
        StageClear();
    }
    public void StageClear()
    {
        _winPanel.SetActive(true);
        Debug.Log("Clear");
    }
}

[System.Serializable]
public struct Wave
{
    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
}

[System.Serializable]
public struct Stage
{
    public Wave[] waves;
}