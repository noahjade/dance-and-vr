using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;



public class ParticleOptions : MonoBehaviour
{
    //image that the current color of the particles is displayed on
    public Image colorRefImage;

    public LinearMapping gravityMap;
    public LinearMapping startSpeedMap;
    public LinearMapping inheritVelocityMap;

    public LinearMapping lifeTimeMap;

    public LinearMapping minEmissionMap;
    public LinearMapping emissionMultiplierMap;

    private float currentGravityMap = 0.0f;
    private float currentStartSpeedMap = 0.0f;
    private float currentInheritVelocityMap = 0.0f;
    private float currentLifeTimeMap = 0.0f;

    private float currentMinEmissionMap = 0.0f;
    private float currentEmissionMultiplierMap = 0.0f;

    //Max min values for the sliders
    private float minMin = 0.0f;
    private float maxMin = 20.0f;

    private float minMultiplier = 1.0f;
    private float maxMultiplier = 10.0f;

    private float minGravity = 0.0f;
    private float maxGravity = 4.0f;

    private float minStartSpeed = 0.1f;
    private float maxStartSpeed = 0.5f;

    private float minInheritVelocity = 0.0f;
    private float maxInheritVelocity = 6.0f;

    private float minLifeTime = 1.0f;
    private float maxLifeTime = 8.0f;



    ParticleSystem[] pS;
    CustomBodyParticles[] cbP;



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
                p.emMultiplier = scaled;
            }
            
        }

        if (currentGravityMap != gravityMap.value)
        {
            updateGravity();
        }

        if (currentStartSpeedMap != startSpeedMap.value)
        {
            updateStartSpeed();
        }

        if (currentInheritVelocityMap != inheritVelocityMap.value)
        {
            updateInheritVelocity();
        }

        if (currentLifeTimeMap != lifeTimeMap.value)
        {
            updateLifeTime();
        }

    }

    private void updateLifeTime()
    {
        currentLifeTimeMap = lifeTimeMap.value;

        float scaled = scale(0.0f, 1.0f, minLifeTime, maxLifeTime, currentLifeTimeMap);

        //update the gravity for each particle
        foreach (ParticleSystem p in pS)
        {
            var main = p.main;
            main.startLifetime = scaled;
        }
    }

    private void updateGravity()
    {
        currentGravityMap = gravityMap.value;

        float scaled = scale(0.0f, 1.0f, minGravity, maxGravity, currentGravityMap);

        //update the gravity for each particle
        foreach (ParticleSystem p in pS)
        {
            var main = p.main;
            main.gravityModifier = scaled;
        }
    }

    private void updateStartSpeed()
    {
        currentStartSpeedMap = startSpeedMap.value;

        float scaled = scale(0.0f, 1.0f, minStartSpeed, maxStartSpeed, currentStartSpeedMap);

        foreach (ParticleSystem p in pS)
        {
            var main = p.main;
            main.startSpeed = scaled;
        }
    }

    private void updateInheritVelocity()
    {
        currentInheritVelocityMap = inheritVelocityMap.value;

        float scaled = scale(0.0f, 1.0f, minInheritVelocity, maxInheritVelocity, currentInheritVelocityMap);

        //update the gravity for each particle
        foreach (ParticleSystem p in pS)
        {
            var iv = p.inheritVelocity;
            iv.curveMultiplier = scaled;
            //main.gravityModifier = scaled;
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

    public void toggleCollision()
    {
        //boolean now = true;

        

        foreach (ParticleSystem p in pS)
        {
            var c = p.collision;
            c.enabled = !c.enabled;
        }
    }
   
    public void acknowledgeMePlease()
    {
        Debug.Log("Yup! That was acknowledged :)");
    }
}
