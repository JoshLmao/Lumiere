using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEnemy
{
    public double Health = 100;

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

    void OnKilled()
    {
        Debug.Log("Killed enemy");
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == Constants.BULLET_TAG)
        {
            GameObject enemy = col.gameObject;
            var moveComponent = enemy.GetComponent<MoveBulletTrail>();
            //Do damage and destroy bullet early
            RecieveHit(moveComponent.Damage);
            moveComponent.DestroyEarly();
        }
    }
}
