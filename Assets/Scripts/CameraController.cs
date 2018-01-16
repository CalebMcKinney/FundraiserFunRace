using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    float yaw;
    float pitch;

    public bool lockCursor;

    public float mouseSensitivity = 10;
    public Transform target;
    public float dstFromTarget = 2;

    public float rotationSmoothTime = 1.2f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public Vector2 pitchMinMax = new Vector2(5, 85);

    bool rightMouseDown;

    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        rightMouseDown = Input.GetMouseButton(1);
    }

    void LateUpdate ()
    {
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);

            Vector3 targetRotation = new Vector3(pitch, yaw);
            transform.eulerAngles = currentRotation;
        }

        transform.position = target.position - transform.forward * dstFromTarget;

    }
}
