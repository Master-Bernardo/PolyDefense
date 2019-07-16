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
    public UnityEvent onDieEvent;

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

    public virtual void OnDie()
    {
        onDieEvent.Invoke();
        foreach (Ability ability in abilities)
        {
            ability.OnDie();
        }
        Destroy(gameObject);
    }
}
