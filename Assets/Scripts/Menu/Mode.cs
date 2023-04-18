

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mode : MonoBehaviour
{
    public enum GameMode { PvP, AI, AIHard }
    public static GameMode mode;
}
