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
    
    #region MonoBehaviors
    void Awake()
    {
        DontDestroyOnLoad(m_masterGlobalObject);

        GameObject playerObject = Instantiate(m_playerPrefab);
        Player = playerObject.GetComponent<PlayerController>();
        Player.CurrentHealth = Constants.TOTAL_HEALTH;
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
        foreach(Transform t in spawnLocs)
        {
            GameObject enemy = Instantiate(m_enemyPrefab, t.position, t.rotation, m_enemySpawnParent.transform);
            EnemyController controller = enemy.GetComponent<EnemyController>();
            controller.Health = UnityEngine.Random.Range(Constants.ENEMY_MIN_HEALTH, Constants.ENEMY_MAX_HEALTH);
            controller.PowerDropped = UnityEngine.Random.Range(Constants.ENEMY_MIN_POWER_DROPPED, Constants.ENEMY_MAX_POWER_DROPPED);
            controller.OnEnemyKilled += OnEnemyKilled;
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
