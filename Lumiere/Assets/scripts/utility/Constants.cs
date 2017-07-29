using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    /// <summary>
    /// Total health of player
    /// </summary>
    public static readonly double TOTAL_HEALTH = 100;

    public static readonly float ENEMY_MIN_HEALTH = 80;
    public static readonly float ENEMY_MAX_HEALTH = 200;

    public static readonly float ENEMY_MIN_POWER_DROPPED = 5;
    public static readonly float ENEMY_MAX_POWER_DROPPED = 40;

    public static readonly float ENEMY_MIN_GUN_DAMAGE = 5f;
    public static readonly float ENEMY_MAX_GUN_DAMAGE = 20f;

    /// <summary>
    /// The distance the player needs to be within for the enemies to be able to shoot at the player
    /// </summary>
    public static readonly float ENEMY_RANGE_TO_PLAYER = 7f;

    #region Tags
    public static string ENEMY_TAG = "Enemy";
    public static string BULLET_TAG = "Bullet";
    #endregion
}
