using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable<T>
{
    void TakeDamage(T damage);
}


//buildings and Units derive from this class - should i use it instead of damageable interface
public class GameEntity : MonoBehaviour
{
    public int teamID;
    public Ability[] abilities;
    public Vector3 aimingCorrector; //correctes the aiming, sets it higher, because every units 0 is at the bottom for distance chekcs
    public UnityEvent onDieEvent;
    public float width;

    private void Start()
    {
        foreach (Ability ability in abilities)
        {
            ability.SetUpAbility(this);
        }
    }

    public void Update()
    {
        foreach (Ability ability in abilities)
        {
            ability.UpdateAbility();
        }
    }

    public Vector3 GetPositionForAiming()
    {
        return (transform.position + aimingCorrector);
    }

    public virtual void Die()
    {
        onDieEvent.Invoke();
        foreach (Ability ability in abilities)
        {
            ability.OnDie();
        }
        Destroy(gameObject);
    }
}
