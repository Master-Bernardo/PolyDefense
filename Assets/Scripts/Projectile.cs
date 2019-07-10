﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float damage;
    public float startVelocity;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * startVelocity;

    }

    private void OnCollisionEnter(Collision collision)
    {
        //solve this with I Damageable
        // if(collision)
        IDamageable<float> damageable = collision.gameObject.GetComponent<IDamageable<float>>();

        if (damageable != null) damageable.TakeDamage(damage);
    }
}
