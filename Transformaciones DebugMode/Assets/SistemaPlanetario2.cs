using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SistemaPlanetario2 : MonoBehaviour
{
    public Mesh model;

    public Material mTierra;
    public Material mLuna;
    public Material mEstrella;

    public int numTierras = 3;

    public float rotacionT = 30;
    public float distanciaT = 1;
    public float escalaT = 0.5f;
    public float separacionT = 2;

    public float velocidadRotacionT = 30.0f;

    public float rotacionL = 60;
    public float distanciaL = 1;
    public float escalaL = 0.2f;

    public float velocidadRotacionL = 30.0f;

    public int numEstrellas = 200;      
    
    public float anchoCampoEstrellas = 300;
    public float altoCampoEstrellas = 200;
    public float distanciaCampoEstrellas = 100;
    public float velocidadEscalaEstrella = 0.1f;

    Vector3[] posicionesEstrellas;
    Matrix4x4[] matricesEstrellas;
    Matrix4x4[] matricesEstrellasEscaladas;

    float[] escalasEstrellas;
    float escalaEstrellas = 0;

    // Start is called before the first frame update
    void Start()
    {
        posicionesEstrellas = new Vector3[numEstrellas];
        matricesEstrellas = new Matrix4x4[numEstrellas];
        escalasEstrellas = new float[numEstrellas];
        matricesEstrellasEscaladas = new Matrix4x4[numEstrellas];

        for (int i = 0; i < numEstrellas; i++)
        {
            posicionesEstrellas[i] = new Vector3(
                                            UnityEngine.Random.Range(-anchoCampoEstrellas / 2, anchoCampoEstrellas / 2),
                                            UnityEngine.Random.Range(-altoCampoEstrellas / 2, altoCampoEstrellas / 2),
                                            distanciaCampoEstrellas);

            matricesEstrellas[i] = Matrix4x4.Translate(posicionesEstrellas[i]);
            escalasEstrellas[i] = UnityEngine.Random.Range(1, 2.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 MSol = transform.localToWorldMatrix;

        for (int i = 0; i < numTierras; i++)
        {
            Matrix4x4 MTierra = MSol * Matrix4x4.Rotate(Quaternion.Euler(0, 0, rotacionT + i * 360.0f / numTierras))
                * Matrix4x4.Translate(new Vector3(distanciaT + i * separacionT, 0, 0))
                * Matrix4x4.Scale(new Vector3(escalaT, escalaT, escalaT));

            Graphics.DrawMesh(model, MTierra, mTierra, 0);

            Matrix4x4 MLuna = MTierra * Matrix4x4.Rotate(Quaternion.Euler(0, 0, rotacionL))
                * Matrix4x4.Translate(new Vector3(distanciaL, 0, 0))
                * Matrix4x4.Scale(new Vector3(escalaL, escalaL, 1));

            Graphics.DrawMesh(model, MLuna, mLuna, 0);
                       
        }

        rotacionT += velocidadRotacionT * Time.deltaTime;
        rotacionL += velocidadRotacionL * Time.deltaTime;

        for (int i = 0; i < numEstrellas; i++)
        {
            float s = escalasEstrellas[i] * escalaEstrellas;
            matricesEstrellasEscaladas[i] = matricesEstrellas[i] * Matrix4x4.Scale(new Vector3(s, s, 1));
        }

        Graphics.DrawMeshInstanced(model, 0, mEstrella, matricesEstrellasEscaladas);

        escalaEstrellas += velocidadEscalaEstrella * Time.deltaTime;

        if (escalaEstrellas > 1.0f)
        {
            escalaEstrellas = 0;
        }
    }
}
