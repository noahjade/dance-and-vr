using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VolumeSound : MonoBehaviour
{

    private float prevVol;

    public VelocityTracker[] velTrackList;

    public string soundName = "chime";

    [Range (0,50)]
    public float volumeDelta = 1f; //The maximum amount of change that can happen to the volume per second

    [Range (0,100)]
    public float maxSpeed = 10f;

    [Range (0,100)]
    public float minSpeed = 0f;

    [Range (0f,0.99f)]
    public float volumeFloor = 0f;

    private AudioManager am;
    private float volume;

    // Start is called before the first frame update
    void Start()
    {
        volume = 0f;
        prevVol = 0f;

        am = FindObjectOfType<AudioManager>();
        am.SetVolume(soundName, volume);
        am.Play(soundName);
        am.Loop(soundName);
    }

    // Update is called once per frame
    void Update()
    {
        float sum = 0;
        foreach (var velTrack in velTrackList)
        {
            sum = sum + velTrack.getSpeed();
        }

        float speed = sum/(velTrackList.Length); //get average speed

        if(speed < minSpeed){
            volume = volumeFloor;
        } else if(speed > minSpeed + maxSpeed){
            volume = 1;
        } else {
            volume = (speed/(minSpeed + maxSpeed))*(1-volumeFloor) + volumeFloor;
        }

        //Smooth out transitions using deltaVolume
        if(volume > prevVol){
            float maxVol = prevVol + volumeDelta*Time.deltaTime;
            volume = Math.Min(maxVol, volume);
        } else if (volume < prevVol){
            float minVol = prevVol - volumeDelta*Time.deltaTime;
            volume = Math.Max(minVol, volume);
        }

        //print("volume: " + volume + " speed: " + speed + " lastspeed: " + lastSpeed + " delta time: " + Time.deltaTime);
        //print("volume: " + volume + " speed: " + speed);
        am.SetVolume(soundName, volume);
        prevVol = volume;

    }
}
