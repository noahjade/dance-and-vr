using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicPlayer : MonoBehaviour
{
    AudioManager am;
    [Range(0f,1f)]
    public float volume = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        am.SetVolume("music", volume);
        am.Play("music");
    }
}
