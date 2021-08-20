using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeVelocity : MonoBehaviour
{

    public VelocityTracker velTrack;

    [Range (0,50)]
    public float volumeDelta = 1f;

    [Range (0,100)]
    public float maxSpeed = 10f;

    [Range (0,100)]
    public float minSpeed = 0f;

    private AudioManager am;
    private float volume;

    // Start is called before the first frame update
    void Start()
    {
        print("In chime start method");

        volume = 0f;

        am = FindObjectOfType<AudioManager>();
        am.SetVolume("chime", volume);
        am.Play("chime");
        am.Loop("chime");
    }

    // Update is called once per frame
    void Update()
    {
        // float speed = velTrack.getSpeed();
        // float lastSpeed = velTrack.getLastSpeed();

        // if(lastSpeed < speed){
        //     volume = volume + volumeDelta*Time.deltaTime;
        //     if(volume > 1){
        //         volume = 1;
        //     }
        // } else if (lastSpeed > speed){
        //     volume = volume - volumeDelta*Time.deltaTime;
        //     if(volume < 0){
        //         volume = 0;
        //     }
        // }

        float speed = velTrack.getSpeed();
        if(speed < minSpeed){
            volume = 0;
        } else if(speed > minSpeed + maxSpeed){
            volume = 1;
        } else {
            volume = speed/(minSpeed + maxSpeed);
        }


        //print("volume: " + volume + " speed: " + speed + " lastspeed: " + lastSpeed + " delta time: " + Time.deltaTime);
        print("volume: " + volume + " speed: " + speed);
        am.SetVolume("chime", volume);

    }
}
