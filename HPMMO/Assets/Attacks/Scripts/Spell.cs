using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Spell : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Shader shader;
    [SerializeField, Range(0, 1)] private float cornerRadius;

    [SerializeField] private AbilityBoilerPlate ability;
    
    private Transform startParent;
    private Image image;
    private Coroutine cooldownCoroutine;

    public void SetAbility(AbilityBoilerPlate ability)
    {
        this.ability = ability;
        UpdateIcon();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       
        startParent = transform.parent;

        if (startParent.GetComponent<Slot>().infinitySlot)
        {
            GameObject copy = Instantiate(gameObject, transform.parent);
            copy.name = gameObject.name;
        }

        transform.SetParent(transform.root);
        GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == transform.root)
        {
            if (startParent.GetComponent<Slot>().infinitySlot)
            {
                Destroy(gameObject);
            }
            else
            {

                GetComponent<RectTransform>().pivot = startParent.GetComponent<RectTransform>().pivot;
                GetComponent<RectTransform>().localScale = startParent.GetComponent<RectTransform>().localScale;
                transform.SetParent(startParent);
                transform.SetSiblingIndex(0);
                GetComponent<RectTransform>().localPosition = Vector3.zero;

            }

        }

        image.raycastTarget = true;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AbilityInfoPanel.Instance.Toggle(true);
        AbilityInfoPanel.Instance.SetupPanel(ability);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AbilityInfoPanel.Instance.Toggle(false);
    }

    public AbilityBoilerPlate GetAbilityBoilerPlate()
    {
        return ability;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SystemManager.InitiateSpell(ability);
    }

    public void UpdateIcon()
    {
        image = GetComponent<Image>();
        if (ability.icon != null)
        {
            image.sprite = ability.icon;

        }
        image.material = new Material(shader);
        image.material.SetColor("_Color", Color.white);
        image.material.SetFloat("_CornerRadius", cornerRadius);
    }

    public void OnEnable()
    {

        UpdateIcon();

        SystemManager.OnSpellCastFinished += CastSpell;

        if (CooldownManager.Instance.GetOnCooldown(ability))
        {
            if (cooldownCoroutine != null)
            {
                StopCoroutine(cooldownCoroutine);
            }

            cooldownCoroutine = StartCoroutine(Cooldown(CooldownManager.Instance.GetCooldown(ability)));
        }
        else
        {
            image.material.SetColor("_Color", Color.white);
        }
    }

    public void OnDisable()
    {
        SystemManager.OnSpellCastFinished -= CastSpell;
    }

    public void CastSpell(AbilityBoilerPlate ability) {

        if (this.ability == ability && CooldownManager.Instance.GetOnCooldown(ability))
        {
            if (cooldownCoroutine != null)
            {
                StopCoroutine(cooldownCoroutine);
            }

            cooldownCoroutine = StartCoroutine(Cooldown(CooldownManager.Instance.GetCooldown(ability)));
        }

    }

    public IEnumerator Cooldown(float cooldownTime) {

        float elapsedTime = 0;

        image.material.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1));

        while (elapsedTime < cooldownTime) {

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.material.SetColor("_Color", Color.white);
    }
}
