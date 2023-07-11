using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGeneration))]
public class TerrainGenerationEditor : Editor
{
    private TerrainGeneration generation;

    public void OnEnable()
    {
        generation = (TerrainGeneration)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate Mesh"))
            {
                generation.GenerateTerrain();
            }

        GUILayout.EndHorizontal();
        
    }
}
