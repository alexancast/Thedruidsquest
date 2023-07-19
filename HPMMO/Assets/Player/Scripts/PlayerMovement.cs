using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject targetPlate;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackSpawnPoint;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameObject droughtPoint;
    [SerializeField] private LayerMask droughtPointLayer;
    [SerializeField] private GameObject selectionIndicator;

    [HideInInspector] public Vector3 movementVector;

    private GameObject selectedObject;

    public static PlayerMovement Instance;

    public void Awake()
    {
        Instance = this;
    }

    public ISelectable SelectedObject() {

        if (selectedObject != null)
        {
            return selectedObject.GetComponent<ISelectable>();
        }
        else
        {
            return null;
        }
    }

    public void Move(CallbackContext context)
    {

        //movementVector.x = context.ReadValue<Vector2>().x;
        movementVector.z = context.ReadValue<Vector2>().y;

        animator.SetFloat("Vertical", movementVector.z);
        //animator.SetFloat("Horizontal", movementVector.x);

    }

    public void Update()
    {

        if (!controller.isGrounded)
        {
            movementVector.y += Time.deltaTime * Physics.gravity.y * 80;

        }
        else
        {
            movementVector.y = 0;
        }

        if (Mathf.Abs(movementVector.z) > 0)
        {
            SystemManager.AbruptSpell(null);
        }

        controller.Move(transform.TransformDirection(movementVector) * Time.deltaTime * playerStats.movementSpeed);
        Look();
    }

    public void OnEnable()
    {
        SystemManager.OnSpellCastInitiated += StartCast;
        SystemManager.OnSpellCastAbrupted += AbruptCast;
        SystemManager.OnSpellCastFinished += FinishCast;
    }

    public void OnDisable()
    {
        SystemManager.OnSpellCastInitiated -= StartCast;
        SystemManager.OnSpellCastAbrupted -= AbruptCast;
        SystemManager.OnSpellCastFinished -= FinishCast;
    }

    public void Select(CallbackContext context)
    {
        if (context.performed)
        {

            // Skapa en stråle från kameran till muspekaren
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Utför raycasting och kontrollera om det finns en träffpunkt
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<ISelectable>() != null)
                {
                    selectedObject = hit.collider.gameObject;

                    selectionIndicator.transform.parent = hit.collider.gameObject.transform;
                    selectionIndicator.transform.localPosition = Vector3.zero;
                    selectionIndicator.SetActive(true);
                    selectionIndicator.GetComponent<Selection>().SetTarget(hit.collider.GetComponent<ISelectable>().GetSelectionType());

                }
                else if(selectedObject != null)
                {
                    selectionIndicator.transform.parent = null;
                    selectionIndicator.transform.position = Vector3.zero;
                    selectionIndicator.SetActive(false);
                }
            }
        }
    }


    public void StartCast(AbilityBoilerPlate ability)
    {

        animator.SetBool("Draining", true);
        particles.Play();

        if (ability.castType == CastType.CHANNELED)
        {
            Instantiate(ability.effect, transform.position, transform.rotation);
        }
    }

    public void AbruptCast(AbilityBoilerPlate ability)
    {

        animator.SetBool("Draining", false);
        particles.Stop();
    }

    public void FinishCast(AbilityBoilerPlate ability)
    {

        animator.SetBool("Draining", false);
        particles.Stop();

        if (ability.castType == CastType.BUILD)
        {
            Instantiate(ability.effect, transform.position, transform.rotation);


            RaycastHit hit;

            Vector3 castPos = transform.position;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, droughtPointLayer))
            {
                castPos = hit.point;
            }

            GameObject dp = Instantiate(droughtPoint, castPos, transform.rotation);
            dp.GetComponent<DroughtPoint>().Setup(ability.reservoirCost);
        }

    }


    public float CheckDrought(AbilityBoilerPlate ability)
    {

        RaycastHit hit;

        Vector3 castPos = transform.position;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, droughtPointLayer))
        {
            castPos = hit.point;
        }


        Collider[] colliders = Physics.OverlapSphere(castPos, ability.reservoirCost);

        float draughtMultiplier = 1; //Lower value means more extensive draught;

        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<DroughtPoint>())
            {
                draughtMultiplier *= collider.GetComponent<DroughtPoint>().GetStrength();
            }
        }

        return draughtMultiplier;

    }


    public float rotationSpeed = 5f; // Justera hastigheten för rotationen

    private Quaternion targetRotation; // Målet för rotationen

    public void Look()
    {
        // Skapa en stråle från kameran till muspekaren
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Utför raycasting och kontrollera om det finns en träffpunkt
        if (Physics.Raycast(ray, out hit))
        {
            // Räkna ut riktningen från spelarens position till träffpunkten
            Vector3 direction = hit.point - transform.position;
            direction.y = 0f; // Se till att spelaren inte roterar kring y-axeln

            // Kontrollera om riktningen är en nollvektor
            if (direction != Vector3.zero)
            {
                // Rotera spelaren för att titta mot muspekaren
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

                // Använd Slerp för att smidigt rotera mot målet
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

}
