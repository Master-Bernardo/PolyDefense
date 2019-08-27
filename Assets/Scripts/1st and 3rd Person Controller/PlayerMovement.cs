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
    public float jumpForce;

    Vector3 nextMovementForce;

    public void Jump()
    {
        jump = true;
    }

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

        rb.MovePosition(transform.position + nextMovementForce * Time.deltaTime);

        if (jump)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0),ForceMode.Impulse);
            jump = false;
        }

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
