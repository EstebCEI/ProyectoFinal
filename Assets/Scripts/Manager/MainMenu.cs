using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string newGameSceneName = "MainGame";

    public void NewGame()
    {

    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(newGameSceneName);
    }

    public void Settings()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
