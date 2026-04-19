using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireWaterScript : MonoBehaviour
{
    [SerializeField] Color colorA;
    [SerializeField] Color colorB;
    [SerializeField] float intensityA;
    [SerializeField] float intensityB;

    new Light2D light;

    void Start()
    {
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        OscileBetween2Colors();
        OscilebetweenIntensities();
    }

    void OscileBetween2Colors()
    {
        float t = Mathf.PingPong(Time.time, 1f);
        light.color = Color.Lerp(colorA, colorB, t);
    }

    void OscilebetweenIntensities()
    {
        
        float t = Mathf.PingPong(Time.time, 1f);
        light.intensity = Mathf.Lerp(intensityA, intensityB, t);
    }
}
