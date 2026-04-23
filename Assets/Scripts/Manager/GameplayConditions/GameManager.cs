using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Misión")]
    public bool hasHackedComputer = false;
    public bool missionCompleted = false;

    public static bool loadGame = false;

    [Header("Escenas")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        ApplySceneState();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySceneState();

        if (loadGame)
        {
            StartCoroutine(LoadGameAfterScene());
        }
    }

    void ApplySceneState()
    {
        Time.timeScale = 1f;

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == mainMenuSceneName)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            PauseMenu.isPaused = false;
            return;
        }

        ForceGameplayState();
    }

    void ForceGameplayState()
    {
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        PauseMenu.isPaused = false;

        PauseMenu pause = Object.FindFirstObjectByType<PauseMenu>();

        if (pause != null)
        {
            if (pause.MenuPausa != null)
                pause.MenuPausa.SetActive(false);

            if (pause.SettingsMenu != null)
                pause.SettingsMenu.SetActive(false);
        }
    }

    IEnumerator LoadGameAfterScene()
    {
        yield return null;

        GameData data = GameDataJSON.Load();

        if (data != null)
        {
            ApplyLoadedData(data);
        }

        ForceGameplayState();

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

        var patrols = Object.FindObjectsByType<EnemyPatrolAdvanced>(FindObjectsSortMode.None);
        var guards = Object.FindObjectsByType<EnemyGuard>(FindObjectsSortMode.None);

        foreach (EnemyData data in enemies)
        {
            bool loaded = false;

            foreach (var e in patrols)
            {
                if (e.enemyID == data.enemyID)
                {
                    e.LoadData(data);
                    loaded = true;
                    break;
                }
            }

            if (loaded) continue;

            foreach (var e in guards)
            {
                if (e.enemyID == data.enemyID)
                {
                    e.LoadData(data);
                    break;
                }
            }
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

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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