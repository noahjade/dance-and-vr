using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionAcceleration : MonoBehaviour
{
    public Rigidbody rb;
    public float interpRatio = 0.2f;
    public Vector3 initVel;
    public Vector3 vectorA;
    public Vector3 vectorB;

    private Quaternion q;
    private Vector3 rotVel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = initVel;
        rotVel = vectorA;

        Debug.DrawLine(Vector3.zero, vectorA, Color.white, 100f);
        Debug.DrawLine(Vector3.zero, vectorB, Color.black, 100f);

        //Get normal to vector
        //Vector3 normal = Vector3.Cross(vectorA, vectorB);

        q = Quaternion.FromToRotation(vectorA, vectorB);
        Vector3 euler = q.eulerAngles;
        print("Quaternion: " + q);
        print("Euler: " + euler);

        Vector3 rotatedQ = q*vectorA;
        
        Debug.DrawLine(Vector3.zero, rotatedQ, Color.green, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        rotVel = (Quaternion.Lerp(Quaternion.identity, q, interpRatio*Time.deltaTime))*rotVel;
        rb.velocity = rotVel + initVel;
        //rb.velocity = (q*rb.velocity)*velScaleFactor;
    }
}


    /*
    //Get angle on XY axis:
    Vector3 AXY = new Vector3(vectorA.x, vectorA.y, 0);
    Vector3 BXY = new Vector3(vectorB.x, vectorB.y, 0);
    float angleZ = Vector3.Angle(AXY, BXY);
    print("AXY: " + AXY + ", BXY: " + BXY);

    //Get angle on XZ axis:
    Vector3 AXZ = new Vector3(vectorA.x, 0, vectorA.z);
    Vector3 BXZ = new Vector3(vectorB.x, 0, vectorB.z);
    float angleY = Vector3.Angle(AXZ, BXZ);

    //Get angle on YZ axis:
    Vector3 AZY = new Vector3(0, vectorA.y, vectorA.z);
    Vector3 BZY = new Vector3(0, vectorB.y, vectorB.z);
    float angleX = Vector3.Angle(AZY, BZY);

    print("XY: " + angleZ + ", XZ: " + angleY + ", YZ: " + angleX);

    Debug.DrawLine(Vector3.zero, vectorA, Color.white, 100f);
    Debug.DrawLine(Vector3.zero, vectorB, Color.black, 100f);

    Vector3 ZAligned = Quaternion.Euler(0, 0, -1*angleZ) * vectorA;
    Debug.DrawLine(Vector3.zero, ZAligned, Color.red, 100f);
    float a = Vector3.Angle(new Vector3(ZAligned.x, 0, ZAligned.z), new Vector3(vectorB.x, 0, vectorB.z));
    print("Z aligned angle on Y axis: " + a);
    Vector3 YAligned = Quaternion.Euler(0,a,0)*ZAligned;
    Debug.DrawLine(Vector3.zero, YAligned, Color.yellow, 100f);




    //Vector3 rotatedVectorX =  Quaternion.Euler(angleX, 0, 0) * vectorA;
    //Debug.DrawLine(Vector3.zero, rotatedVectorX, Color.blue, 200f);
    */

    /*
    Debug.DrawLine(Vector3.zero, vectorA, Color.red, 100f);
    Debug.DrawLine(Vector3.zero, vectorB, Color.yellow, 100f);

    Vector3 rotatedVectorX =  Quaternion.Euler(angleX, 0, 0) * vectorA;
    Debug.DrawLine(Vector3.zero, rotatedVectorX, Color.blue, 200f);

    Vector3 rotatedVectorY =  Quaternion.Euler(0, angleY, 0) * vectorA;
    Debug.DrawLine(Vector3.zero, rotatedVectorY, Color.green, 200f);

    Vector3 rotatedVectorZ =  Quaternion.Euler(0, 0, angleX) * vectorA;
    Debug.DrawLine(Vector3.zero, rotatedVectorZ, Color.black, 200f);

    Vector3 rotatedVectorTotal =  Quaternion.Euler(0, angleY, angleX) * vectorA;
    Debug.DrawLine(Vector3.zero, rotatedVectorTotal, Color.white, 200f);
    */
