using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoiseVisualiser))]
public class NoiseVisualiserEditor : Editor
{

    NoiseVisualiser noise;

    public void OnEnable()
    {
        noise = (NoiseVisualiser)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate"))
        {
            noise.SetNoiseTexture();
        }

        if (GUILayout.Button("Randomize"))
        {
            noise.SetRandomCoord();
        }
    }

}
