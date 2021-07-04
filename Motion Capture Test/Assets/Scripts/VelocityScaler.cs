using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityScaler : MonoBehaviour
{

    public int _arraySize = 20;
    public float _maxAcceleration = 0.1f;
    public float _scaleVariable = 0.5f;

    private float[] _speedArray;
    private float _speed;
    private Vector3 _lastPos;
    private Vector3 _initScale;
    private Transform _tf;

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
        
        //Update the scale of the transform
        gameObject.transform.localScale = new Vector3(_initScale.x, lastAvg1*_scaleVariable, _initScale.z);

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
