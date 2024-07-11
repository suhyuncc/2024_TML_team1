using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TowerTemplate;

public class TowerMerge : MonoBehaviour
{
    [SerializeField]
    private TowerWeapon[] merges;
    private int index_count;
    private void Awake()
    {
        merges = new TowerWeapon[3];
        index_count = 0;
    }

    private void Update()
    {
        if(index_count == 3)
        {
            Merge();
            index_count = 0;
        }
    }

    public void AddTower(Transform towerWeapon)
    {
        // 타워 정보를 받아오기
        TowerWeapon weapon = towerWeapon.GetComponent<TowerWeapon>();

        if (merges[0].TowerType != weapon.TowerType ||
            merges[0].Level != weapon.Level ||
            weapon.Is_Selected)
        {
            return;
        }
        
        merges.SetValue(weapon, index_count);
        index_count++;
    }

    public void SetMainTower(Transform towerWeapon)
    {
        // 타워 정보를 받아오기
        TowerWeapon weapon = towerWeapon.GetComponent<TowerWeapon>();

        merges[0] = weapon;
        merges[0].Is_Selected = true;
        index_count++;
    }

    public void MergeCancel()
    {
        for (int i = 0; i < index_count; i++)
        {
            merges[i].Is_Selected = false;
            merges[i] = null;
        }
        index_count = 0;
    }

    private void Merge()
    {
        for(int i = 0; i < merges.Length; i++)
        {
            if(i == 0)
            {
                merges[i].MergeUpgrade();
            }
            else
            {
                merges[i].MergeSell();
            }
            merges[i] = null;
        }

        //머지모드 해제
        ObjectDetector.Instance.ResetMergeMode();
    }
}
