using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillingScript : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private GameObject cameraBillingCounter;

    public float rotationSpeed = 100.0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start()
    {
        // Initialize rotationX and rotationY within their allowed ranges
        rotationX = Mathf.Clamp(transform.localEulerAngles.x, 7f, 30f);
        rotationY = Mathf.Clamp(transform.localEulerAngles.y, -87f, -27f);
    }

    void Update()
    {
        // Get joystick input
        float joystickVertical = joystick.Vertical;
        float joystickHorizontal = joystick.Horizontal;

        // Calculate rotation angles based on joystick input
        // Invert the vertical input by multiplying by -1
        rotationX -= joystickVertical * rotationSpeed * Time.deltaTime;
        rotationY += joystickHorizontal * rotationSpeed * Time.deltaTime;

        // Clamp vertical rotation to be within 0 to 42 degrees
        rotationX = Mathf.Clamp(rotationX, 7f, 30f);
        // Clamp horizontal rotation to be within -87 to -27 degrees
        rotationY = Mathf.Clamp(rotationY, -87f, -27f);

        // Apply rotation to camera
        cameraBillingCounter.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
    }
}

