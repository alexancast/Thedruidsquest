using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class SpellBar : MonoBehaviour
{

    private Material mat;

    public void Start()
    {
        mat = GetComponent<RawImage>().material;
        
    }


    public void Cast(CallbackContext context) {


        if (context.started)
        {
            GetComponent<RawImage>().enabled = true;
            StartCoroutine(CastSpell(10));
        }
        else if (context.canceled)
        {
            StopAllCoroutines();
            GetComponent<RawImage>().enabled = false;
        }


    }

    public IEnumerator CastSpell(float castTime) {

        float elapsedTime = 0;

        while (elapsedTime < castTime)
        {
            mat.SetFloat("_FillAmount", elapsedTime / castTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }


}
