using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AsteroidField : MonoBehaviour
{
    public Mesh model;

    public Material sunMaterial;
    public Material asteroidMaterial;

    public float sunSize;

    public int numAsteroids = 100;

    public float asteroidDistance = 100;
    public float asteroidDistanceVariation = 20;
    public float asteroidAngularSpeed = 20;
    public float asteroidAngularSpeedVariation = 5;
    public float asteroidSize = 5;
    public float asteroidSizeVariation = 3;
    public float asteroidRotationSpeed = 5;
    public float asteroidRotationSpeedVariation = 3;

    float[] asteroidAngularSpeeds;
    float[] asteroidDistances;
    float[] asteroidSizes;
    float[] asteroidRotationSpeeds;

    float[] asteroidAngularPositions;
    float[] asteroidRotations;

    Matrix4x4[] asteroidMatrices;

    Matrix4x4[][] asteroidPackedMatrices;
    int numPacks;

    CommandBuffer commands;


    public bool regenerate;


    // Start is called before the first frame update
    void Start()
    {
        Regenerate();


    }

    // Update is called once per frame
    void Update()
    {
        if(regenerate)
        {
            Regenerate();
        }

        for (int i = 0; i < numAsteroids; i++)
        {

            asteroidMatrices[i] = transform.localToWorldMatrix * Matrix4x4.Rotate(Quaternion.Euler(0, asteroidAngularPositions[i], 0)) *
                                    Matrix4x4.Translate(new Vector3(asteroidDistances[i], 0, 0)) * Matrix4x4.Rotate(Quaternion.Euler(0, -asteroidAngularPositions[i], asteroidRotations[i])) *
                                    Matrix4x4.Scale(Vector3.one * asteroidSizes[i]);

            asteroidPackedMatrices[i / 1023][i % 1023] = asteroidMatrices[i];


            asteroidAngularPositions[i] += asteroidAngularSpeeds[i] * Time.deltaTime;
            asteroidRotations[i] += asteroidRotationSpeeds[i] * Time.deltaTime;

        }

        Matrix4x4 sunMatrix = transform.localToWorldMatrix * Matrix4x4.Scale(Vector3.one * sunSize);

        commands = new CommandBuffer();

        commands.Clear();
        commands.DrawMesh(model, sunMatrix, sunMaterial);
        for(int i = 0; i < numPacks; i ++)
        {
            commands.DrawMeshInstanced(model, 0, asteroidMaterial, -1, asteroidPackedMatrices[i]);
        }

        regenerate = false;
    }

    void OnRenderObject()
    {
        Graphics.ExecuteCommandBuffer(commands);
    }

    void Regenerate()
    {
        asteroidAngularSpeeds = new float[numAsteroids];
        asteroidDistances = new float[numAsteroids];
        asteroidSizes = new float[numAsteroids];
        asteroidRotationSpeeds = new float[numAsteroids];
        asteroidAngularPositions = new float[numAsteroids];
        asteroidRotations = new float[numAsteroids];

        asteroidMatrices = new Matrix4x4[numAsteroids];

        numPacks = numAsteroids / 1023 + (numAsteroids % 1023 == 0 ? 0 : 1);
        asteroidPackedMatrices = new Matrix4x4[numPacks][];
        for(int i = 0; i < numPacks; i ++) { asteroidPackedMatrices[i] = new Matrix4x4[1023]; }


        for (int i = 0; i < numAsteroids; i++)
        {
            asteroidAngularSpeeds[i] = asteroidAngularSpeed + UnityEngine.Random.Range(-asteroidAngularSpeedVariation / 2, asteroidAngularSpeedVariation / 2);
            asteroidDistances[i] = asteroidDistance + UnityEngine.Random.Range(-asteroidDistanceVariation / 2, asteroidDistanceVariation / 2);
            asteroidSizes[i] = asteroidSize + UnityEngine.Random.Range(-asteroidSizeVariation / 2, asteroidSizeVariation / 2);
            asteroidRotationSpeeds[i] = asteroidRotationSpeed + UnityEngine.Random.Range(-asteroidRotationSpeedVariation / 2, asteroidRotationSpeedVariation / 2);

            asteroidAngularPositions[i] = UnityEngine.Random.Range(0.0f, 360.0f);
            asteroidRotations[i] = UnityEngine.Random.Range(0.0f, 360.0f);

        }

    }
}
