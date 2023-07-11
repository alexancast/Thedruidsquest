using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NatureMeter : MonoBehaviour
{

    private Image image;

    [Range(0,1)] public float natureValue;
    public Color livingColor;
    public Color deadColor;

    public void Start()
    {
        image = GetComponent<Image>();
    }

    public void Update()
    {
        image.color = Color.Lerp(livingColor, deadColor, natureValue);
    }

}
