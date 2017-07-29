using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Current 
    double m_currentHealth;
    public double CurrentHealth
    {
        get { return m_currentHealth; }
        set { m_currentHealth = value; }
    }

    float MoveSpeed = 10f;
    float JumpAmount = 5f;

    GameController m_game;
    Rigidbody2D m_rigidBody;

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

        //if (Input.GetKeyDown(KeyCode.Space) && !m_isJumping)
        //{
        //    m_isJumping = true;
        //    m_rigidBody.AddForce(new Vector3(0, JumpAmount, 0), ForceMode2D.Impulse);
        //}
        //if (IsGrounded() && m_isJumping)
        //    m_isJumping = false;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, (float)(m_distanceToGround));
    }
}
