using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicPlayer : MonoBehaviour
{
    AudioManager am;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        am.SetVolume("music", 0.5f);
        am.Play("music");
    }
}
