using System.Collections;
using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TextAnims : MonoBehaviour
{
    [Tooltip("If true, the text will have a glow animation.")]
    [SerializeField] private bool glowAnim = true;
    [Tooltip("If true, the text will have a write animation.")]
    [SerializeField] private bool writeAnim = true;
    [Tooltip("If true, the text will have sfx when it's appearing.")]
    [SerializeField] private bool hasAudio = true;

    [SerializeField] private float CharsPerSecond = 50f;
    [Tooltip("The speed of the glow animation, lower is slower.")]
    [SerializeField] private float glowAnimSpeed = 0.75f;
    [Tooltip("How often the sfx will sound (ex: 2 means, every 2 char, 5, every 5 char...")]
    [SerializeField] private int sfxEveryXChars = 2;

    public enum KeybindType
    {
       None,
       Jump, 
       Dash,
       Adapt
    }

    [SerializeField] private KeybindType textKeybinds;

    private TMP_Text textComponent;
    void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        if (textKeybinds == KeybindType.None) return;
        string key = GetKeybind(textKeybinds);
        textComponent.text = $"press {key} to {textKeybinds.ToString().ToLower()}";
    }

    private string GetKeybind(KeybindType type) {
        string path = type switch {
            KeybindType.Jump =>
                InputManager.Instance.PlayerControls.Player.Jump.bindings[0].effectivePath,

            KeybindType.Dash =>
                InputManager.Instance.PlayerControls.Player.Dash.bindings[0].effectivePath,

            KeybindType.Adapt =>
                InputManager.Instance.PlayerControls.Player.ChangeElement.bindings[0].effectivePath,

            _ => ""
        };

        return InputControlPath.ToHumanReadableString(
            path,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );
    }



    // Update is called once per frame
    void Update()
    {
        if (glowAnim) ChangeGlowText();
    }

    private void OnEnable() {
        if (writeAnim) StartCoroutine(ShowText());
    }
    private void OnDisable() {
        if (writeAnim) textComponent.maxVisibleCharacters = 0;
    }

    private IEnumerator ShowText() {
        textComponent.ForceMeshUpdate();

        textComponent.maxVisibleCharacters = 0;

        int totalCharacters = textComponent.textInfo.characterCount;
        while (textComponent.maxVisibleCharacters < totalCharacters) {
            textComponent.maxVisibleCharacters += 1;
            if (textComponent.maxVisibleCharacters % sfxEveryXChars == 0 && hasAudio) {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.textAppearing, transform.position);
            }
            yield return new WaitForSeconds(1f / CharsPerSecond);
        }
    }
    private void ChangeGlowText() {
        float glow = Mathf.PingPong(Time.time * 0.75f, 1f);

        textComponent.fontMaterial.SetFloat(ShaderUtilities.ID_GlowOuter, glow);
    }
}
