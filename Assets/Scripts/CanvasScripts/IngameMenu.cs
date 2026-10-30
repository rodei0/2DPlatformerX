using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour
{
    public static IngameMenu Instance { get; private set; }

    [SerializeField] private GameObject ingameMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject audioSettings;
    [SerializeField] private GameObject keybindSettings;
    [SerializeField] private GameObject winMenu;


    private bool isWinScreen = false;
    private bool isPaused;
    private bool endedTransition = false;

    private bool isMainMenuScene => SceneManager.GetActiveScene().buildIndex == 0;

    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;

        ingameMenu.SetActive(true);
        audioSettings.SetActive(true);
        keybindSettings.SetActive(true);
        settingsMenu.SetActive(true);
        winMenu.SetActive(true);
        ChangeButtonsSound();
        ingameMenu.SetActive(false);
        audioSettings.SetActive(false);
        keybindSettings.SetActive(false);
        settingsMenu.SetActive(false);
        winMenu.SetActive(false);

    }
    void Update() {
        if (InputManager.Instance.PlayerControls.Player.GoBack.WasPressedThisFrame() && !isMainMenuScene && !isWinScreen) {
            GoBack();
        }

        if (isPaused) {
            Time.timeScale = 0f;
            endedTransition = false;
        }
        else if (!isPaused && !endedTransition) {
            ReturnToNormalTime();
        }
    }

    private void ReturnToNormalTime() {
        Time.timeScale = 1f;
        endedTransition = true;
    }

    private void OnDestroy() {
        if (Instance == this) {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
            isPaused = false;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        CloseAll();
    }
    private void CloseAll() {
        ingameMenu.SetActive(false);
        settingsMenu.SetActive(false);
        audioSettings.SetActive(false);
        keybindSettings.SetActive(false);
        winMenu.SetActive(false);
        isPaused = false;
    }

    public void OnSettingsClicked() {
        ingameMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnAudioSettingsClicked() {
        settingsMenu.SetActive(false);
        audioSettings.SetActive(true);
    }

    public void OnKeybindSettingsClicked() {
        settingsMenu.SetActive(false);
        keybindSettings.SetActive(true);
    }

    public void OnContinueClicked() {
        ingameMenu.SetActive(false);
        isPaused = false;
    }

    public void OnExitClicked() {
        SceneManager.LoadScene(0);
        ingameMenu.SetActive(false);
    }

    public void OnWinMenu() {
        isWinScreen = true;
        winMenu.SetActive(true);
        isPaused = true;
    }

    private void GoBack() {
        if (ingameMenu.activeSelf || settingsMenu.activeSelf || audioSettings.activeSelf || keybindSettings.activeSelf) {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.buttonClickedUI, transform.position);
        }

        if (!ingameMenu.activeSelf && !settingsMenu.activeSelf && !audioSettings.activeSelf && !keybindSettings.activeSelf) {
            ingameMenu.SetActive(true);
            isPaused = true;
        }
        else if (ingameMenu.activeSelf) {
            ingameMenu.SetActive(false);
            isPaused = false;
        }
        else if (settingsMenu.activeSelf) {
            settingsMenu.SetActive(false);
            ingameMenu.SetActive(true);
        }
        else if (audioSettings.activeSelf) {
            audioSettings.SetActive(false);
            settingsMenu.SetActive(true);
        }
        else if (keybindSettings.activeSelf) {
            keybindSettings.SetActive(false);
            settingsMenu.SetActive(true);
        }
    }

    private void ChangeButtonsSound() {
        foreach (var button in GetComponentsInChildren<UnityEngine.UI.Button>()) {
            button.onClick.AddListener(() => AudioManager.Instance.PlayOneShot(FMODEvents.Instance.buttonClickedUI, transform.position));
        }
    }
}
