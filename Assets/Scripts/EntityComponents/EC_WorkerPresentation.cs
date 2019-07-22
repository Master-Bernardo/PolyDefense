using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_WorkerPresentation : Ability
{
    public float lookInterval;
    float nextLookTime;
    public EC_Movement movement;
    public Transform lookAtObject;

    public override void SetUpAbility(GameEntity entity)
    {
        lookAtObject.SetParent(null);
        nextLookTime = Time.time + Random.Range(lookInterval / 2, lookInterval);
        //lookAtObject.position = transform.position + transform.forward * 2 + Random.insideUnitSphere * 2;
        movement.LookAt(lookAtObject);
    }

    public override void UpdateAbility()
    {
        if (Time.time > nextLookTime)
        {
            nextLookTime = Time.time + Random.Range(lookInterval / 2, lookInterval);
            lookAtObject.position = transform.position + transform.forward * 2 + Random.insideUnitSphere * 2;

        }
    }

}
