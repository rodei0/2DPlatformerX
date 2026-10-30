using TMPro;
using UnityEngine;

public class SliderAudio : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider sliderMaster;
    [SerializeField] private UnityEngine.UI.Slider sliderSFX;
    [SerializeField] private UnityEngine.UI.Slider sliderMusic;

    [SerializeField] TMP_Text textMaster;
    [SerializeField] TMP_Text textSFX;
    [SerializeField] TMP_Text textMusic;

    private void Awake() {
        sliderMaster.value = SaveManager.Instance.Data.masterVolume;
        sliderSFX.value = SaveManager.Instance.Data.sfxVolume;
        sliderMusic.value = SaveManager.Instance.Data.musicVolume;
        textMaster.text = Mathf.RoundToInt(sliderMaster.value * 100) + "%";
        textSFX.text = Mathf.RoundToInt(sliderSFX.value * 100) + "%";
        textMusic.text = Mathf.RoundToInt(sliderMusic.value * 100) + "%";
    }
    public void OnGlobalSliderChange() {
        SaveManager.Instance.Data.masterVolume = sliderMaster.value;
        SaveManager.Instance.Save();
        textMaster.text = Mathf.RoundToInt(sliderMaster.value * 100) + "%";

    }

    public void OnSFXSliderChange() {
        SaveManager.Instance.Data.sfxVolume = sliderSFX.value;
        SaveManager.Instance.Save();
        textSFX.text = Mathf.RoundToInt(sliderSFX.value * 100) + "%";
    }

    public void OnMusicSliderChange() {
        SaveManager.Instance.Data.musicVolume = sliderMusic.value;
        SaveManager.Instance.Save();
        textMusic.text = Mathf.RoundToInt(sliderMusic.value * 100) + "%";
    }
}
