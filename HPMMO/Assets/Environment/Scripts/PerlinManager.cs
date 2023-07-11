using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinManager
{


    public static float GetHeight(float x, float z) {

        return Mathf.PerlinNoise(x, z);
    }
    
}
