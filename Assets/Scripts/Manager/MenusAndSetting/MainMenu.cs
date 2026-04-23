using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "MainGame";

    [Header("UI")]
    public GameObject mainMenuUI;
    public GameObject settingsMenuUI;

    [Header("Botón Continue")]
    public Button continueButton;
    public TMP_Text continueText;

    [Header("Colores Texto")]
    public Color disabledTextColor = new Color32(111, 133, 111, 255); // #6F856F

    [Header("Otros")]
    [SerializeField] private GameObject endScreen;

    private Color originalTextColor;

    void Start()
    {
        Time.timeScale = 1f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        mainMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);


        if (continueText != null)
            originalTextColor = continueText.color;

        UpdateContinueButton();
    }

    void UpdateContinueButton()
    {
        bool hasSave = File.Exists(GameDataJSON.GetPath());

        continueButton.interactable = hasSave;

        if (continueText != null)
        {
            continueText.color = hasSave ? originalTextColor : disabledTextColor;
        }
    }

    public void ContinueGame()
    {
        if (!File.Exists(GameDataJSON.GetPath()))
            return;

        GameManager.loadGame = true;
        SceneManager.LoadScene(gameSceneName);
    }

    public void NewGame()
    {
        GameDataJSON.Delete();

        if (GameManager.instance != null)
            GameManager.instance.ResetState();

        GameManager.loadGame = false;
        SceneManager.LoadScene(gameSceneName);
        if (endScreen != null)
            endScreen.SetActive(false);
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