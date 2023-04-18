using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameMode = Mode.GameMode;

public class MenuController : MonoBehaviour
{
    public void PlayPvP() {
        Mode.mode = GameMode.PvP;
        SceneManager.LoadScene("Game");
    }

    public void PlayAI() {
        Mode.mode = GameMode.AI;
        SceneManager.LoadScene("Game");
    }

    public void PlayAIHard() {
        Mode.mode = GameMode.AIHard;
        SceneManager.LoadScene("Game");
    }


    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif    
    }

}
