using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidalMovement : MonoBehaviour
{
    private Vector3 _startPosition;
    public float velScale = 2.0f;
    
    void Start (){
        _startPosition = transform.position;
    }
    
    void FixedUpdate(){
        transform.position = _startPosition + new Vector3(Mathf.Sin(Time.time)*velScale, Mathf.Cos(Time.time)*velScale, 0.0f);
    }
}
