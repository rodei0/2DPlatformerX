using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightOscile : MonoBehaviour
{
    new Light2D light;
    bool canUse = true;
    private float oscillationTime;
    [SerializeField]private float minIntensity;
    [SerializeField]private float maxIntensity;
    void Start()
    {
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canUse) {
            oscillationTime += Time.deltaTime;
            LightOsciling();
        }
    }

    private void LightOsciling() {
        float t = Mathf.PingPong(oscillationTime, 1f);
        light.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }

    private IEnumerator FadeLight() {
        canUse = false;
        float startIntensity = light.intensity;

        for (float i = startIntensity; i >= 0; i -= Time.deltaTime * 2.5f) {
            light.intensity = i;
            yield return null;
        }

        light.intensity = 0;
    }

    private IEnumerator FadeInLight() {
        for (float i = 0; i <= minIntensity; i += Time.deltaTime * 2.5f) {
            light.intensity = i;
            yield return null;
        }
        light.intensity = minIntensity;
        oscillationTime = 0f;
        canUse = true;
    }

    public void FadelightFunc() {
        StartCoroutine(FadeLight());
    }

    public IEnumerator FadeInLightFunc() {
        return FadeInLight();
    }
}
