using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroughtManager : MonoBehaviour
{
    public static Vector4[] points = new Vector4[10];
    public static int pointsAmount = 0;

    private static Renderer terrainRenderer;

    public void Start()
    {
        terrainRenderer = FindObjectOfType<TerrainController>().GetComponent<Renderer>();
    }

    public static void AddPoint() {

        //Temp, overwrites array to not overflow.
        if (pointsAmount < points.Length - 1)
        {
            pointsAmount++;

        }
        else {

            pointsAmount = 0;
        }

    }

    public void Update()
    {
        terrainRenderer.material.SetVectorArray("droughtPoints", points);
    }
}
