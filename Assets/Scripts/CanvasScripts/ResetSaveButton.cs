using UnityEngine;

public class ResetSaveButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            SaveManager.Instance.ResetSave();
        });
    }

}
