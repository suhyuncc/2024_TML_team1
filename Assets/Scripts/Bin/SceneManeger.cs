using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneManeger : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _buildings;
    [SerializeField]
    private int _currentStage;
    [SerializeField]
    private PlayerInfo _playerInfo;
    private Image test;

    public void GoToScene(string sceneName)
    {
        
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }


    

    private void Start()
    {
        if (_playerInfo.current_stage != _playerInfo.previous_stage)
        {
            _playerInfo.previous_stage = _playerInfo.current_stage;
            //스토리 출력
            Dialogue_Manage.instance.GetEventName("Stage_" + _playerInfo.current_stage.ToString());
        }

        ShowStage(_playerInfo.current_stage);


    }

    private void Update()
    {
        //ShowStage(_currentStage);
    }

    public void ShowStage(int number)
    {
        for(int i = 0; i < number; i++)
        {
            _buildings[i].SetActive(true);
        }

        for (int i = 0; i < number - 1; i++)
        {
            _buildings[i].GetComponent<Button>().interactable = false;
        }
    }
    
    public void GameExit()
    {
	    Application.Quit();
    }
}
