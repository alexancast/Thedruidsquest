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
    [SerializeField] private GameObject baseAttack;

    [Header("Values")]
    [SerializeField] private float movementSpeed;

    private Vector3 movementVector;

    public void Move(CallbackContext context) {

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

        controller.Move(transform.TransformDirection(movementVector) * Time.deltaTime * movementSpeed);
        Look();
    }

    public void LaunchAttack() {

        GameObject o = Instantiate(baseAttack, attackSpawnPoint.position, transform.rotation);
        o.AddComponent<Rigidbody>().AddForce(o.transform.forward * 20, ForceMode.Impulse);

    }

    public void MouseButtonDown(CallbackContext context)
    {

        if (context.performed)
        {

            //// Skapa en ray från muspositionen
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //// Skapa en raycast träffvariabel för att lagra resultatet
            //RaycastHit hit;

            //// Utför raycasten och kontrollera om den träffar något
            //if (Physics.Raycast(ray, out hit))
            //{
            //    LaunchAttack();
            //}

        }
        else if (context.canceled)
        {

        }
        
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
