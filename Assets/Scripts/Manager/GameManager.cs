using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Misión")]
    public bool hasHackedComputer = false;
    public bool missionCompleted = false;

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

    // -------- HACK --------

    public void HackComputer()
    {
        if (hasHackedComputer) return;

        hasHackedComputer = true;
    }

    // -------- COMPLETAR MISIÓN --------

    public void CompleteMission()
    {
        if (!hasHackedComputer || missionCompleted) return;

        missionCompleted = true;

        Debug.Log("🏆 MISIÓN COMPLETADA");

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // -------- UTILIDADES --------

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}