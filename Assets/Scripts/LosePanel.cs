using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0.0f;
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        this.gameObject.SetActive(false);
        string currentSceneName = SceneManager.GetActiveScene().name; // 현재 씬 이름 가져오기
        SceneManager.LoadScene(currentSceneName);
    }
}
