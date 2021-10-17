using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBodyParticles : MonoBehaviour
{

    //variables that we want to be able to adjust from within app
    public float minEmission = 10.0f;
    public float emMultiplier = 2.0f; //always x10000 so its noticeable.

    // needs to get the object who's velocity we care about
    public Rigidbody rb;

    private Vector3 position;
    private Vector3 newposition;


    private int delayCount =0;  

    // and the particle system who's emmission and colour we want to mess with
    public ParticleSystem ps;

    //The different gradients to use at different speeds
    private Gradient grad1;
    private Gradient grad2;
    private Gradient grad3;

    // Start is called before the first frame update
    void Start()
    {
        position = rb.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // want to set the particle emission rate relative to the velocity
        // also the colour since thats easier to see.
        //Vector3 velocity = rb.velocity;
        if(delayCount < 20){
            position = rb.transform.position;
            delayCount = delayCount + 1;
            
        }

        var em = ps.emission;
        em.enabled = true;
  
        newposition = rb.transform.position;

        //Linear relationship
        em.rateOverTime = Vector3.Distance(position, newposition)*10000 * emMultiplier + minEmission;

        position = newposition;
    }

}
