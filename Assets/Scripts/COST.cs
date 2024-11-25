using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class COST : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate _tower;
    [SerializeField]
    private TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        text.text = $"${_tower.weapon[0].cost}";
    }
}
