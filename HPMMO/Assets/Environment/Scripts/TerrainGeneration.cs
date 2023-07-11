using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainGeneration : MonoBehaviour
{

    public NoiseVisualiser noiseVisualiser;

    

    public int size = 10;
    public float scale = 1f;
    public float amplitude = 100;
    private Mesh terrainMesh;


    public void UpdateColor() {

        terrainMesh = GetComponent<MeshFilter>().sharedMesh;

        Color[] colors = new Color[terrainMesh.vertices.Length];

        for (int i = 0; i < terrainMesh.vertices.Length; i++)
        {
            Vector3 normal = terrainMesh.normals[i];
            float dot = Vector3.Dot(normal, Vector3.up);
            Debug.Log(dot);
            colors[i] = Color.green * dot;
            terrainMesh.colors[i] = colors[i];
        }
    }

    public void UpdateHeights() {

        
        terrainMesh = GetComponent<MeshFilter>().sharedMesh;

        int vertexIndex = 0;

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                
                Vector3 vertex = terrainMesh.vertices[vertexIndex];
                terrainMesh.vertices[vertexIndex].Set(vertex.x, 100, vertex.z);

                vertexIndex++;
            }
        }
    }


    public void GenerateTerrain()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        terrainMesh = new Mesh();
        Texture2D noiseTexture = noiseVisualiser.GetTexture();


        Vector3[] vertices = new Vector3[size * size];
        int[] triangles = new int[(size - 1) * (size - 1) * 6];
        Vector2[] uv = new Vector2[size * size];

        int vertexIndex = 0;
        int triangleIndex = 0;

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                float noise = noiseTexture.GetPixelBilinear((float)x / (size - 1), (float)z / (size - 1)).r;
                noise *= amplitude;

                Vector3 vertex = new Vector3(x - size * 0.5f, noise, z - size * 0.5f) * scale;

                vertices[vertexIndex] = vertex;
                uv[vertexIndex] = new Vector2((float)x / size, (float)z / size);

                if (x != size - 1 && z != size - 1)
                {
                    triangles[triangleIndex] = vertexIndex;
                    triangles[triangleIndex + 1] = vertexIndex + size;
                    triangles[triangleIndex + 2] = vertexIndex + size + 1;
                    triangles[triangleIndex + 3] = vertexIndex;
                    triangles[triangleIndex + 4] = vertexIndex + size + 1;
                    triangles[triangleIndex + 5] = vertexIndex + 1;
                    triangleIndex += 6;
                }

                vertexIndex++;
            }
        }

        terrainMesh.vertices = vertices;
        terrainMesh.triangles = triangles;
        terrainMesh.uv = uv;
        terrainMesh.RecalculateNormals();

        meshCollider.sharedMesh = terrainMesh;
        meshFilter.sharedMesh = terrainMesh;
    }
}
