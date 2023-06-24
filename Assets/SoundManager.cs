
using UnityEngine;

public class SoundManager : SingletonMonobehavior<SoundManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] popSounds;

    public void SoundPop()
    {
        var i = Random.Range(0, popSounds.Length);
        Debug.Log($"playing pop clip {i}");
        audioSource.PlayOneShot(popSounds[i]);
    }

    public void SetVolume(float val)
    {
        audioSource.volume = val;
    }

    public float GetVolume() => audioSource.volume;
}
