using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    public static CooldownManager Instance;
    private Dictionary<AbilityBoilerPlate, float> cooldowns = new Dictionary<AbilityBoilerPlate, float>();


    public void Awake()
    {
        Instance = this;
    }

    public void OnEnable()
    {
        SystemManager.OnSpellCastFinished += AddCooldown;
    }

    public void OnDisable()
    {
        SystemManager.OnSpellCastFinished -= AddCooldown;
    }

    public void Update()
    {
        List<AbilityBoilerPlate> keysToRemove = new List<AbilityBoilerPlate>();

        foreach (var cooldown in cooldowns)
        {
            keysToRemove.Add(cooldown.Key);
        }

        foreach (var key in keysToRemove)
        {
            cooldowns[key] -= Time.deltaTime;

            if (cooldowns[key] <= 0)
            {
                cooldowns.Remove(key);
            }
        }

    }


    public float GetCooldown(AbilityBoilerPlate ability) {

        return cooldowns[ability];
    }

    public bool GetOnCooldown(AbilityBoilerPlate ability) {

        return cooldowns.ContainsKey(ability);
    }

    public void AddCooldown(AbilityBoilerPlate ability) {

        if (!GetOnCooldown(ability))
        {
            cooldowns.Add(ability, ability.cooldown);
        }
    }
}
