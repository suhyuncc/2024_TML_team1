using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;
    public GameObject followTowerPrefab; //임시 타워 프리팹
    public Weapon[] weapon;
    public int towerType;

    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;
        public AnimatorController animatorController;
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
