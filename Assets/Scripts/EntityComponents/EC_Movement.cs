using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EC_Movement : Ability
{  
    [HideInInspector]
    NavMeshAgent agent;

    //for rotation independent of navmeshAgent;
    float angularSpeed;

    public bool showGizmo;


    //for optimisation we can call the updater only every x frames
    float nextMovementUpdateTime;
    public float movementUpdateIntervall = 1 / 6;

    //our agent can either rotate to the direction he is facing or have a target to which he is alwys rotated to - if lookAt is true
    Transform targetToLookAt;
    bool lookAt = false;
    float lastRotationTime; // if we rotate only once every x frames, we need to calculate our own deltaTIme

    public override void SetUpAbility(GameEntity entity)
    {
        agent = GetComponent<NavMeshAgent>();

        //almost the same speed as original navmeshAgent
        angularSpeed = agent.angularSpeed;
        //optimisation
        nextMovementUpdateTime = Time.time + Random.Range(0, movementUpdateIntervall);

    }

    //update is only for looks- the rotation is important for logic but it can be a bit jaggy if far away or not on screen - lod this script, only call it every x seconds
    public override void UpdateAbility()
    {
       if (lookAt)
       {
            if (Time.time > nextMovementUpdateTime)
            {
                nextMovementUpdateTime = Time.time + movementUpdateIntervall;
                if (targetToLookAt != null)
                {
                    RotateTo(targetToLookAt.position - transform.position);
                }
            }
        }
    }


    //sets the agent to rotate 
    public void RotateTo(Vector3 direction)
    {
        float deltaTime = Time.time - lastRotationTime;
        direction.y = 0;
        Quaternion desiredLookRotation = Quaternion.LookRotation(direction);
        //because we want the same speed as the agent, which has its angular speed saved as degrees per second we use the rotaate towards function
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredLookRotation, angularSpeed * deltaTime );
        lastRotationTime = Time.time;
    }

    //for now simple moveTo without surface ship or flying
    public void MoveTo(Vector3 destination)
    {
        agent.SetDestination(destination);
    }


    //this method tells the agent to look at a specific target while moving
    public void LookAt(Transform targetToLookAt)
    {
        this.targetToLookAt = targetToLookAt;
        agent.updateRotation = false;
        lastRotationTime = Time.time;
        lookAt = true;
    }

    public void StopLookAt()
    {
        agent.updateRotation = true;
        lookAt = false;
    }

    public bool IsMoving()
    {
        return agent.velocity.magnitude > agent.speed/2;
    }

    public void Stop()
    {
        agent.ResetPath();
    }

    public float GetCurrentVelocityMagnitude()
    {
        return agent.velocity.magnitude;
    }

    public Vector3 GetCurrentVelocity()
    {
        return agent.velocity;
    }

    public float GetMaxSpeed()
    {
        return agent.speed;
    }

     private void OnDrawGizmos()
     {
         if (showGizmo && agent!=null)
         {
             if (agent.destination != null)
             {
                 Gizmos.color = Color.green;
                 Gizmos.DrawCube(agent.destination, new Vector3(0.2f, 2, 0.2f));

             }
         }
     }
}

