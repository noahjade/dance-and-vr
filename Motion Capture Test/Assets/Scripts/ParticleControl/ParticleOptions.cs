using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;



public class ParticleOptions : MonoBehaviour
{
    //image that the current color of the particles is displayed on
    public Image colorRefImage;

    public LinearMapping minEmissionMap;
    public LinearMapping emissionMultiplierMap;

    private float currentMinEmissionMap = 0.0f;
    private float currentEmissionMultiplierMap = 0.0f;
    private float minMin = 0.0f;
    private float maxMin = 20.0f;
    private float minMultiplier = 1.0f;
    private float maxMultiplier = 10.0f;

    //All the instances of the customisable particles.
    //Though, I guess I want them to be particle systems....

    ParticleSystem[] pS;

    CustomBodyParticles[] cbP;

    //Okay. Honestly I'm going to refactor so all particle control is here, including movement-velocity stuff.




    // Start is called before the first frame update
    void Start()
    {
        


        // First, get all the particles in the scene.
        // Note: if in the furture the scene includes more particles systems than just the mover, this will have to be altered to filter by tag.
        pS = FindObjectsOfType<ParticleSystem>();

        // extremely scuffed way of getting all the custombodyparticles lol
        cbP = new CustomBodyParticles[pS.Length];
        int i = 0;
        foreach(ParticleSystem p in pS)
        {
            CustomBodyParticles[] bb = p.gameObject.GetComponents<CustomBodyParticles>();
            if(bb.Length > 0)
            {
                cbP[i] = bb[0];
            }
            i++;
        }
        
        
        // debug the number of the particle systems that were found so we can easily spot weird behavior
        Debug.Log("number of particle systems: " + pS.Length);
        Debug.Log("number of custom bps: " + cbP.Length);

    }

    void Update()
    {
        //Tracks whether the sliders have changed.
        if (currentMinEmissionMap != minEmissionMap.value)
        {
            currentMinEmissionMap = minEmissionMap.value;

            //this value needs to be scaled so that its not just between 0 and 1.
            float scaled = scale(0.0f, 1.0f, minMin, maxMin, currentMinEmissionMap);     

            // update the variables in the CustomBodyParticles Script.
            foreach (CustomBodyParticles p in cbP)
            {
                p.minEmission = scaled;
            }

            

                //img.color = new Color(currentrMap, currentgMap, currentbMap, .5f);
        }

        if (currentEmissionMultiplierMap != emissionMultiplierMap.value)
        {
            
            currentEmissionMultiplierMap = emissionMultiplierMap.value;

            //this value needs to be scaled so that its not just between 0 and 1.
            float scaled = scale(0.0f, 1.0f, minMultiplier, maxMultiplier, currentEmissionMultiplierMap);

            // update the variables in the CustomBodyParticles Script.
            foreach (CustomBodyParticles p in cbP)
            {
                p.emMultiplier = currentEmissionMultiplierMap;
            }
            
        }

    }

    //helper function
    private float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
        return (NewValue);
    }

    public void toggleEnable(GameObject objectTag)
    {
        if(objectTag.activeInHierarchy)
        {
            objectTag.SetActive(false);           
        } else
        {
            objectTag.SetActive(true);
        }
    }

    public void toggleButtonColor(Image butt)
    {
        
        if (butt.color == Color.red)
        {
            butt.color = Color.green;
        }
        else
        {
            butt.color = Color.red;
        }
    }


    // UI elements trigger these public functions

    //random color
    public void setParticleColor()
    {
        Color color = new Color(Random.Range(0.0f,1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), .5f);
        colorRefImage.color = color;

        foreach (ParticleSystem p in pS)
        {
            //p.startColor = color;
            ParticleSystem.MainModule settings = p.main;
            settings.startColor = new ParticleSystem.MinMaxGradient(color);
        }
    }

    //selected color
    public void setSelectedParticleColor(Image img)
    {
        Color color = img.color;
        colorRefImage.color = color;

        foreach (ParticleSystem p in pS)
        {
            //p.startColor = color;
            ParticleSystem.MainModule settings = p.main;
            settings.startColor = new ParticleSystem.MinMaxGradient(color);
        }
    }

    public void acknowledgeMePlease()
    {
        Debug.Log("Yup! That was acknowledged :)");
    }
}
