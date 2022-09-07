using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void RestartLevel() // Restarts the level after a GAME OVER
    {
        SceneManager.LoadScene(0);
    }
}
