using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo _playerInfo;
    [SerializeField]
    private AudioClip _SFX;

    private void OnEnable()
    {
        SFX_Manager.instance.Play_oneshot(_SFX);
        Time.timeScale = 0.0f;
    }

    public void GoToScene(string sceneName)
    {
        _playerInfo.current_stage += 1;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
