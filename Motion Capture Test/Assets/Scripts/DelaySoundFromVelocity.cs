using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

/*
 * This repeats a sound over and over with a delay period in between. The delay period is based on the velocity.
 */
public class DelaySoundFromVelocity : MonoBehaviour
{
    public AudioSource aS;

    //aS.Play();

    public VelocityTracker[] velTrackList;

    public float _scaleVariable = 0.5f;

    [Range(0, 100)]
    public float maxSpeed = 10f;

    [Range(0, 100)]
    public float minSpeed = 0f;

    [Range(0, 1)]
    public float minDelay = 1f;

    [Range(1, 10)]
    public float maxDelay = 5f;

    [Range(0, 50)]
    public float delta = 3f; //The maximum amount of change that can happen per second


    private float ratio;
    private float prevRatio;

    public float _initDelay = 1.0f;

    void Start()
    {
        //_initScale = gameObject.transform.localScale;

        ratio = 0;
        prevRatio = 0;

        
    }

    void Update()
    {
        float sum = 0f;
        foreach (var velTrack in velTrackList)
        {
            sum = sum + velTrack.getSpeed();
        }

        float speed = sum / (velTrackList.Length); //get average speed

        if (speed < minSpeed)
        {
            ratio = 0;
        }
        else if (speed > minSpeed + maxSpeed)
        {
            ratio = 1;
        }
        else
        {
            ratio = (speed / (minSpeed + maxSpeed));
        }

        //Smooth out transitions using delta
        if (ratio > prevRatio)
        {
            float maxRatio = prevRatio + delta * Time.deltaTime;
            ratio = Math.Min(maxRatio, ratio);
        }
        else if (ratio < prevRatio)
        {
            float minRatio = prevRatio - delta * Time.deltaTime;
            ratio = Math.Max(minRatio, ratio);
        }

        float delayRatio = ((maxDelay - minDelay) * ratio + minDelay);

        Debug.Log(_initDelay * delayRatio);

        //Play the audiosource after the value of this delay
        //delay = _initDelay * delayRatio
        //Invoke("playSound", _initDelay * delayRatio);
        //Invoke("playSound", 2);
        //gameObject.transform.localScale = new Vector3(_initScale.x, scale * _initScale.y, _initScale.z);
        
        /*
         * The problem:
         * This is invoking for every tick, so once it hits 2 seconds all the stuff happens at once.
         */

        prevRatio = ratio;

    }

    /*
     * Creates a new audiosource which is a copy of the initial aS, plays it, and then when thats finished it deletes it.
     */
    public void playSound()
    {   

        AudioSource cS = gameObject.AddComponent<AudioSource>();
        cS.clip = aS.clip;
        cS.volume = aS.volume;
        cS.pitch = aS.pitch;
        cS.spatialBlend = aS.spatialBlend;
        cS.spatialize = aS.spatialize;

        cS.Play();
    }

}