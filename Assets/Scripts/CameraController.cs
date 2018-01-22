using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    float yaw;
    float pitch;

    public bool lockCursor;

    public float mouseSensitivity = 10;
    public float firstPersonSensitivity;
    public float arrowKeySpeed;

    public Transform target;
    public float dstFromTarget = 2;

    public float rotationSmoothTime = 1.2f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public float positionSmoothTime = 1.2f;
    Vector3 positionVelocity;
    Vector3 currentPosition;

    public Vector2 pitchMinMax = new Vector2(5, 85);
    public Vector2 firstPersonMinMax = new Vector2(-50, 35);

    public Vector2 zoomMinMax = new Vector2(0, 5);
    public float scrollSensitivity;

    bool rightMouseDown;

    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate ()
    {
        dstFromTarget += -Input.GetAxisRaw("Mouse ScrollWheel") * scrollSensitivity;
        dstFromTarget = Mathf.Clamp(dstFromTarget, zoomMinMax.x, zoomMinMax.y);

        if (Input.GetMouseButton(1) || dstFromTarget < 0.1)
        {
            yaw += Input.GetAxis("Mouse X") * ((dstFromTarget < 0.1) ? firstPersonSensitivity : mouseSensitivity);
            pitch -= Input.GetAxis("Mouse Y") * ((dstFromTarget < 0.1) ? firstPersonSensitivity : mouseSensitivity);

            if (dstFromTarget < 0.1)
            {
                pitch = Mathf.Clamp(pitch, firstPersonMinMax.x, firstPersonMinMax.y);
            }
            else
            {
                pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
            }
        }

        if(dstFromTarget >= 0.1)
        {
            if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                yaw -= arrowKeySpeed;
            }
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                yaw += arrowKeySpeed;
            }
        }

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        Vector3 targetRotation = new Vector3(pitch, yaw);
        transform.eulerAngles = currentRotation;

        Vector3 targetPosition = target.position - transform.forward * dstFromTarget;
        currentPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref positionVelocity, positionSmoothTime);

        /* if(Physics.OverlapSphere(currentPosition, 0.1f).Length > 1)
        {
            Debug.Log("something there");
        }
        else
        {
            Debug.Log("Nothing there");
        }
        */

        transform.position = currentPosition;
    }
}
