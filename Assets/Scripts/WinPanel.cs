using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo _playerInfo;

    private void OnEnable()
    {
        Time.timeScale = 0.0f;
    }

    public void GoToScene(string sceneName)
    {
        _playerInfo.current_stage += 1;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
