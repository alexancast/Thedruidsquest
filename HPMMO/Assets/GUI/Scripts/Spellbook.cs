using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Spellbook : MonoBehaviour
{

    [SerializeField] private GameObject spellbook;
    [SerializeField] private GameObject title;
    [SerializeField] private Spellist spellist;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject spellPrefab;

    public void Toggle(CallbackContext context) {

        if (context.performed)
        {
            spellbook.SetActive(!spellbook.activeSelf);
            title.SetActive(!title.activeSelf);
        }
    }

    public void Awake()
    {
        for (int i = 0; i < spellist.abilities.Length; i++)
        {
            Transform slot = Instantiate(slotPrefab, spellbook.transform).transform;
            Spell spell = Instantiate(spellPrefab, slot).GetComponent<Spell>();
            spell.SetAbility(spellist.abilities[i]);
        }
    }
}
