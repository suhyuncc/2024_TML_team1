using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler
{
    private SpriteRenderer _sprite;

    // 타일에 타워가 건설되어 있는지 검사하는 변수
    public bool IsBuildTower { set; get; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TowerSpawner.Instance.GetisOnTowerButton())
        {
            _sprite.color = new Color(0.8f, 0.8f, 0.8f, 0.2f);
        }
    }

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
