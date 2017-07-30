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
    /// <summary>
    /// The rate that will be removed from the player's power if the player is jumping
    /// </summary>
    public double JumpingRate = 0.008;

    float MoveSpeed = 5f;
    float JumpAmount = 5f;

    GameController m_game;
    Rigidbody2D m_rigidBody;
    GunController m_gun;

    bool m_isJumping = false;
    bool m_canLosePower = false;
    float m_distanceToGround;

    #region MonoBehaviours
    void Start()
    {
        m_game = FindObjectOfType<GameController>();
        m_rigidBody = GetComponentInChildren<Rigidbody2D>();

        m_distanceToGround = GetComponentInChildren<Collider2D>().bounds.extents.y;
        SpawnInvulnerability();
    }

    void Update()
    {
        if (m_game.IsGameFinished)
            return;

        UpdateMovement();

        UpdateJump();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        //Detect when player gets hit by a bulley
        if (col.gameObject.tag == Constants.BULLET_TAG)
        {
            var bullet = col.gameObject.GetComponent<MoveBulletTrail>();
            if (bullet.Owner != this.gameObject)
            {
                OnBeenShot(bullet.Damage);
                bullet.DestroyEarly();
            }
        }
        else if (col.gameObject.tag == Constants.RESTART_LEVEL_TAG)
        {
            m_game.RestartLevel();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer == Constants.FLOOR_LAYER)
        {
            if(m_isJumping)
                StartCoroutine(JumpCooldown(Constants.PLAYER_JUMP_COOLDOWN));
        }
    }
    #endregion

    /// <summary>
    /// When the player spawns, give them invulnerability for a little bit
    /// </summary>
    public void SpawnInvulnerability()
    {
        StartCoroutine(Invulnerability(1.5f));
    }

    void UpdateMovement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        m_rigidBody.velocity = new Vector2(horizontalMove * MoveSpeed, m_rigidBody.velocity.y);

        double mouseX = Input.GetAxis("Mouse X");
        double mouseY = Input.GetAxis("Mouse Y");

        if (m_canLosePower)
        {
            //If moving player, or moving mouse, else idle
            bool isIdle = !m_isJumping && (horizontalMove == 0 && verticalMove == 0);/* && (mouseX == 0 && mouseY == 0);*/
            if (m_isJumping)
                OnRemovePower(JumpingRate);
            if (horizontalMove != 0 || verticalMove != 0)
                OnRemovePower(MovingRate);
            if (mouseX != 0 || mouseY != 0)
                OnRemovePower(MouseMoveRate);
            
            if(isIdle)
                OnRemovePower(IdleRate);
        }
    }

    void UpdateJump()
    {
        float jumpMove = Input.GetAxis("Jump");
        if (jumpMove > 0 && !m_isJumping)
        {
            m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, Constants.PLAYER_JUMP_FORCE);
            m_isJumping = true;
        }
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

        if (CurrentHealth < 0 && !m_game.IsGameFinished)
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

    #region Coroutines
    IEnumerator Invulnerability(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        m_canLosePower = true;
    }

    IEnumerator JumpCooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        m_isJumping = false;
    }
    #endregion
}
