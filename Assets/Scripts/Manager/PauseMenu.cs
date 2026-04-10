using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string mainMenu = "MainMenu";
    public GameObject MenuPausa;
    public GameObject SettingsMenu;
    public static bool isPaused;
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        MenuPausa.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        MenuPausa.SetActive(false);
        isPaused = false;
    }

    public void Save()
    {

    }

    public void Settings()
    {
        SettingsMenu.SetActive(true);
        MenuPausa.SetActive(false);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(mainMenu);
    }
}