using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public void PlayTheGame()
    {
        SceneManager.LoadScene("Justin");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
