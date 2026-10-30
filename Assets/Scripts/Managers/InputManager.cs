using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static InputManager Instance { get; private set; }

    public PlayerControls PlayerControls { get; private set; }
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        PlayerControls = new PlayerControls();
        if (!string.IsNullOrEmpty(SaveManager.Instance.Data.inputRebinds)) {
            PlayerControls.asset.LoadBindingOverridesFromJson(
                SaveManager.Instance.Data.inputRebinds
            );
        }
        PlayerControls.Enable();
    }

    private void OnDestroy() {
        PlayerControls?.Disable();
    }


}
