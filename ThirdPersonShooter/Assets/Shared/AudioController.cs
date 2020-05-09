using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    //[SerializeField] AudioClip clip;
    //[SerializeField] float delayBetweenClips;
    //[SerializeField] float speed = 1f;

    //bool canPlay;
    //bool playing = false;
    //AudioSource source;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    source = GetComponent<AudioSource>();
    //}

    //public void Stop()
    //{
    //    if (source && playing)
    //        source.Stop();
    //}
    //public void Play()
    //{
    //    if (playing)
    //        return;

    //    GameManager.Instance.Timer.Add(() =>
    //    {
    //        playing = false;
    //    }, delayBetweenClips);
    //    playing = true;
    //    source.pitch = speed;
    //    source.PlayOneShot(clip);
    //}
}
