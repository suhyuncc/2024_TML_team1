using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer _sprite;

    // 타일에 타워가 건설되어 있는지 검사하는 변수
    public bool IsBuildTower { set; get; }

    private void Awake()
    {
        IsBuildTower = false;
        _sprite = this.GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        if(TowerSpawner.Instance.GetisOnTowerButton())
        {
            _sprite.color = new Color(0.8f, 0.8f, 0.8f, 0.2f);
        }
    }

    private void OnMouseExit() 
    {
        _sprite.color = new Color(1, 1, 1, 0);
        
    }
}
