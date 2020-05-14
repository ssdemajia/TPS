using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    AudioSource audioSource;
    public void StartMusic()
    {
        if (audioSource.isPlaying)
            return;
        audioSource.Play();
    }
    public bool IsPlay()
    {
        return audioSource.isPlaying;
    }
    public void StopMusic()
    {
        if (!audioSource.isPlaying)
            return;
        audioSource.Stop();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartMusic();
    }
}
