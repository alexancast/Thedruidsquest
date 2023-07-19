using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SystemManager
{

    public delegate void ExperienceEventHandler(int experiencePoints);
    public static event ExperienceEventHandler OnExperienceGained;

    public delegate void LevelEventHandler();
    public static event LevelEventHandler OnLevelIncreased;

    public delegate void SpellCastHandler(AbilityBoilerPlate ability);
    public static event SpellCastHandler OnSpellCastInitiated;
    public static event SpellCastHandler OnSpellCastAbrupted;
    public static event SpellCastHandler OnSpellCastFinished;

    public static void RaiseExperienceEvent(int experiencePoints)
    {
        OnExperienceGained?.Invoke(experiencePoints);
    }

    public static void IncreaseLevel() {

        if (OnLevelIncreased != null)
            OnLevelIncreased();
    }

    public static void InitiateSpell(AbilityBoilerPlate ability) {


        if (CooldownManager.Instance.GetOnCooldown(ability))
        {
            Teleprompter.Instance.RunMessage("That ability is on cooldown.", MessageType.ERROR);
            return;
        }

        if (Mathf.Abs(PlayerMovement.Instance.movementVector.z) > 0) {
            Teleprompter.Instance.RunMessage("Cannot cast that spell while moving.", MessageType.ERROR);
            return;
        }

        if (PlayerMovement.Instance.SelectedObject() == null)
        {
            Teleprompter.Instance.RunMessage("No target selected.", MessageType.ERROR);
            return;
        }

        if (PlayerMovement.Instance.SelectedObject().GetSelectionType() != Selection.SelectionType.ANY && PlayerMovement.Instance.SelectedObject().GetSelectionType() != ability.targetType)
        {
            Teleprompter.Instance.RunMessage("Invalid target.", MessageType.WARNING);
            return;
        }

        float draughtThreshold = 0.5f;

        if (PlayerMovement.Instance.CheckDrought(ability) < draughtThreshold)
        {
            Teleprompter.Instance.RunMessage("Cannot cast spell due to extensive drought.", MessageType.ERROR);
            return;
        }

        OnSpellCastInitiated?.Invoke(ability);

    }

    public static void FinishSpell(AbilityBoilerPlate ability)
    {
        OnSpellCastFinished?.Invoke(ability);
    }

    public static void AbruptSpell(AbilityBoilerPlate ability)
    {
        OnSpellCastAbrupted?.Invoke(ability);
    }
}
