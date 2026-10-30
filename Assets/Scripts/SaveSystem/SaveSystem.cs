using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string path => Path.Combine(Application.persistentDataPath, "save.json");

    public static bool HasSave() => File.Exists(path);

    public static void Save(SaveData data) {
        string json = JsonUtility.ToJson(data,true);
        File.WriteAllText(path, json);
    }

    public static SaveData Load() {
        if (!File.Exists(path)) {
        return new SaveData();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }
    public static void DeleteSave() {
        if (File.Exists(path))
            File.Delete(path);
    }
}
