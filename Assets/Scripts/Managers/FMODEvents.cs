using UnityEngine;
using FMODUnity;
using FMOD;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Player SFX")]
    //[field: SerializeField] public EventReference footstepSFX { get; private set; }
    //[field: SerializeField] public EventReference landSFX { get; private set; }
    [field: SerializeField] public EventReference jumpSFX { get; private set; }
    [field: SerializeField] public EventReference dashSFX { get; private set; }
    [field: SerializeField] public EventReference swapElementSFX { get; private set; }
    [field: SerializeField] public EventReference hittedSFX { get; private set; }







    //[field: Header("Music SFX")]
    //[field: SerializeField] public EventReference musicSFX { get; private set; }


    public static FMODEvents Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
