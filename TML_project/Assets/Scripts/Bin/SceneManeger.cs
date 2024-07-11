using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneManeger : MonoBehaviour
{
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }


    [SerializeField]
    private GameObject[] _buildings;

   
    public void NextStage(int number)
    {
       _buildings[number].SetActive(true);
       
    }
    
    public void GameExit()
    {
	    Application.Quit();
    }
}
