using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LiveMeshGenerator : MonoBehaviour
{
    public Gradient gradient;

	public int mapWidth;
	public int mapHeight;
	public float noiseScale;

	public int octaves;

	[Range(0,1)]
	public float minPersistance;
    public float maxPersistance;

	public float minLacunarity;
    public float maxLacunarity;

    public float minAltitude;
    public float maxAltitude;


	public int seed;
	public Vector2 offset;

    private Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Color[] colours;


    //STUFF FOR SPEED
    [Range (0,100)]
    public float maxSpeed = 1f;

    [Range (0,100)]
    public float minSpeed = 0f;

    [Range (0,100)]
    public float maxAcceleration = 1f;

    [Range (0,100)]
    public float minAcceleration= 0f;

    [Range (0,100)]
    public float maxHeight = 1f;

    [Range (0,100)]
    public float minHeight= 0f;

    public float delta = 0.1f;

    private float heightRatio;
    private float prevHeightRatio;

    private float speedRatio;
    private float prevSpeedRatio;

    private float accelerationRatio;
    private float prevAccelerationRatio;

    public VelocityTracker[] velTrackList;

    public float _flatRad = 5f;
    public float _fullRad = 7f;

    // Start is called before the first frame update
    void Start()
    {
        if(seed == 0){
            seed = (int)System.DateTime.Now.Ticks;
        }

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Update(){

        float speedSum = 0f;
        float accSum = 0f;
        float heightSum = 0f;

        foreach (var velTrack in velTrackList)
        {
            speedSum = speedSum + velTrack.getSpeed();
            accSum = accSum + velTrack.getAcceleration();
            heightSum = heightSum + velTrack.getHeight();
        }

        float speed = speedSum/(velTrackList.Length); //get average speed
        float acceleration = accSum/(velTrackList.Length); //get average acc
        float height = heightSum/(velTrackList.Length); //get average acc

        speedRatio = Mathf.InverseLerp(minSpeed, maxSpeed, speed);
        accelerationRatio = Mathf.InverseLerp(minAcceleration, maxAcceleration, acceleration);
        heightRatio = Mathf.InverseLerp(minHeight, maxHeight, height);

        //Smooth out transitions using delta
        if(speedRatio > prevSpeedRatio){
            float maxRatio = prevSpeedRatio + delta*Time.deltaTime;
            speedRatio = Math.Min(maxRatio, speedRatio);
        } else if (speedRatio < prevSpeedRatio){
            float minRatio = prevSpeedRatio - delta*Time.deltaTime;
            speedRatio = Math.Max(minRatio, speedRatio);
        }

        if(accelerationRatio > prevAccelerationRatio){
            float maxRatio = prevAccelerationRatio + delta*Time.deltaTime;
            accelerationRatio = Math.Min(maxRatio, accelerationRatio);
        } else if (accelerationRatio < prevAccelerationRatio){
            float minRatio = prevAccelerationRatio - delta*Time.deltaTime;
            accelerationRatio = Math.Max(minRatio, accelerationRatio);
        }

        if(heightRatio > prevHeightRatio){
            float maxRatio = prevHeightRatio + delta*Time.deltaTime;
            heightRatio = Math.Min(maxRatio, heightRatio);
        } else if (heightRatio < prevHeightRatio){
            float minRatio = prevHeightRatio - delta*Time.deltaTime;
            heightRatio = Math.Max(minRatio, heightRatio);
        }

        CreateShape();

        UpdateMesh();

        prevSpeedRatio = speedRatio;
        prevAccelerationRatio = accelerationRatio;
        prevHeightRatio = heightRatio;
    }

	public void CreateShape() {
        float speedLac = Mathf.Lerp(minLacunarity, maxLacunarity, speedRatio);
        float accPersistance = Mathf.Lerp(minPersistance, maxPersistance, accelerationRatio);
        float heightScale = Mathf.Lerp(minAltitude, maxAltitude, heightRatio);

		float[,] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, seed, noiseScale, octaves, accPersistance, speedLac, offset);
        float[,] radiusMap = CircularFlatten(noiseMap);
        CalculateColours(radiusMap);

		MeshData meshData = MeshGenerator.GenerateTerrainMesh (radiusMap, heightScale);
        triangles = meshData.triangles;
        vertices = meshData.vertices;
		
	}

    void UpdateMesh(){
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colours;

        mesh.RecalculateNormals();
    }

	void OnValidate() {
		if (mapWidth < 1) {
			mapWidth = 1;
		}
		if (mapHeight < 1) {
			mapHeight = 1;
		}
		if (minLacunarity < 1) {
			minLacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
	}

    private float[,] CircularFlatten(float[,] noiseMap){

        float[,] output = new float[mapWidth,mapHeight];

        for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

                float height = noiseMap[x,y];
                var radDist = Math.Pow((Math.Pow(x-mapWidth/2, 2) + Math.Pow(y-mapHeight/2,2)), 0.5f); //centered around xSize/2, zSize/2

                if(radDist < _flatRad){ //if point is within the flat radius
                    height = 0;
                } else if((_flatRad <= radDist) && (radDist < _fullRad)){ //else if in between flat and full radius, increase to max height with a gradient
                    var scalar = 1 - (_fullRad - radDist)/(_fullRad-_flatRad);
                    height = (float) scalar*height;
                }

                output[x,y] = height;
            }
        }

        return output;
    }

    private void CalculateColours(float[,] noiseMap){

        colours = new Color[mapWidth*mapHeight];

        for (int i = 0, y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

                colours[i] = gradient.Evaluate(noiseMap[x,y]);
                i++;
            }
        }
    }
}
