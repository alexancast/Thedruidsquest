using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private float transitionTime;

    private RawImage image;

    public void Start()
    {
        image = GetComponent<RawImage>();

        AddExperience(0);
    }

    public void OnEnable()
    {
        SystemManager.OnExperienceGained += AddExperience;
    }

    public void OnDisable()
    {
        SystemManager.OnExperienceGained -= AddExperience;
    }

    public void AddExperience(int exp) {

        UpdateExpText();
        UpdateBar();
    }

    public void UpdateExpText()
    {
        text.text = stats.experience + " / " + stats.CalculateMaxExp();
    }

    public void UpdateBar() {

        StopCoroutine("TransitionBar");
        StartCoroutine(TransitionBar());

    }


    public IEnumerator TransitionBar() {

        float elapsedTime = 0;

        float oMin = image.material.GetFloat("_FillAmount");
        oMin = Mathf.Lerp(0, stats.CalculateMaxExp(), oMin);
        float oMax = stats.experience;

        while (elapsedTime < transitionTime)
        {
            float fill = Mathf.Lerp(oMin, oMax, elapsedTime / transitionTime);
            fill = Mathf.InverseLerp(0, stats.CalculateMaxExp(), fill);

            image.material.SetFloat("_FillAmount", fill);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
