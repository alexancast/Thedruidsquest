using UnityEngine;
using System.Collections;
using static UnityEngine.InputSystem.InputAction;
using System.Collections.Generic;

public class TerrainController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform droughtPoint;
    [SerializeField] private TerrainGeneration terrainGeneration;

    [Header("Values")]
    [SerializeField] private Color livingColor, deadColor;
    [SerializeField] private int textureSize;


}