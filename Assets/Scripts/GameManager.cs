using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    private SoundInfo _soundinfo;
    [SerializeField]
    private PlayerInfo _playerinfo;

    public int current_stage;
    public int previous_stage;

    private void Awake()
    {
        GameReset();
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void GameReset()
    {
        DontDestroyOnLoad(gameObject);

        _soundinfo.Master_value = 100;
        _soundinfo.BGMvalue = 100;
        _soundinfo.SFXvalue = 100;

        _playerinfo.current_stage = 1;
        _playerinfo.previous_stage = 0;
        //current_stage = 1;
        //previous_stage = 0;
    }
}
