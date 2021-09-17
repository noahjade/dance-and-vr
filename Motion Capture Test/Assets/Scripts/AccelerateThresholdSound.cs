using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerateThresholdSound : MonoBehaviour
{

    public AccelerationThresholdColour threshCol;
    
    public VelocityTracker velTrack;
    public string soundName = "lowGat";

    public bool greaterThan = true; //if false, triggered when acceleration is below an amount.

    [Range(-20f, 20f)]
    public float threshold = 2f;

    private AudioManager am;
    

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update(){

        float acceleration = velTrack.getAcceleration();

        bool isPlaying = am.isPlaying(soundName);


        if (((greaterThan && acceleration > threshold) || (!greaterThan && acceleration < threshold)) && isPlaying == false ){
            am.Play(soundName);
            if(threshCol != null){
                threshCol.flash();
            }
            print("reached threshold, acceleration is: " + acceleration + ", playing: " + soundName);
        }

    }
}
