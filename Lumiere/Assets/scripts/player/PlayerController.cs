using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Current health
    double m_currentHealth;
    public double CurrentHealth
    {
        get { return m_currentHealth; }
        set { m_currentHealth = value; }
    }

    /// <summary>
    /// Amount of damage one bullet from the gun will do
    /// </summary>
    public double GunDamage = 40;

    /// <summary>
    /// The rate that will be removed from the player's power if they are stood still, idle
    /// </summary>
    public double IdleRate = 0.001;
    /// <summary>
    /// The rate that will be removed from the player's power if they are moving
    /// </summary>
    public double MovingRate = 0.005;
    /// <summary>
    /// The rate that will be removed from the player's power if the player ha sshot a bullet
    /// </summary>
    public double ShootingRate = 0.01;

    float MoveSpeed = 10f;
    float JumpAmount = 5f;

    GameController m_game;
    Rigidbody2D m_rigidBody;
    GunController m_gun;

    bool m_isJumping = false;
    float m_distanceToGround;

    #region MonoBehaviours
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

        //If moving, else idle
        if (horizontalMove != 0 || verticalMove != 0)
            OnRemovePower(MovingRate);
        else
            OnRemovePower(IdleRate);
    }
    #endregion

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, (float)(m_distanceToGround));
    }

    /// <summary>
    /// Add power (health) to the player
    /// </summary>
    /// <param name="powerAmount">The amount of power to add to the player</param>
    public void AddPower(double powerAmount)
    {
        if (CurrentHealth + powerAmount > Constants.TOTAL_HEALTH)
            CurrentHealth = Constants.TOTAL_HEALTH;
        else
            CurrentHealth += powerAmount;
    }

    /// <summary>
    /// When the Gun Controller shoots a bullet
    /// </summary>
    public void OnShot()
    {
        OnRemovePower(ShootingRate);
    }

    void OnRemovePower(double amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth < 0)
            m_game.OnPlayerKilled();
    }
}
