using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

public class Slot : MonoBehaviour, IDropHandler
{
    public bool infinitySlot;
    private Spell spell;

    public void Start()
    {
        spell = GetComponentInChildren<Spell>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        spell = eventData.pointerDrag.GetComponent<Spell>();

        if (infinitySlot || GetComponentInChildren<Spell>() != null)
        {
            Destroy(spell.gameObject);
        }


        if (spell != null)
        {
            spell.GetComponent<RectTransform>().pivot = GetComponent<RectTransform>().pivot;
            spell.GetComponent<RectTransform>().localScale = GetComponent<RectTransform>().localScale;
            spell.transform.SetParent(transform);
            spell.transform.SetSiblingIndex(0);
            spell.GetComponent<RectTransform>().localPosition = Vector3.zero;

        }
    }

    public void RunSpell(CallbackContext context)
    {

        if (GetComponentInChildren<Spell>() != null && context.performed)
        {
            SystemManager.InitiateSpell(spell.GetAbilityBoilerPlate());
        }

    }
}
