using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpCube : MonoBehaviour
{
    public int experience;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SystemManager.RaiseExperienceEvent(experience);
        }
    }
}
