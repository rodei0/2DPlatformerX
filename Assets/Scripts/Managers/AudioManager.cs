using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1f;
    [Range(0, 1)]
    public float musicVolume = 0.5f;
    [Range(0, 1)]
    public float sfxVolume = 1f;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    public static AudioManager Instance { get; private set; }
    private List<EventInstance> eventInstances = new List<EventInstance>();
    private EventInstance musicInstance;

    bool musicStarted = false;

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

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);

        //if (!musicStarted && Input.anyKeyDown)
        //{
        //    InitializeMusic(FMODEvents.Instance.musicSFX);
        //    musicStarted = true;
        //}
    }
    private void Start()
    {
        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    public void PlayOneShot(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    public EventInstance CreateEventInstance(EventReference sound)
    {
        EventInstance instance = RuntimeManager.CreateInstance(sound);
        eventInstances.Add(instance);
        return instance;
    }

    private void InitializeMusic(EventReference musicSound)
    {
        musicInstance = CreateEventInstance(musicSound);
        musicInstance.start();
    }

    public void Cleanup()
    {
        foreach (var instance in eventInstances)
        {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }
    }

    private void OnDestroy()
    {
        Cleanup();
    }
}
