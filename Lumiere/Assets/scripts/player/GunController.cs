using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    [SerializeField]
    Transform m_firePosition;

    [SerializeField]
    Transform m_bulletTrailPrefab;

    public float FireRate = 0f;
    public LayerMask m_hitMask;
    public float EffectSpawnRate = 10f;

    float m_timeToFire = 0f;
    float m_timeToSpawnEffect = 0f;
    PlayerController m_player;

    void Start()
    {
        m_player = transform.parent.GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (FireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > m_timeToFire)
            {
                m_timeToFire = Time.time + 1 / FireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(m_firePosition.position.x, m_firePosition.position.y);
        RaycastHit2D hit = Physics2D.Raycast(m_firePosition.position, mousePosition - firePointPosition, 100, m_hitMask);

        if (Time.time >= m_timeToSpawnEffect)
        {
            Effect();
            m_timeToSpawnEffect = Time.time + 1 / EffectSpawnRate;
        }

        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);
        if(hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Debug.Log("Hit");
        }
    }

    void Effect()
    {
        var bullet = Instantiate(m_bulletTrailPrefab, m_firePosition.position, m_firePosition.rotation);
        bullet.GetComponent<MoveBulletTrail>().Damage = m_player.GunDamage;
    }
}
