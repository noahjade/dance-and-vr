using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour
{

    Renderer renderer;
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer> ();
        mat = renderer.material;
        mat.EnableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update () {
 

        //  float emission = Mathf.PingPong (Time.time, 1.0f);
        //  Color baseColor = Color.yellow; //Replace this with whatever you want for your base color at emission level '1'
 
        //  Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
 
        //  mat.SetColor ("_EmissionColor", finalColor);
    }
}
