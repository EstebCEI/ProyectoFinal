using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string mainMenu = "MainMenu";

    public GameObject MenuPausa;
    public GameObject SettingsMenu;

    public PlayerHealth playerHealth;
    public Transform player;
    public PlayerMovement playerMovement;

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
        MenuPausa.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Save()
    {
        Debug.Log("Saving game...");
        Debug.Log(Application.persistentDataPath);
        GameData data = new GameData(
            playerHealth.health,
            player.position,
            playerMovement.GetStance(),
            GameManager.instance.hasHackedComputer,
            GameManager.instance.missionCompleted,
            SaveEnemies()
        );

        GameDataJSON.SaveGameData(data);
    }

    System.Collections.Generic.List<EnemyData> SaveEnemies()
    {
        System.Collections.Generic.List<EnemyData> list = new System.Collections.Generic.List<EnemyData>();

        foreach (MonoBehaviour mb in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
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
        SceneManager.LoadScene(mainMenu);
    }
}