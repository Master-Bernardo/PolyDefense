using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//buildings and Units derive from this class - should i use it instead of damageable interface
public class GameEntity : MonoBehaviour
{
    public int teamID;

    public virtual void TakeDamage(float damage)
    {

    }

    public virtual void OnDie()
    {

    }
}
