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
    /// The rate that will be removed from the player's power if the mouse is moving
    /// </summary>
    public double MouseMoveRate = 0.002;
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

        double mouseX = Input.GetAxis("Mouse X");
        double mouseY = Input.GetAxis("Mouse Y");

        //If moving player, or moving mouse, else idle
        if (horizontalMove != 0 || verticalMove != 0)
            OnRemovePower(MovingRate);
        else if (mouseX != 0 || mouseY != 0)
            OnRemovePower(MouseMoveRate);
        else
            OnRemovePower(IdleRate);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        //Detect when player gets hit by a bulley
        if(col.gameObject.tag == Constants.BULLET_TAG)
        {
            var bullet = col.gameObject.GetComponent<MoveBulletTrail>();
            if (bullet.Owner != this.gameObject)
            {
                OnBeenShot(bullet.Damage);
                bullet.DestroyEarly();
            }
        }
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
        {
            m_game.OnPlayerKilled();
            CurrentHealth = 0;
        }
    }

    /// <summary>
    /// Player gets shot by an AI
    /// </summary>
    /// <param name="damage"></param>
    void OnBeenShot(double damage)
    {
        OnRemovePower(damage);
    }
}
