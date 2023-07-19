using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.InputSystem.InputAction;

public class SpellBar : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;

    private Material mat;

    public void Start()
    {
        mat = GetComponent<RawImage>().material;
        
    }

    public void OnEnable()
    {
        SystemManager.OnSpellCastInitiated += InitiateCast;
        SystemManager.OnSpellCastAbrupted += AbruptCast;
        SystemManager.OnSpellCastFinished += AbruptCast;
    }


    public void OnDisable()
    {
        SystemManager.OnSpellCastInitiated -= InitiateCast;
        SystemManager.OnSpellCastAbrupted -= AbruptCast;
        SystemManager.OnSpellCastFinished -= AbruptCast;
    }

    public void InitiateCast(AbilityBoilerPlate ability) {

        GetComponent<RawImage>().enabled = true;
        text.text = ability.abilityName;
        text.gameObject.SetActive(true);
        StartCoroutine(CastSpell(ability));
    }

    public void AbruptCast(AbilityBoilerPlate ability) {

        StopAllCoroutines();
        text.gameObject.SetActive(false);
        GetComponent<RawImage>().enabled = false;
    }



    public IEnumerator CastSpell(AbilityBoilerPlate ability) {

        float elapsedTime = 0;

        while (elapsedTime < ability.castTime)
        {

            float fill = elapsedTime / ability.castTime;

            if (ability.castType == CastType.CHANNELED)
            {
                fill = Mathf.Lerp(1, 0, fill);
            }

            mat.SetFloat("_FillAmount", fill);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SystemManager.FinishSpell(ability);
        

    }


}
