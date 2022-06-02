using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    [HideInInspector]
    public GameObject current_level_go;
    [SerializeField]
    private MainCanvas main_canvas;
    [SerializeField]
    private Player player;
    private bool game_started = false;
    [SerializeField]
    private GameObject[] level_prefabs;
    private int current_level_index=0;
    #endregion
    private void Awake()
    {
        int level = PlayerPrefs.GetInt("level",0);
        current_level_index = level;
    }
    private void Start()
    {
        LoadLevel(current_level_index);
#if !UNITY_EDITOR
        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 60;
#endif
    }

    void Update()
    {
        if (Input.touchCount > 0 && !game_started)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Run();
            }

        }
    }
    #region Commands
    private void Run()
    {
        game_started = true;
        player.Activate();

    }
    private void LoadLevel(int index)
    {
        player.GoStart();
        Destroy(current_level_go);
        GameObject new_level_go = Instantiate(level_prefabs[index]);
        current_level_go = new_level_go;
        game_started = false;
        
    }

    public void NextLevel()
    {
        current_level_index++;
        LoadLevel(current_level_index);
        PlayerPrefs.SetInt("level", current_level_index);
    }
    public void RetryLevel()
    {
        LoadLevel(current_level_index);
    }
    public void LevelCompleted()
    {
        main_canvas.LevelCompletedScreen();
    }

    public void LevelFailed()
    {
        main_canvas.LevelFailedScreen();
    }
    #endregion

}
