using System;
using UnityEngine;

public class VelocityColour : MonoBehaviour
{

    public int _arraySize = 20;
    public float _maxSpeed = 5;
    
    //Colours

    public int _r1 = 0;
    public int _g1 = 0;
    public int _b1 = 0;
    public int _i1 = 255;

    public int _r2= 255;
    public int _g2= 100;
    public int _b2= 100;
    public int _i2= 255;

    //Transforms

    private float[] _speedArray;
    private float _speed;
    private Vector3 _lastPos;
    private Vector3 _initScale;
    private Transform _tf;
    private Renderer _renderer;

    private float lastAvg1 = 0.0f;
    private float lastAvg2 = 0.0f;

    public GameObject _referenceObj;

    void Start()
    {
        //Get the transform and position of transform
        _tf = _referenceObj.GetComponent<Transform>();
        _lastPos = _tf.position;

        _initScale = gameObject.transform.localScale;
        
        //Initialise the speed array
        _speedArray = new float[_arraySize];
        initialiseArray(_speedArray, 0.0f);

        //initialise averages
        float average = Average(_speedArray);
        lastAvg1 = average;
        lastAvg2 = average;

        //Get the Renderer component
        _renderer = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        //Get new speed
        _speed = (_tf.position - _lastPos).magnitude;
        _speed /= Time.deltaTime;
        _lastPos = _tf.position;
        
        //Update the speed array for averaging
        updateArray(_speedArray, _speed);

        float average = Average(_speedArray);

        //Correct sudden fluctuations
        if(((lastAvg2 < lastAvg1) && (average < lastAvg1)) || ((lastAvg2 > lastAvg1) && (average > lastAvg1))){
            lastAvg1 = (lastAvg2 + average)/2;
        }

        //Ratio of speed to max speed
        float ratio = Math.Min(1, (lastAvg1/_maxSpeed));

        float r = (_r1 + (_r2 - _r1)*ratio)/255;
        float g = (_g1 + (_g2 - _g1)*ratio)/255;
        float b = (_b1 + (_b2 - _b1)*ratio)/255;
        float intensity = (_i1 + (_i2 - _i1)*ratio)/255;

        //print("R,g,b,i: " + r + ", "+ g + ", "+ b + ", "+ intensity);

        Color col = new Color(r,g,b,intensity);

        //Call SetColor using the shader property name "_Color"
        _renderer.material.SetColor("_Color",  col);

        //Update the last speed values
        lastAvg2 = lastAvg1;
        lastAvg1 = average;

    }

    void initialiseArray(float[] arrayIn, float initValue)
    {
        for(int i = 0; i < arrayIn.Length; i++){
            arrayIn[i] = initValue;
        }
    }

    void updateArray(float[] arrayIn, float nextValue){
        float prev = nextValue;
        float temp;

        for(int i = 0; i < arrayIn.Length; i++){
            temp = arrayIn[i];
            arrayIn[i] = prev;
            prev = temp;
        }
    }

    float Average(float[] array){

        float sum = 0.0f;

        for(int i =0; i < array.Length; i++){
            sum += array[i];
        }

        return(sum/array.Length);
    }
}
