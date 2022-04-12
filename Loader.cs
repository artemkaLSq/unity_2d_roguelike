using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;           
    //public GameObject soundManager;            


    void Awake()
    {
        if (Game.instance == null)
            Instantiate(gameManager);

        //if (SoundManager.instance == null)
        //    Instantiate(soundManager);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }
    }

    public void restartButton()
    {
        Game.instance.playerhp = 10;
        Game.instance.level = 0;
        Game.instance.playersTurn = true;
        Game.instance.enemiesMoving = false;
        Game.instance.enabled = true;
        Application.LoadLevel(Application.loadedLevel);
    }
}
