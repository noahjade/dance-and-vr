using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBodyParticles : MonoBehaviour
{
    // needs to get the object who's velocity we care about
    public Rigidbody rb;

    private Vector3 position;
    private Vector3 newposition;
    private bool firstTime = true;

    private int delayCount =0;  

    // and the particle system who's emmission and colour we want to fuck with
    public ParticleSystem ps;

    //The different gradients to use at different speeds
    private Gradient grad1;
    private Gradient grad2;
    private Gradient grad3;

    // Start is called before the first frame update
    void Start()
    {
       // var col = ps.colorBySpeed;
       // col.enabled = true;

       // Gradient grad = new Gradient();
       // grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.red, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );

        // Initialise the gradients
        //InitialiseGradients();

        position = rb.transform.position;

        //col.color = grad;
        //ps.Stop();
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
            firstTime = false;
        }

        //var col = ps.colorBySpeed;
        //col.enabled = true;
        var em = ps.emission;
        em.enabled = true;
  
        newposition = rb.transform.position;

        //Linear relationship no base
        //em.rateOverTime = Vector3.Distance(position, newposition)*10000 * 2.0f;

        //Linear relationship
        em.rateOverTime = Vector3.Distance(position, newposition)*10000 * 2.0f + 10.0f;

        //expontential relationship
        // 10 is waaay too high per update
        // so is 1


        /* if( Vector3.Distance(position, newposition)*10000 > 12 ) {

            em.rateOverTime = 40.0f;
            //its moving?
            //col.color = grad3;
        } else {
            em.rateOverTime = 10.0f;
            //col.color = grad1;
           // Debug.Log(rb.velocity.magnitude);
           //Debug.Log(Vector3.Distance(position, newposition)*100000); //when we *100000 we get ranges between 0 and ~ 69 max. biggest max was 100+ but that might have been an outlier.
        } */

        //var emission = ParticleSystem.emission;
        position = newposition;
    }


    private void InitialiseGradients(){
        grad1 = new Gradient(); //blue
        grad1.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.blue, 1.0f), new GradientColorKey(Color.red, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );

        grad2 = new Gradient();
        grad2.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.magenta, 0.0f), new GradientColorKey(Color.yellow, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );

        grad3 = new Gradient();
        grad3.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.red, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );

    }
}
