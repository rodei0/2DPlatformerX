using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextDeaths : MonoBehaviour
{
    TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Deaths: " + SaveManager.Instance.Data.deaths.ToString();
    }
}
