using UnityEngine;
using System.Collections;
using static UnityEngine.InputSystem.InputAction;

public class TerrainController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform playerTransform;

    [Header("Values")]
    [SerializeField] private Color livingColor, deadColor;
    [SerializeField] private LayerMask layerMask;
    [SerializeField, Range(0, 5)] private int steps = 1;
    [SerializeField, Tooltip("The influence of the distance on draught.")] private float distanceEffect = 1;
    public float power = 1;
    public int width = 100;

    private MeshFilter meshFilter;
    private Mesh mesh;
    private Color[] colors;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        colors = new Color[mesh.vertices.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.green;
        }


        mesh.colors = colors;

    }

    public void GenerateAttack(CallbackContext context)
    {

        if (context.started)
        {
            StartCoroutine(SourceEnergy(10));
        }
        else if (context.canceled)
        {
            StopAllCoroutines();
        }
    }


    public IEnumerator SourceEnergy(float castTime)
    {

        Vector2 pos = new Vector2(playerTransform.position.x, playerTransform.position.z);

        int triIndex = RaycastTri(pos);
        int[] verts = GetTriangleIndices(triIndex, steps);

        float elapsedTime = 0;
        Color[] startColors = colors;

        while (elapsedTime < castTime)
        {

            foreach (var item in verts)
            {
                float dist = Vector3.Distance(playerTransform.position, mesh.vertices[item]);
                dist = Mathf.Pow(1/dist, distanceEffect);
                colors[item] = Color.Lerp(startColors[item], deadColor, (elapsedTime / castTime) * dist);

            }
            
            mesh.colors = colors;

            elapsedTime += Time.deltaTime;
            yield return null;
        }


    }

    public int RaycastTri(Vector2 pos)
    {

        RaycastHit hit;

        if (Physics.Raycast(new Vector3(pos.x, 100, pos.y), Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            return hit.triangleIndex;
        }

        return 0;

    }

    public int[] GetTriangleIndices(int triangleIndex, int steps)
    {
        int[] triangles = mesh.triangles;

        // Check if the triangle index is within range
        if (triangleIndex >= 0 && triangleIndex < triangles.Length / 3)
        {
            int vertexIndex = triangleIndex * 3;
            int[] triangleIndices = new int[3];

            triangleIndices[0] = triangles[vertexIndex];
            triangleIndices[1] = triangles[vertexIndex + 1];
            triangleIndices[2] = triangles[vertexIndex + 2];

            if (steps > 0)
            {
                for (int i = 0; i < steps; i++)
                {
                    int[] expandedIndices = new int[triangleIndices.Length * 4];

                    for (int j = 0; j < triangleIndices.Length; j++)
                    {
                        int currentVertexIndex = triangleIndices[j];

                        int[] adjacentIndices = GetAdjacentVertices(currentVertexIndex);
                        for (int k = 0; k < 4; k++)
                        {
                            expandedIndices[j * 4 + k] = adjacentIndices[k];
                        }
                    }

                    triangleIndices = expandedIndices;
                }
            }

            return triangleIndices;
        }
        else
        {
            Debug.LogError("Invalid triangle index!");
            return null;
        }
    }

    private int[] GetAdjacentVertices(int vertexIndex)
    {
        int[] adjacentIndices = new int[4];

        int row = vertexIndex / width;
        int column = vertexIndex % width;

        adjacentIndices[0] = vertexIndex + 1; // Right
        adjacentIndices[1] = vertexIndex - 1; // Left
        adjacentIndices[2] = vertexIndex + width; // Up
        adjacentIndices[3] = vertexIndex - width; // Down

        // Check if the right vertex is within the same row
        if ((row * width + (column + 1)) / width == row)
        {
            adjacentIndices[0] = row * width + (column + 1);
        }
        else
        {
            adjacentIndices[0] = vertexIndex;
        }

        // Check if the left vertex is within the same row
        if ((row * width + (column - 1)) / width == row)
        {
            adjacentIndices[1] = row * width + (column - 1);
        }
        else
        {
            adjacentIndices[1] = vertexIndex;
        }

        // Check if the top vertex is within the grid
        if (vertexIndex + width < mesh.vertexCount)
        {
            adjacentIndices[2] = vertexIndex + width;
        }
        else
        {
            adjacentIndices[2] = vertexIndex;
        }

        // Check if the bottom vertex is within the grid
        if (vertexIndex - width >= 0)
        {
            adjacentIndices[3] = vertexIndex - width;
        }
        else
        {
            adjacentIndices[3] = vertexIndex;
        }

        return adjacentIndices;
    }

}