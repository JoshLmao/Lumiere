using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEnemy
{
    public double Health = 100;
    public double PowerDropped = 30;

    public int ScoreAmount { get { return CalculateScore(); } }

    /// <summary>
    /// Amount of damage to the players power/health
    /// </summary>
    public double EnemyGunDamage = 10;

    public event Action<EnemyController> OnEnemyKilled;

    double m_startHealth;

    void Start()
    {

    }

    void Update()
    {

    }

    public void RecieveHit(double damage)
    {
        Health -= damage;
        Debug.Log("Player hit for '" + damage + "' damage");
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
        m_startHealth = health;
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
            OnEnemyKilled.Invoke(this);
    }

    /// <summary>
    /// Way of calculating score for when enemy is killed rather than being set random
    /// </summary>
    /// <returns></returns>
    int CalculateScore()
    {
        var first = m_startHealth * PowerDropped;
        return (int)Math.Round(first, 0, MidpointRounding.ToEven);
    }
}
