using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameController : MonoBehaviour {

    [SerializeField]
    GameObject m_masterGlobalObject;

    [SerializeField]
    GameObject m_playerPrefab;

    [SerializeField]
    GameObject m_enemyPrefab;

    [SerializeField]
    GameObject m_enemySpawnParent;

    public PlayerController Player;
    public Transform[] m_enemySpawnLocations;

    List<EnemyController> m_enemies = new List<EnemyController>();
    GameObject m_playerObject;

    #region MonoBehaviors
    void Awake()
    {
        DontDestroyOnLoad(m_masterGlobalObject);

        GameObject playerObject = Instantiate(m_playerPrefab);
        Player = playerObject.GetComponent<PlayerController>();
        Player.CurrentHealth = Constants.TOTAL_HEALTH;

        SpawnEnemies(m_enemySpawnLocations);
    }

    void Start ()
    {

	}
	
	void Update ()
    {
		
	}
    #endregion

    void SpawnEnemies(Transform[] spawnLocs)
    {
        if (m_enemies.Count < 0)
            m_enemies.Clear();

        foreach(Transform t in spawnLocs)
        {
            GameObject enemyObject = Instantiate(m_enemyPrefab, t.position, t.rotation, m_enemySpawnParent.transform);
            EnemyController enemy = enemyObject.GetComponent<EnemyController>();
            enemy.OnEnemyKilled += OnEnemyKilled;

            enemy.Initialize(Player.gameObject.transform, 
                UnityEngine.Random.Range(Constants.ENEMY_MIN_HEALTH, Constants.ENEMY_MAX_HEALTH), 
                UnityEngine.Random.Range(Constants.ENEMY_MIN_POWER_DROPPED, Constants.ENEMY_MAX_POWER_DROPPED),
                UnityEngine.Random.Range(Constants.ENEMY_MIN_GUN_DAMAGE, Constants.ENEMY_MAX_GUN_DAMAGE));

            m_enemies.Add(enemy);
        }
    }

    void OnEnemyKilled(double powerAmount)
    {
        Player.AddPower(powerAmount);
    }

    public void OnPlayerKilled()
    {
        Debug.Log("Game Over, son");
    }
}
