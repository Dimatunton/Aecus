using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void InfiniteMode()
    {
        SceneManager.LoadScene("Infinite Mode_scene");
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("0_TutorialLevel_scene");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
