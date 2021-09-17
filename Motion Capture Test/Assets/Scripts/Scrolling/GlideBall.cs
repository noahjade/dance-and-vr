using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideBall : MonoBehaviour
{
    public float killTimer = 2f;
    private SinusoidalMovement sine;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        sine = gameObject.AddComponent(typeof(SinusoidalMovement)) as SinusoidalMovement;
        setKillTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setKillTimer(){
        StartCoroutine(kill(killTimer));
    }

    IEnumerator kill(float time){
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }


}
