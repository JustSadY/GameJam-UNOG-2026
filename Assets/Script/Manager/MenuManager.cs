using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TMP_Text playButtonText;

    [Header("Settings")]
    [SerializeField] private string mainMenuSceneName = "mainmenu";
    [SerializeField] private int gameSceneBuildIndex = 1;

    [Header("Dependencies")]
    [SerializeField] private GameObject player;

    private bool isMainMenu;
    private bool isPaused = false;

    private void Awake()
    {
        // Ensure the script persists across scenes if needed, 
        // but typically a MenuManager exists per scene or is a Singleton.
        SetupSceneState();
    }

    private void Start()
    {
        InitializeUIReferences();
        BindButtons();
        UpdateUIState();
    }

    private void Update()
    {
        if (!isMainMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    private void SetupSceneState()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        isMainMenu = (currentSceneName == mainMenuSceneName);
        Time.timeScale = 1f;
    }

    private void InitializeUIReferences()
    {
        // Search for Menu Panel if not assigned (including inactive objects)
        if (menuPanel == null)
        {
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                Transform[] allTransforms = canvas.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in allTransforms)
                {
                    if (t.name.ToLower() == "menu")
                    {
                        menuPanel = t.gameObject;
                        break;
                    }
                }
            }
        }

        if (menuPanel != null)
        {
            // Find buttons within the panel even if they are inactive
            if (playButton == null) playButton = FindChildButton(menuPanel, "play");
            if (quitButton == null) quitButton = FindChildButton(menuPanel, "exit");

            if (playButtonText == null && playButton != null)
            {
                playButtonText = playButton.GetComponentInChildren<TMP_Text>(true);
            }
        }
    }

    private Button FindChildButton(GameObject parent, string nameTag)
    {
        Button[] buttons = parent.GetComponentsInChildren<Button>(true);
        foreach (var btn in buttons)
        {
            if (btn.name.ToLower().Contains(nameTag)) return btn;
        }
        return null;
    }

    private void BindButtons()
    {
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners(); // Prevent double binding
            playButton.onClick.AddListener(OnPlayClicked);
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(OnQuitClicked);
        }
    }

    private void UpdateUIState()
    {
        if (isMainMenu)
        {
            if (menuPanel != null) menuPanel.SetActive(true);
            if (playButtonText != null) playButtonText.text = "Oyna";
        }
        else
        {
            if (menuPanel != null) menuPanel.SetActive(false);
            if (playButtonText != null) playButtonText.text = "Devam Et";
        }
    }

    public void ToggleMenu()
    {
        isPaused = !isPaused;

        if (menuPanel != null) menuPanel.SetActive(isPaused);
        if (player != null) player.SetActive(!isPaused);

        Time.timeScale = isPaused ? 0f : 1f;

        // Ensure cursor is visible when paused
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnPlayClicked()
    {
        Debug.Log("uyyy calisti");
        if (isMainMenu)
        {
            SceneManager.LoadScene(gameSceneBuildIndex);
        }
        else
        {
            ToggleMenu();
        }
    }

    private void OnQuitClicked()
    {
        Debug.Log("Quit Game Requested");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}