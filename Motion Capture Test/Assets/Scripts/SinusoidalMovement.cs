using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidalMovement : MonoBehaviour
{
    private Vector3 _startPosition;

    [Range (0.1f, 10f)]
    public float velScale = 1.0f;
    
    void Start (){
        _startPosition = transform.position;
    }
    
    void FixedUpdate(){
        transform.position = _startPosition + (new Vector3(Mathf.Sin(Time.time*velScale), 0.0f, 0.0f));
    }
}
