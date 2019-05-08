using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DrawGrassPatch : MonoBehaviour
{
    public Mesh grassInstance;
    public Material grassMaterial;
    [Range(0f, 1f)]
    public float noiseInterval;
    float prevNoiseInterval;
    [Range(0f, 1f)]
    public float density;
    float prevDensity;
    public float spread;
    float prevSpread;

    List<Matrix4x4> matrices;
    List<Vector3> positions;
    //List<Vector3> rotations;
    List<Vector3> scales;

    // Start is called before the first frame update
    void Start()
    {
        matrices = new List<Matrix4x4>();
        positions = new List<Vector3>();
        //rotations = new List<Vector3>();
        scales = new List<Vector3>();
        ComputeInstanceData();
    }

    void ComputeInstanceData()
    {
        prevNoiseInterval = noiseInterval;
        prevDensity = density;
        prevSpread = spread;
        matrices.Clear();

        int limit = 1023;

        for (float x = 0f; x < 1f; x += noiseInterval)
        {
            for (float z = 0f; z < 1f; z += noiseInterval)
            {
                float sample = Mathf.PerlinNoise(x, z);
                if (sample >= density && matrices.Count < limit)
                {
                    //Debug.Log("add position");
                    Vector3 position = transform.position + new Vector3((x - 0.5f) * spread, 0, (z - 0.5f) * spread);
                    matrices.Add(Matrix4x4.TRS(position, Quaternion.identity, transform.localScale));
                }
            }
        }

        // Let the user know that their settings generated too many instances
        if (matrices.Count == limit)
        {
            Debug.LogWarning("Mesh Instance limit reached, adjust your settings for better results");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Re-compute instance data if parameters have changed
        if (!Mathf.Approximately(noiseInterval, prevNoiseInterval) ||
            !Mathf.Approximately(spread, prevSpread) ||
            !Mathf.Approximately(density, prevDensity))
        {
            ComputeInstanceData();
        }

        Graphics.DrawMeshInstanced(grassInstance, 0, grassMaterial, matrices);
    }
}
