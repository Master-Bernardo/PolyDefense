using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float accelerationForce;


    public float maxSpeed;

    public float turnAccelerationForce;
    public float turnSpeed;

    bool jump;

    Vector3 nextMovementForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*void Update()
    {
        Vector3 lookRotation = cam.forward;
        lookRotation.y = 0;
        SetRotation(Quaternion.LookRotation(lookRotation));

    }*/
    /*
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        /*if (vertical < 0)
        {
            vertical *= accelerationForceForward;
        }
        else
        {
            vertical *= accelerationForceBackwards;
        }*/

       // nextMovementForce = rb.transform.TransformDirection(new Vector3(horizontal, 0, vertical) * accelerationForce);
    //}*/

    public void Move(Vector3 movementVector)
    {
        nextMovementForce = rb.transform.TransformDirection(movementVector*accelerationForce);
    }

    public void LookAt(Vector3 direction)
    {
        direction.y = 0;
        SetRotation(Quaternion.LookRotation(direction));
    }

    private void FixedUpdate()
    {
        /*rb.AddForce(nextMovementForce);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
            Debug.Log("too fast");

        }*/
        rb.MovePosition(transform.position + nextMovementForce * Time.deltaTime);


    }

    public void SetRotation(Quaternion rotation)
    {
        rb.MoveRotation(rotation);
       // rb.transform.rotation = rotation;
    }

    public void AddRotation(Quaternion rotation)
    {
        rb.MoveRotation(rb.rotation * rotation);
    }
}
