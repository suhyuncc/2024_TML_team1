using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject SettingPanel;

    public void Menu_button()
    {
        //Time.timeScale = 0; //게임 일시정지
        SettingPanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        SettingPanel.SetActive(false);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void BackToLobby()
    {
        SettingPanel.SetActive(false);
    }
}
