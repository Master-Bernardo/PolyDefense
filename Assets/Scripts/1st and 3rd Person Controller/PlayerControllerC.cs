using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerC : MonoBehaviour
{
    public PlayerMovement playerMovement;
    //public Quaternion lookRotation;
    public Transform lookDirection;
    //Quaternion rotatePlayerRotation;
    public PCam tPCam;
    public PCam fPCam;
    public FPModelHider fPModelHider;

    public bool firstPersonMode;

    float horizontalAngle;
    float verticalAngle;

    public float horizontalAimingSpeed = 6f;                           
    public float verticalAimingSpeed = 6f;

    float angleH = 0;
    float angleV = 0;

    public float maxVerticalAngleTP = 30f;                               // Camera max clamp angle. 
    public float minVerticalAngleTP = -60f;                              // Camera max clamp angle. 

    //for first person camera
    public float maxVerticalAngleFP = 30f;                               
    public float minVerticalAngleFP = -60f;


    void Start()
    {
        tPCam.EnableCam();
        fPCam.DisableCam();
    }
    // Update is called once per frame
    void Update()
    {
        //get the wasd movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Debug.Log("movement: " + new Vector3(horizontal, 0, vertical));
        playerMovement.Move(new Vector3(horizontal, 0, vertical));
        
        //playerMovement.LookAt(tPCam.transform.forward);

        horizontalAngle = Input.GetAxis("Mouse X");
        //Debug.Log("horiz: " + horizontalAngle);
        verticalAngle = Input.GetAxis("Mouse Y");

        angleH += Mathf.Clamp(horizontalAngle, -1, 1) * horizontalAimingSpeed;
        angleV += Mathf.Clamp(verticalAngle, -1, 1) * verticalAimingSpeed;

        // Set vertical movement limit.
        if (firstPersonMode)
        {
            angleV = Mathf.Clamp(angleV, minVerticalAngleFP, maxVerticalAngleFP);

        }
        else
        {
            angleV = Mathf.Clamp(angleV, minVerticalAngleTP, maxVerticalAngleTP);

        }

        // Set camera orientation.
        // lookRotation = Quaternion.Euler(-angleV, angleH, 0);
        // rotatePlayerRotation = Quaternion.Euler(0, angleH, 0);

        //transform.localEulerAngles = new Vector3(0f, lookRotation.eulerAngles.y, 0f);
        transform.localEulerAngles = new Vector3(0f, angleH, 0f);
        lookDirection.localEulerAngles = new Vector3(-angleV, 0f, 0f);





        if (Input.GetKeyDown(KeyCode.T))
        {
            if (firstPersonMode)
            {
                fPModelHider.ShowMeshes();
                firstPersonMode = false;
                tPCam.EnableCam();
                fPCam.DisableCam();
            }
            else
            {
                fPModelHider.HideMeshes();
                firstPersonMode = true;
                fPCam.EnableCam();
                tPCam.DisableCam();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.Jump();
        }


    }

    private void LateUpdate()
    {
        // Get mouse movement to orbit the camera.
        // Mouse:
       

        //fPCam.RotateCam(lookRotation.eulerAngles.x);
        //tPCam.RotateCam(horizontalAngle,verticalAngle, lookRotation, rotatePlayerRotation);
    }
}
