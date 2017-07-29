using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEnemy
{
    public double Health = 100;
    public double PowerDropped = 30;

    /// <summary>
    /// Amount of damage to the players power/health
    /// </summary>
    public double EnemyGunDamage = 10;

    public event Action<double> OnEnemyKilled;

    void Start()
    {

    }

    void Update()
    {

    }

    public void RecieveHit(double damage)
    {
        Health -= damage;
        Debug.Log("Hit enemy for '" + damage + "' damage");
        if (Health < 0)
        {
            OnKilled();
        }
    }

    public void Initialize(Transform player, double health, double powerDropped, double enemyGunDamage)
    {
        GetComponentInChildren<EnemyGunController>().Player = player;
        Health = health;
        PowerDropped = powerDropped;
    }

    void OnKilled()
    {
        Debug.Log("Killed enemy. Dropped '" + PowerDropped + "' power");
        InvokeEnemyKilled();
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == Constants.BULLET_TAG)
        {
            GameObject enemy = col.gameObject;
            MoveBulletTrail moveComponent = enemy.GetComponent<MoveBulletTrail>();
            //Do damage and destroy bullet early
            if (moveComponent.Owner != this.gameObject)
            {
                RecieveHit(moveComponent.Damage);
                moveComponent.DestroyEarly(); //destroy bulley early
            }
        }
    }

    void InvokeEnemyKilled()
    {
        if(OnEnemyKilled != null)
            OnEnemyKilled.Invoke(PowerDropped);
    }
}
