using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroughtPoint : MonoBehaviour
{
    [SerializeField] private float killTime = 10;
    [SerializeField] private float expandTime = 3;

    private int index;
    private float targetRadius;
    private float strength = 0;
    private bool expanding;
    private bool despanding;
    private float elapsedTime = 0;
    private float radius;

    private float halfTerrainScale;

    public void Setup(float radius)
    {
        halfTerrainScale = FindObjectOfType<TerrainGeneration>().size / 2;

        targetRadius = radius;
        index = DroughtManager.pointsAmount;
        DroughtManager.points[index] = new Vector4(transform.position.x + halfTerrainScale, transform.position.z + halfTerrainScale, 0, strength);

        DroughtManager.AddPoint();

        expanding = true;
    }


    public void Update()
    {

        if (expanding)
        {
            if (elapsedTime < expandTime)
            {
                radius = Mathf.Lerp(0, targetRadius, elapsedTime / expandTime);

                DroughtManager.points[index] = new Vector4(transform.position.x + halfTerrainScale, transform.position.z + halfTerrainScale, radius, strength);
                elapsedTime += Time.deltaTime;

            }
            else
            {
                expanding = false;
                elapsedTime = 0;
            }
        }
        else if (despanding)
        {

            if (elapsedTime < expandTime)
            {

                radius = Mathf.Lerp(0, targetRadius, 1 - (elapsedTime / expandTime));

                DroughtManager.points[index] = new Vector4(transform.position.x + halfTerrainScale, transform.position.z + halfTerrainScale, radius, strength);

                elapsedTime += Time.deltaTime;
            }
            else
            {
                DroughtManager.points[index] = new Vector4(0, 0, 0, strength);
                Destroy(gameObject);
            }
        }
        else
        {

            if (elapsedTime < killTime)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                despanding = true;
                elapsedTime = 0;
            }
        }


    }


    public float GetStrength()
    {
        return strength;
    }

}
