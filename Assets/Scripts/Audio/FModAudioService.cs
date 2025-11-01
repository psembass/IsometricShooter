using UnityEngine;
using FMODUnity;

public class FModAudioService : IAudioService
{
    public void PlayOneShot(string sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }
}
