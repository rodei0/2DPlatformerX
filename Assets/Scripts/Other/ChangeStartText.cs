using System.IO;
using TMPro;
using UnityEngine;

public class ChangeStartText : MonoBehaviour
{
    private TMP_Text TMP_Text;
    private void Awake() {
        TMP_Text = GetComponent<TMP_Text>();
    }
    void Start()
    {
        Refresh();
    }

    private void OnEnable() {
        Refresh();
    }
    

    void Refresh() {
        if (SaveManager.Instance.Data.hasStartedGame) {
            TMP_Text.text = "Continue";
        }
        else {
            TMP_Text.text = "Start";
        }
    }
}
