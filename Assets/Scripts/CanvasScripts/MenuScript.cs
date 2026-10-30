using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject defaultMenu;
    [SerializeField] private GameObject rebindMenu;
    [SerializeField] private GameObject audioMenu;

    private PlayerControls playerControls;

    private void Start() {
        settings.SetActive(false);
        defaultMenu.SetActive(true);
        rebindMenu.SetActive(false);
        audioMenu.SetActive(false);
        playerControls = InputManager.Instance.PlayerControls;
    }

    private void Awake() {
        settings.SetActive(true);
        defaultMenu.SetActive(true);
        rebindMenu.SetActive(true);
        audioMenu.SetActive(true);
        ChangeButtonsSound();
    }
    // Update is called once per frame
    void Update() {
        if (playerControls.Player.GoBack.WasPressedThisFrame()) {
            GoBack();
        }
    }


    public void OnPlayClick() {
        SceneManager.LoadScene(1);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.changeScene, transform.position);
        SaveManager.Instance.HasStartedGame();
    }

    public void OnClickSettings() {
        defaultMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void OnKeybindSettings() {
        defaultMenu.SetActive(false);
        settings.SetActive(false);
        rebindMenu.SetActive(true);
    }

    public void OnAudioSettings() {
        defaultMenu.SetActive(false);
        settings.SetActive(false);
        audioMenu.SetActive(true);
    }

    private void GoBack() {
        if (settings.activeSelf) {
            settings.SetActive(false);
            defaultMenu.SetActive(true);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.goBackMenu, transform.position);
        }
        if (rebindMenu.activeSelf) {
            rebindMenu.SetActive(false);
            settings.SetActive(true);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.goBackMenu, transform.position);
        }
        if (audioMenu.activeSelf) {
            audioMenu.SetActive(false);
            settings.SetActive(true);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.goBackMenu, transform.position);
        }
    }

    private void ChangeButtonsSound() {
        foreach (var button in GetComponentsInChildren<UnityEngine.UI.Button>()) {
            button.onClick.AddListener(() => AudioManager.Instance.PlayOneShot(FMODEvents.Instance.buttonClickedUI, transform.position));
        }
    }
}
