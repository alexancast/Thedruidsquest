using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability")]
public class AbilityBoilerPlate : ScriptableObject
{

    public Sprite icon;
    public string abilityName;
    public AbilityType abilityType;
    [TextArea] public string description;
    public CastType castType;
    public float castTime;
    public int cooldown;
    public Selection.SelectionType targetType;

    [Tooltip("A measure of the delicate balance of life-sustaining energy within a designated area, represented in square meters, representing the extent to which the surrounding nature is drained and exploited by the druid's spellcasting abilities.")]
    public int reservoirCost;

    public GameObject effect; //This is where the actual attack or spell is stored. All abilities must have attached GameObjects.


}

public enum AbilityType {

    SPELL, BUFF, DEBUFF

}

public enum CastType {

    BUILD, CHANNELED,

}