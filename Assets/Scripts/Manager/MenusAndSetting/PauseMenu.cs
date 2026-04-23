using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string mainMenu = "MainMenu";

    public GameObject MenuPausa;
    public GameObject SettingsMenu;

    public PlayerHealth playerHealth;
    public Transform player;
    public PlayerMovement playerMovement;

    public static bool isPaused;

    [Header("Otros")]
    [SerializeField] private GameObject endScreen;

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

        MenuPausa.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Save()
    {
        if (!isPaused)
        {
            Debug.LogWarning("Debes pausar el juego para guardar");
            return;
        }

        GameData data = new GameData(
            playerHealth.GetHealth(),
            player.position,
            playerMovement.GetStance(),
            GameManager.instance.hasHackedComputer,
            GameManager.instance.missionCompleted,
            SaveEnemies()
        );

        GameDataJSON.SaveGameData(data);
    }

    List<EnemyData> SaveEnemies()
    {
        List<EnemyData> list = new List<EnemyData>();

        var all = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var mb in all)
        {
            if (mb is IEnemySaveable enemy)
            {
                list.Add(enemy.GetData());
            }
        }

        return list;
    }

    public void Settings()
    {
        SettingsMenu.SetActive(true);
        MenuPausa.SetActive(false);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);

        if (endScreen != null)
            endScreen.SetActive(false);
    }
}