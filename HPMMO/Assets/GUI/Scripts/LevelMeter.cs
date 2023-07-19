using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelMeter : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;

    private TextMeshProUGUI text;

    private void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = stats.level.ToString();
        SystemManager.OnLevelIncreased += IncreaseLevel;
        
    }

    private void OnDisable()
    {
        SystemManager.OnLevelIncreased -= IncreaseLevel;
    }

    public void IncreaseLevel() {
        text.text = stats.level.ToString();
    }
}
