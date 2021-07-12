using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritParentMotion : MonoBehaviour
{

    public ParticleSystem _ps;
    public Transform _tf;
    public const int _arraySize = 5;
    public float _accMultiplier = 1f;

    private float[] _aX;
    private float[] _aY;
    private float[] _aZ;
    private Vector3 _lastPos;
    private Vector3 _lastVelocity;
    private int count = 0;

    private List<CustomParticle> _list;

    // Start is called before the first frame update
    void Start()
    {
        _list = new List<CustomParticle>();
        _lastPos = _tf.position;
        _lastVelocity = new Vector3(0,0,0);

        //Initialise average arrays
        _aX= new float[_arraySize];
        _aY= new float[_arraySize];
        _aZ= new float[_arraySize];

        initialiseArray(_aX, 0);
        initialiseArray(_aX, 0);
        initialiseArray(_aX, 0);
    }

    void FixedUpdate()
    {
        //Update average acceleration in each direction
        Vector3 currVel = _tf.position - _lastPos;
        Vector3 currAcc = currVel - _lastVelocity;

        updateArray(_aX, currAcc.x);
        updateArray(_aY, currAcc.x);
        updateArray(_aZ, currAcc.x);

        /*
        float aX = Average(_aX);
        float aY = Average(_aX);
        float aZ = Average(_aX);
        */

        Vector3 avgAcc = new Vector3(Average(_aX), Average(_aY), Average(_aZ));

        if(count >= 5){
            //Apply this acceleration to new particles
            _list.Add(new CustomParticle(50, currVel, avgAcc*_accMultiplier, _tf.position));
            count = 0;
        }
        count += 1;
        

        //Update all particles in a list
        for (int i = _list.Count - 1; i >= 0; i--)
        {
            _list[i].Tick(); //accelerate particle

            //Remove particles after a time
            if (_list[i].getLifetime() <= 0) {
                _list[i].setKillTimer();
                _list.RemoveAt(i);
            }
        }
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

public class CustomParticle : MonoBehaviour{

    public const float _forceMultiplier = 0.1f;

    private GameObject _sphere;
    private int _lifetime;
    private Vector3 _acceleration;
    private Vector3 _velocity;
    private Rigidbody _rb;

    public CustomParticle(int lifetime, Vector3 initialVelocity, Vector3 acceleration, Vector3 initialPosition){
        _lifetime = lifetime;
        _acceleration = acceleration;
        _velocity = initialVelocity;
        

        //initialise sphere
        _sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _rb = _sphere.AddComponent<Rigidbody>();
        _rb.useGravity = false;
        _sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        _sphere.transform.position = initialPosition;
        //_rb.velocity = _velocity;
    }

    public void setKillTimer(){
        Invoke("kill", 2.0f);
    }

    private void kill(){
        Destroy(_sphere);
    }

    public void Tick(){
        _lifetime = _lifetime - 1;
        _rb.velocity = _rb.velocity + _acceleration*_forceMultiplier;
    }

    public int getLifetime(){
        return _lifetime;
    }
}
