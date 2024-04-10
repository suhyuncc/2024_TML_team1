using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate towerTemplate;
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private int towerBuildGold = 50;
    [SerializeField]
    private EnemySpawner enemySpawner;     // 현재 맵에 존재하는 적 리스트 정보를 얻기 위해
    [SerializeField]
    private PlayerGold playerGold;

    

    private bool isOnTowerButton = false;  // 타워 건설 버튼을 눌렀는지 체크

    public void ReadyToSpawnTower()
    {
        /*
        if(towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        {
            return;
        }
        */
        isOnTowerButton = true;
    }
    public void SpawnTower(Transform tileTransform)
    {
        if(isOnTowerButton == false)
        {
            return;
        }
        if(towerBuildGold > playerGold.CurrentGold)
        {
            return;
        }
        Tile tile = tileTransform.GetComponent<Tile>();

        // 타워 건설 가능 여부 확인
        // 1. 현재 타일의 위치에 이미 타워가 건설되어 있으면 타워 건설 X
        if(tile.IsBuildTower == true)
        {
            return;
        }

        // 타워가 건설되어 있음으로 설정
        tile.IsBuildTower = true;
        // 타워 건설에 필요한 골드만큼 감수
        playerGold.CurrentGold -= towerBuildGold;
        // 선택한 타일의 위치에 타워 건설
        GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        // 타워 무기에 enemySpawner 정보 전달
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }
}
