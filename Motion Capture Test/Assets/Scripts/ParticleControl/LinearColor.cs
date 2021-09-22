using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

// This takes the input from 3 different linear sliders to produce a color
public class LinearColor : MonoBehaviour
{
    public LinearMapping rMap;
    public LinearMapping gMap;
    public LinearMapping bMap;

    //this is the image we are changing the color of.
    public Image img;

    // for keeping track of when changes are made
    private float currentrMap = 0.0f;
    private float currentgMap = 0.0f;
    private float currentbMap = 0.0f;
    private int framesUnchanged = 0;


    // Update is called once per frame
    void Update()
    {
        if (currentrMap != rMap.value)
        {
            currentrMap = rMap.value;
            img.color = new Color(currentrMap, currentgMap, currentbMap, .5f);         
        }
        if (currentgMap != gMap.value)
        {
            currentgMap = gMap.value;
            img.color = new Color(currentrMap, currentgMap, currentbMap, .5f);
        }
        if (currentbMap != bMap.value)
        {
            currentbMap = bMap.value;
            img.color = new Color(currentrMap, currentgMap, currentbMap, .5f);
        }

    }
}
