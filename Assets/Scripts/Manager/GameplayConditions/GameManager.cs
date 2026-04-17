using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Misión")]
    public bool hasHackedComputer = false;
    public bool missionCompleted = false;

    public static bool loadGame = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (!loadGame)
            return;

        GameData data = GameDataJSON.Load();
        if (data == null)
            return;

        ApplyLoadedData(data);

        loadGame = false;
    }

    void ApplyLoadedData(GameData data)
    {
        var player = Object.FindFirstObjectByType<PlayerMovement>();
        var health = Object.FindFirstObjectByType<PlayerHealth>();

        if (player != null)
        {
            player.transform.position = data.playerPosition;
            player.SetStance(data.playerStance);
        }

        if (health != null)
        {
            health.SetHealth(data.playerHealth);
        }

        hasHackedComputer = data.hasHackedComputer;
        missionCompleted = data.missionCompleted;

        LoadEnemies(data.enemies);
    }

    void LoadEnemies(List<EnemyData> enemies)
    {
        if (enemies == null) return;

        var allEnemies = Object.FindObjectsByType<EnemyPatrolAdvanced>(FindObjectsSortMode.None);

        for (int i = 0; i < enemies.Count && i < allEnemies.Length; i++)
        {
            allEnemies[i].LoadData(enemies[i]);
        }
    }

    public void HackComputer()
    {
        if (hasHackedComputer) return;
        hasHackedComputer = true;
    }

    public void CompleteMission()
    {
        if (!hasHackedComputer || missionCompleted) return;

        missionCompleted = true;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetState()
    {
        hasHackedComputer = false;
        missionCompleted = false;
        loadGame = false;
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}