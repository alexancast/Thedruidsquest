using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Player")]
public class PlayerStats : ScriptableObject
{

    [Header("Player Stats")]
    [Tooltip("The speed in which the player is moving along the z axis.")]
    public float movementSpeed;

    [Tooltip("The current level of the player.")]
    public int level;

    [Tooltip("The highest level that can be achieved by the player.")]
    public int levelCap;

    [Tooltip("Reflects the character's physical well-being and vitality, indicating the amount of damage they can withstand before succumbing to defeat.")]
    public int health;

    [Tooltip("Represents the character's magical energy reserves, enabling them to cast spells and use magical abilities, with higher mana pools allowing for more frequent or powerful magic usage.")]
    public int mana;

    [Tooltip("Represents the accumulation of knowledge, skills, and practical experience gained by the character throughout their journey, serving as a measure of their progression within their current level and indicating their proximity to leveling up and acquiring new abilities or advancements.")]
    public int experience;

    [Header("Druid Stats")]
    [Tooltip("Reflects the druid's connection and harmony with nature, reducing the negative impact of their magic on the environment.")]
    public float natureAttunement;

    [Tooltip("Reflects the character's profound wisdom and spiritual attunement, enabling them to tap into vast reserves of mana with greater efficiency and control.")]
    public float wisdom;

    [Tooltip("Determines the character's physical resilience and stamina, reducing the amount of damage taken from physical attacks and increasing overall survivability.")]
    public float endurance;

    [Tooltip("Reflects the character's charm, persuasiveness, and social skills, influencing interactions with non-player characters (NPCs) and reducing prices when purchasing items from vendors, due to the character's ability to negotiate favorable deals and establish rapport.")]
    public float charisma;

    [Tooltip("Represents the druid's deep understanding and mastery of arcane forces, enhancing the raw power and effectiveness of their spells.")]
    public float mastery;

    public void OnEnable()
    {
        SystemManager.OnExperienceGained += AddExperience;
    }

    public void OnDisable()
    {
        SystemManager.OnExperienceGained -= AddExperience;
    }


    public int CalculateMaxExp()
    {

        int BaseExperience = 100;
        float ExperienceMultiplier = 1.2f;

        if (level == 1)
        {
            return BaseExperience;
        }

        // Calculate the required experience based on the logarithmic progression
        int requiredExperience = Mathf.RoundToInt(BaseExperience * Mathf.Log(level, ExperienceMultiplier));

        return requiredExperience;
    }

    public void AddExperience(int exp)
    {
        if (exp + experience > CalculateMaxExp())
        {
            int rest = exp + experience - CalculateMaxExp();

            level++;
            experience = rest;
            SystemManager.IncreaseLevel();
        }
        else
        {
            experience += exp;
        }
    }
}
