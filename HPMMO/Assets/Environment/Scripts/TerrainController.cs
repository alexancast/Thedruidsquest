using UnityEngine;
using System.Collections;
using static UnityEngine.InputSystem.InputAction;
using System.Collections.Generic;

public class TerrainController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform draughtPoint;
    [SerializeField] private TerrainGeneration terrainGeneration;

    [Header("Values")]
    [SerializeField] private Color livingColor, deadColor;
    [SerializeField] private int textureSize;

    public List<Vector4> draughtPoints = new List<Vector4>();

    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        
    }

    public void GenerateAttack(CallbackContext context)
    {

        if (context.started)
        {
            StartCoroutine(DrainGround(10));
        }
        else if (context.canceled)
        {
            StopAllCoroutines();

            //Vector3 draughtPoint = playerTransform.position;
            //draughtPoints.Add(draughtPoint);
        }
    }


    public IEnumerator DrainGround(float castTime) {

        float elapsedTime = 0;

        draughtPoint.position = playerTransform.position + new Vector3(50, 0, 50); //Accounts for the offset in the shader, not a very nice way of doing this but anyway.

        while (elapsedTime < castTime)
        {

            renderer.material.SetFloat("_Radius", (elapsedTime / castTime) * 10);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }




    public void Update()
    {
        renderer.material.SetVector("_PlayerPos", draughtPoint.position);
    }





}