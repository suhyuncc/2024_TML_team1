using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _chito;
    [SerializeField]
    private PlayerInfo _playerInfo;
    // Start is called before the first frame update
    void Start()
    {
        SetPanel(_playerInfo.current_stage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetPanel(int num)
    {
        int count = 0;

        for(int i = 0; i < _chito.Length; i++)
        {
            _chito[i].SetActive(false);

            if(count < num)
            {
                _chito[i].SetActive(true);
                count++;
            }
        }
    }
}
