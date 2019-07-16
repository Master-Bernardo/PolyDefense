using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float damage;
    public float startVelocity;
    public int projectileTeamID; //who shoot this projectile
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

        if (damageable != null)
        {
            // check who did we hit, check if he has an gameEntity
            GameEntity entity = collision.gameObject.GetComponent<GameEntity>();
            if (entity != null)
            {
                if (!Settings.Instance.friendlyFire)
                {
                    DiplomacyStatus diplomacyStatus = Settings.Instance.GetDiplomacyStatus(projectileTeamID, entity.teamID);
                    if(diplomacyStatus == DiplomacyStatus.War)
                    {
                        damageable.TakeDamage(damage);
                    }

                }
                else
                {
                    damageable.TakeDamage(damage);
                }

            }
            else
            {
                damageable.TakeDamage(damage);
            }
        }

        Destroy(gameObject);

    }
}
