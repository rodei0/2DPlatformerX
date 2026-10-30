using UnityEngine;

[System.Serializable]
public class SaveData
{
    public bool hasStartedGame = false;
    public bool hasCompletedGame = false;
    public string lastCheckpoint = "";

    public string inputRebinds = "";

    public float masterVolume = 0.5f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    public int deaths = 0;
}
