using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCam : MonoBehaviour
{
    public AudioListener audioListener;
    public Camera camComponent;

    public float horizontalAimingSpeed = 6f;                           // Horizontal turn speed.
    public float verticalAimingSpeed = 6f;                             // Vertical turn speed.
    public float maxVerticalAngle = 30f;                               // Camera max clamp angle. 
    public float minVerticalAngle = -60f;                              // Camera min clamp angle.

    private float angleH = 0;                                          // Float to store camera horizontal angle related to mouse movement.
    private float angleV = 0;                                          // Float to store camera vertical angle related to mouse movement.

    void Start()
    {
        
    }

    // Update is called once per frame
   /* void LateUpdate()
    {
        /*angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * horizontalAimingSpeed;
        angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * verticalAimingSpeed;

        // Set vertical movement limit.
        angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);
        
        // Set camera orientation.
       // Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
        Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
        transform.rotation = aimRotation;
        //transform.rotation = Quaternion.Euler( new Vector3(0f, angleV, 0f));
        //transform.localEulerAngles = new Vector3(angleH, 0f, 0f);
    }*/

    public void RotateCam(float rotation)
    {
        /*angleH += Mathf.Clamp(horizontalAngle, -1, 1) * horizontalAimingSpeed;
        angleV += Mathf.Clamp(verticalAngle, -1, 1) * verticalAimingSpeed;

        // Set vertical movement limit.
        angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);

        // Set camera orientation.
        // Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
        Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);*/
        //transform.rotation = rotation;
        transform.localEulerAngles = new Vector3(rotation, 0f, 0f);
    }

    public void EnableCam()
    {
        camComponent.enabled = true;
        audioListener.enabled = true;
    }

    public void DisableCam()
    {
        camComponent.enabled = false;
        audioListener.enabled = false;
    }
        
}
