using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;          //생성될 타워 프리펩
    public GameObject followTowerPrefab;    //생성전 마우스를 따라다니는 타워 프리팹
    public Weapon[] weapon;
    public int towerType;

    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;
        public string name;
        public float damage;
        public float slow;      // 감속 퍼센트 (0.2 = 20%)
        public float buff;      // 공격속돈 버프
        public float debuff;    // 받는 피해 증가
        public float rate;
        public float range;
        public int cost;
        public int sell;
    }
}
