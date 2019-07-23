using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_MoveForward : MonoBehaviour
{
    public EC_Movement movement;
    public Transform targetToLookAt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            movement.MoveTo(transform.position + transform.forward * 20);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            movement.LookAt(targetToLookAt);
        }
    }
}
