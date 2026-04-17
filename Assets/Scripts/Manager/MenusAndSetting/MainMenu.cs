using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "MainGame";

    public GameObject mainMenuUI;
    public GameObject settingsMenuUI;

    void Start()
    {
        mainMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }

    public void ContinueGame()
    {
        if (!System.IO.File.Exists(GameDataJSON.GetPath()))
        {
            NewGame();
            return;
        }

        GameManager.loadGame = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void NewGame()
    {
        GameDataJSON.Delete();
        GameManager.loadGame = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    bool SaveExists()
    {
        string path = GameDataJSON.GetPath();
        return File.Exists(path);
    }

    public void Settings()
    {
        mainMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    public void BackFromSettings()
    {
        settingsMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}