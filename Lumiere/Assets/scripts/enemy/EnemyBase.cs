using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IEnemy
{
    public double Health { get; private set; }
    public double PowerDropped { get; private set; }

    public int ScoreAmount { get { return CalculateScore(); } }
    public double TotalHealth { get; set; }

    /// <summary>
    /// Amount of damage to the players power/health
    /// </summary>
    public double EnemyGunDamage = 10;

    public event Action<EnemyBase> OnEnemyKilled;

    #region  MonoBehaviours

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
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
    #endregion

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
        TotalHealth = health;
    }

    void OnKilled()
    {
        Debug.Log("Killed enemy. Dropped '" + PowerDropped + "' power");
        InvokeEnemyKilled();
        Destroy(this.gameObject);
    }


    void InvokeEnemyKilled()
    {
        if (OnEnemyKilled != null)
            OnEnemyKilled.Invoke(this);
    }

    /// <summary>
    /// Way of calculating score for when enemy is killed rather than being set random
    /// </summary>
    /// <returns></returns>
    protected int CalculateScore()
    {
        var first = TotalHealth * PowerDropped;
        return (int)Math.Round(first, 0, MidpointRounding.ToEven);
    }

}
