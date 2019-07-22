using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    public bool movementEnabled;
    public bool orthographic;
    public float movementSpeed;
    [Tooltip("the higher - the faster")]
    public float heightMovementMultiplier;
    public float heightZoomMultiplier;

    public float zoomSpeed;

    [Header("Perspective")]
    public float maxY;
    public float minY;

    public float smoothingSpeed;


    Vector3 desiredPosition;

    void Start()
    {
        desiredPosition = cam.transform.position;
    }

    void Update()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, desiredPosition, smoothingSpeed * Time.deltaTime);
    }

    public void MoveCamera(float horizontalInput, float verticalInput, float mouseScrollInput)
    {
        if (movementEnabled)
        {
            //simple rts camera movement
            Vector3 flatUpVectorOfCamera = cam.transform.up;
            flatUpVectorOfCamera.y = 0;
            flatUpVectorOfCamera.Normalize();
            Vector3 flatRightVectorOfCamera = cam.transform.right;
            flatRightVectorOfCamera.y = 0;
            flatRightVectorOfCamera.Normalize();

            flatRightVectorOfCamera *= horizontalInput;
            flatUpVectorOfCamera *= verticalInput;

            Vector3 movementVector = flatRightVectorOfCamera + flatUpVectorOfCamera;

            if (!orthographic)
            {

                desiredPosition += movementVector * movementSpeed * cam.transform.position.y * heightMovementMultiplier;
            }
            else
            {
                desiredPosition += movementVector * movementSpeed * cam.orthographicSize * heightMovementMultiplier;
            }
         
            if (mouseScrollInput != 0)
            {
                if (orthographic)
                {
                    desiredPosition.y -= mouseScrollInput * zoomSpeed * heightZoomMultiplier;
                }
                else
                {
                    float riser = -(mouseScrollInput *= zoomSpeed * heightZoomMultiplier);


                    if (riser + desiredPosition.y < maxY && riser + desiredPosition.y > minY)
                    {
                        desiredPosition.y += riser;
                    }
                   

                 
                }
   
            }
        }
    }

   
}
