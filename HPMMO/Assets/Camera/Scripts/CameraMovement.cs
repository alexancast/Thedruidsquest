using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CameraMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform target;

    [Header("Values")]
    [SerializeField] private Vector3 offset;
    [SerializeField, Range(0, 1)] private float followSpeed;
    [SerializeField, Range(0,1)] private float cameraFollowTreshold;
    [SerializeField, Range(0, 10)] private float cameraFollowDistance;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private CameraZoomSettings zoomSettings;

    [System.Serializable]
    public struct CameraZoomSettings {

        public float minCameraDistance;
        public float maxCameraDistance;

    }

    public void LateUpdate()
    {

        Vector3 mouseOffset = GetNormalizedMousePosition().magnitude > cameraFollowTreshold? GetNormalizedMousePosition() * cameraFollowDistance : new Vector3(0,0,0);
        transform.position = Vector3.Lerp(transform.position, target.position + offset + mouseOffset, followSpeed / 100);
    }

    public Vector3 GetNormalizedMousePosition() {

        // Få muspositionen i pixlar
        Vector3 mousePositionPixels = Input.mousePosition;

        // Konvertera muspositionen från pixlar till enhetsposition
        Vector3 mousePositionViewport = Camera.main.ScreenToViewportPoint(mousePositionPixels);

        // Skala muspositionen till önskat område (-1, -1) till (1, 1)
        Vector3 mousePositionScaled = new Vector3(mousePositionViewport.x * 2 - 1, 0, mousePositionViewport.y * 2 - 1);

        return mousePositionScaled;
    }

    public void Scroll(CallbackContext context)
    {

        float z = context.ReadValue<float>();
        //Scroll up

        offset.y += z * scrollSpeed;
        offset.z -= z * scrollSpeed;

        offset.y = Mathf.Clamp(offset.y, zoomSettings.minCameraDistance, zoomSettings.maxCameraDistance);
        offset.z = Mathf.Clamp(offset.z, -zoomSettings.maxCameraDistance, -zoomSettings.minCameraDistance);

    }

}
