using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    public bool movementEnabled;

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

            cam.transform.position += movementVector;

            if (mouseScrollInput != 0)
            {
                cam.orthographicSize -= mouseScrollInput * 2;
            }
        }
    }
}
