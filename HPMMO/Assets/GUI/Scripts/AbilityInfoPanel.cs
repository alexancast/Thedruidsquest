using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI mana;
    [SerializeField] private TextMeshProUGUI castTime;
    [SerializeField] private TextMeshProUGUI cooldown;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject background;

    public static AbilityInfoPanel Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void Toggle(bool toggle) {

        background.gameObject.SetActive(toggle);
        title.gameObject.SetActive(toggle);
        castTime.gameObject.SetActive(toggle);
        cooldown.gameObject.SetActive(toggle);
        description.gameObject.SetActive(toggle);
        mana.gameObject.SetActive(toggle);

    }

    public void SetupPanel(AbilityBoilerPlate ability) {

        title.text = ability.abilityName;
        mana.text = ability.reservoirCost + " reservoir";
        castTime.text = ability.castTime + " Sec";
        cooldown.text = ability.cooldown + " Sec cooldown";
        description.text = ability.description;

    }
}
