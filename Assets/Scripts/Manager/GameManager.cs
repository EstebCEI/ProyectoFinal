using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public GameObject hud;           // Todo tu HUD (vida, crosshair, etc)
    public GameObject deathScreen;   // Panel de muerte

    private bool isDead = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayerDied()
    {
        if (isDead) return;
        isDead = true;

        // Ocultar HUD
        if (hud != null)
            hud.SetActive(false);

        // Mostrar pantalla de muerte
        if (deathScreen != null)
            deathScreen.SetActive(true);

        // Parar el juego
        Time.timeScale = 0f;

        // Liberar cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

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