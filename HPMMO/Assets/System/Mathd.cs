using UnityEngine;

public static class Mathd
{
    public static float Remap(float iMin, float iMax, float oMin, float oMax, float v) {

        float t = Mathf.InverseLerp(iMin, iMax, v);
        return Mathf.Lerp(oMin, oMax, t);
    }

}
