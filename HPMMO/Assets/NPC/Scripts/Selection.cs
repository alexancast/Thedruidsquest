using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{

    public void SetTarget(SelectionType selectionType) {


        Color color = Color.blue;

        switch (selectionType) {

            case SelectionType.FRIENDLY:
                color = Color.green;
                break;


            case SelectionType.ENEMY:
                color = Color.red;
                break;


            case SelectionType.NEUTRAL:
                color = Color.yellow;
                break;

        }

        GetComponent<Renderer>().material.SetColor("_Color", color);

    }

    public enum SelectionType {

        FRIENDLY, ENEMY, NEUTRAL, ANY
    }
}
