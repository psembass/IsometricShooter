using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class FModAudioService : IAudioService
{
    private Dictionary<string, EventInstance> eventInstances = new();

    public void PlayOneShot(string sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    public void PlaySoundEvent(string sound)
    {
        if (eventInstances.ContainsKey(sound))
        {
            eventInstances[sound].start();
        } 
        else
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
            eventInstances.Add(sound, eventInstance);
            eventInstance.start();
        }   
    }

    public void StopSoundEvent(string sound)
    {
        if (eventInstances.ContainsKey(sound))
        {
            eventInstances[sound].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
