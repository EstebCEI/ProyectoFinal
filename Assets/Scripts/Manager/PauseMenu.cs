using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string newGameSceneName = "MainMenu";

    public void Resume()
    {

    }

    public void Save()
    {
    }

    public void Settings()
    {

    }

    public void ExitGame()
    {
        SceneManager.LoadScene(newGameSceneName);
    }
}