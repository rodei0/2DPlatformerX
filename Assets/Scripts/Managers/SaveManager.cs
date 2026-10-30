using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public SaveData Data;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return; 
        }

        Instance = this;

        Data = SaveSystem.Load();
    }

    public void Save() {
        SaveSystem.Save(Data);
    }

    public void Load() {
        Data = SaveSystem.Load();
    }

    public void ResetSave() {
        SaveSystem.DeleteSave();
        Data = new SaveData();
        SaveSystem.Save(Data);
        SceneManager.LoadScene(0);
    }

    public void SetCheckpoint (string checkpoint) {
        Data.lastCheckpoint = checkpoint;
        Save();
    }

    public void AddDeathCount() {
        Data.deaths++;
        Save();
    }

    public void HasStartedGame() {
        Data.hasStartedGame = true;
        Save();
    }

    public void HasCompletedGame() {
        Data.hasCompletedGame = true;
        Save();
    }

}
