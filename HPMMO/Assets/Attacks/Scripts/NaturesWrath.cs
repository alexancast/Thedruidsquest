using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturesWrath : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float force = 50;
    [SerializeField] private float killTime = 5;

    void Start()
    {
        transform.position += transform.forward * 1;
        //GetComponent<Collider>().isTrigger = false;
        rb.AddForce(transform.forward * force, ForceMode.Acceleration);
        StartCoroutine(KillSwitch());
    }


    public IEnumerator KillSwitch() {

        yield return new WaitForSeconds(killTime);
        Destroy(gameObject);

    }
}
