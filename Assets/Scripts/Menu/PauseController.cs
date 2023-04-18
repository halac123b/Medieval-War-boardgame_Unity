using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameMode = Mode.GameMode;

public class PauseController : MonoBehaviour
{
    public static bool isPaused = false;

    public void Pause()
    {
        isPaused = true;
        GameObject.FindWithTag("PauseMenu").GetComponent<Canvas>().enabled = true;
    }

    public void Resume()
    {
        isPaused = false;
        GameObject.FindWithTag("PauseMenu").GetComponent<Canvas>().enabled = false;
    }

    public void QuitToMenu()
    {
        isPaused = false;
        SceneManager.LoadScene("Menu");
    }

}
