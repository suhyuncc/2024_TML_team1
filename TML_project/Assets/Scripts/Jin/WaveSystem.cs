using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;                   // 현재 스테이지의 모든 웨이브 정보
    [SerializeField]
    private EnemySpawner enemySpawner;
	private int currentWaveIndex = -1;

	private void Start()
	{
		StartCoroutine(StartWaveCoroutine());
	}

	// 처음 5초 대기 후, 웨이브 시작
	// 웨이브는 10초마다 시작
	// 최대 웨이브이면서 Enemy의 수가 0일 때 Clear 출력
	private IEnumerator StartWaveCoroutine()
    {
		yield return new WaitForSeconds(5f);

		while(currentWaveIndex < waves.Length - 1)
        {
			StartWave();
			yield return new WaitForSeconds(10f);

			if (currentWaveIndex == waves.Length - 1)
            {
				while(enemySpawner.EnemyList.Count > 0)
                {
					yield return null;
                }
				StageClear();
            }

		}
	}
	public void StartWave()
	{
		// 인덱스의 시작이 -1이기 때문에 웨이브 인덱스 증가를 제일 먼저 함
		currentWaveIndex++;
		// EnemySpawner의 StartWave() 함수 호출. 현재 웨이브 정보 제공
		if (currentWaveIndex < waves.Length)
		{
			enemySpawner.StartWave(waves[currentWaveIndex]);
		}
	}
	// 나중에 UI 띄울때 쓸 듯
	public void StageClear()
    {
		Time.timeScale = 0;
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