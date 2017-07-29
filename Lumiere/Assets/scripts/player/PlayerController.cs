using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Current health
    double m_currentHealth;
    public double CurrentHealth
    {
        get { return m_currentHealth; }
        set { m_currentHealth = value; }
    }

    public double GunDamage = 40;

    float MoveSpeed = 10f;
    float JumpAmount = 5f;

    GameController m_game;
    Rigidbody2D m_rigidBody;
    GunController m_gun;

    bool m_isJumping = false;
    float m_distanceToGround;

    void Start()
    {
        m_game = FindObjectOfType<GameController>();
        m_rigidBody = GetComponentInChildren<Rigidbody2D>();

        m_distanceToGround = GetComponentInChildren<Collider2D>().bounds.extents.y;
    }

    void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        m_rigidBody.velocity = new Vector2(horizontalMove * MoveSpeed, m_rigidBody.velocity.y);

        if(Input.GetButtonDown("Fire1"))
        {
            
        }

    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, (float)(m_distanceToGround));
    }
}
