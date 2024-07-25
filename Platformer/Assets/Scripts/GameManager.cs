using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //creates the gamemanager instance
    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance;

    //variables

    //cutscenes
    public bool inCutscene;

    //player
    public Vector3 SpawnPosition;

    //UI
    public bool GameMenuOpen;

    void Awake()
    {
        //makes sure there is only one gamemanager instance and sets that instance to this
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        inCutscene = false;

        SpawnPosition = new Vector3(0, -2, 0);

        GameMenuOpen = false;
    }

    private void Update()
    {
        if (GameMenuOpen) { Time.timeScale = 0; } 
        if (!GameMenuOpen) { Time.timeScale = 1; }
    }
}
