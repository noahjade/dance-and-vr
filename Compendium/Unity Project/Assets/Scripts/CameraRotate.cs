using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraRotate : MonoBehaviour
{
    public float _radius = 10;
    public Vector3 focal = new Vector3(0,0,0);
    public float _angularVelocity = 0.1f;
    private float angle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        updateRadialPosition();
    }

    // Update is called once per frame
    void Update()
    {
        angle = angle + (_angularVelocity*Time.deltaTime);

        updateRadialPosition();

        Vector3 target = focal;

        transform.LookAt(target);
    }

    void updateRadialPosition()
    {
        //Don't change the y axis
        Vector3 initPos = gameObject.transform.position;
        float y = initPos.y;

        //Calculate new x and z
        float x = (float)(_radius*(Math.Cos(angle)));
        float z = (float)(_radius*(Math.Sin(angle)));

        gameObject.transform.position = new Vector3(x, y, z);
    }
}
