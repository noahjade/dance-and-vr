using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ParticleOptions : MonoBehaviour
{
    //image that the current color of the particles is displayed on
    public Image colorRefImage;

    //All the instances of the customisable particles.
    //Though, I guess I want them to be particle systems....

    //Object.FindObjectsOfType
    //
    ParticleSystem[] pS;

    
    

    // Start is called before the first frame update
    void Start()
    {
        
        // First, get all the particles in the scene.
        // Note: if in the furture the scene includes more particles systems than just the mover, this will have to be altered to filter by tag.
        pS = FindObjectsOfType<ParticleSystem>();

        // debug the number of the particle systems that were found so we can easily spot weird behavior
        Debug.Log("number of particle systems: " + pS.Length);
        
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
