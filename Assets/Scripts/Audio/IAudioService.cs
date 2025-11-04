using UnityEngine;

public interface IAudioService
{
    void PlayOneShot(string sound, Vector3 position);
    void PlaySoundEvent(string sound);
    void StopSoundEvent(string sound);
}
