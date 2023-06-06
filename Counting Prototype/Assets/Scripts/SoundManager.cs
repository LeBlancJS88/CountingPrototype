using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] audioClips; // Array of audio clips to play

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < audioClips.Length)
        {
            audioSource.PlayOneShot(audioClips[clipIndex]);
        }
    }
}
