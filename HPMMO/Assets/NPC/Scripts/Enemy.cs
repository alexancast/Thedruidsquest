using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ISelectable
{
    public Selection.SelectionType GetSelectionType()
    {
        return Selection.SelectionType.ENEMY;
    }
}
