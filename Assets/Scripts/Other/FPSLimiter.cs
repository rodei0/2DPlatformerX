using UnityEngine;

public static class FPSLimiter
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 240;
    }
}
