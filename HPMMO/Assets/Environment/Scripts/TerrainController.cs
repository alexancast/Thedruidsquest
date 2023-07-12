using UnityEngine;
using System.Collections;
using static UnityEngine.InputSystem.InputAction;

public class TerrainController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform playerTransform;

    [Header("Values")]
    [SerializeField] private Color livingColor, deadColor;
    [SerializeField] private int textureSize;

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
        }
    }


    public IEnumerator DrainGround(float castTime) {

        float elapsedTime = 0;

        while (elapsedTime < castTime)
        {

            renderer.material.SetFloat("_Radius", (elapsedTime / castTime) * 10);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }




    public void Update()
    {

        Vector3 pos = new Vector3(playerTransform.position.x + 50, 0, playerTransform.position.z + 50);
        renderer.material.SetVector("_PlayerPos", pos);
    }





}