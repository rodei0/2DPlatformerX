using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindButton : MonoBehaviour
{
    [SerializeField] private int bindingIndex;
    [SerializeField] private TMP_Text keyText;
    private Image buttonImage;
    public enum ActionType
    {
        Jump,
        Dash,
        ChangeElement,
        GoBack
    }

    [SerializeField] private ActionType actionType;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Start() {
        UpdateText();
        buttonImage = GetComponent<Image>();
    }

    public void StartRebind() {
        keyText.text = "...";
        Color colorOg = buttonImage.color;
        buttonImage.color = new Color(0.419f, 0.74f, 0.47f, 0.745f);

        Action.Disable();

        rebindingOperation = Action.PerformInteractiveRebinding(bindingIndex).OnComplete(operation =>
        {
            if (IsBindingAlreadyUsed(Action, bindingIndex)) {
                Action.RemoveBindingOverride(bindingIndex);

                operation.Dispose();
                Action.Enable();

                keyText.text = "Already in use";
                buttonImage.color = colorOg;
                return;
            }
            operation.Dispose();
            Action.Enable();

            SaveManager.Instance.Data.inputRebinds =
            InputManager.Instance.PlayerControls.asset.SaveBindingOverridesAsJson();

            SaveManager.Instance.Save();

            UpdateText();
            buttonImage.color = colorOg;
        });

        rebindingOperation.Start();
    }
    private InputAction Action
    {
        get {
            switch (actionType) {
                case ActionType.Jump:
                    return InputManager.Instance.PlayerControls.Player.Jump;
                case ActionType.Dash:
                    return InputManager.Instance.PlayerControls.Player.Dash;
                case ActionType.ChangeElement:
                    return InputManager.Instance.PlayerControls.Player.ChangeElement;
                case ActionType.GoBack:
                    return InputManager.Instance.PlayerControls.Player.GoBack;
                default:
                    return null;
            }
        }
    }
    private void UpdateText() {
        //este codigo raro es para que salga en ingles siempre
        string path = Action.bindings[bindingIndex].effectivePath;
        keyText.text = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    private bool IsBindingAlreadyUsed(InputAction currentAction, int currentBinding) {
        string newPath = currentAction.bindings[currentBinding].effectivePath;

        foreach (var action in InputManager.Instance.PlayerControls.asset) {
            foreach (var binding in action.bindings) {
                // Ignora el propio binding que estamos cambiando
                if (action == currentAction &&
                    binding.id == currentAction.bindings[currentBinding].id)
                    continue;

                if (binding.effectivePath == newPath)
                    return true;
            }
        }

        return false;
    }
}
