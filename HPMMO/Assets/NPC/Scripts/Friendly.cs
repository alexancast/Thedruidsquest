using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friendly : MonoBehaviour, ISelectable
{
    public Selection.SelectionType GetSelectionType()
    {
        return Selection.SelectionType.FRIENDLY;
    }
}
