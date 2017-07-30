using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    int m_score = 0;
    public int Score
    {
        get { return m_score; }
        set { m_score = value; }
    }

    [SerializeField]
    GameObject m_masterGlobalObject;

    [SerializeField]
    UIController m_ui;

    [SerializeField]
    GameObject m_playerPrefab;

    [SerializeField]
    GameObject m_enemyPrefab;

    [SerializeField]
    GameObject m_enemySpawnParent;

    [SerializeField]
    GameObject m_playerSpawnPosition;

    public PlayerController Player;
    public Transform[] m_enemySpawnLocations;

    public bool IsGameFinished { get; set; }

    List<EnemyController> m_enemies = new List<EnemyController>();
    GameObject m_playerObject;

    #region MonoBehaviors
    void Awake()
    {
        DontDestroyOnLoad(m_masterGlobalObject);

        StartSpawnPlayer();

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

    void OnEnemyKilled(EnemyController enemy)
    {
        Player.AddPower(enemy.PowerDropped);
        AddToScore(enemy.ScoreAmount);
    }


    public void AddToScore(int amount)
    {
        if ((amount % 1) == 0)
            Score += amount;
        else
        {
            //Round if need to
            Score += (int)Math.Round((double)amount, 0, MidpointRounding.ToEven);
        }
    }

    /// <summary>
    /// Spawn player at start of level
    /// </summary>
    void StartSpawnPlayer()
    {
        GameObject playerObject = Instantiate(m_playerPrefab);
        playerObject.transform.position = m_playerSpawnPosition.transform.position;

        Player = playerObject.GetComponent<PlayerController>();
        Player.CurrentHealth = Constants.TOTAL_HEALTH;

        //Incase prefab is inactive
        if (!playerObject.activeInHierarchy)
            playerObject.SetActive(true);
    }

    public void RestartLevel()
    {
        DestroyAllSpawnedEnemies();

        //Dont reload level. Just change player pos and respawn enemies
        Player.gameObject.transform.position = m_playerSpawnPosition.transform.position;
        Player.SpawnInvulnerability();

        SpawnEnemies(m_enemySpawnLocations);
    }

    void DestroyAllSpawnedEnemies()
    {
        foreach (Transform t in m_enemySpawnParent.transform)
        {
            //Only destroy spawned enemies
            if (t.gameObject.GetComponent<EnemyController>() != null)
                Destroy(t.gameObject);
        }
    }

    public void OnPlayerKilled()
    {
        m_ui.OnGameEnded();
        IsGameFinished = true;
    }

    /// <summary>
    /// Done by the "Restart" button once game has finished
    /// </summary>
    public void OnRestartScene()
    {
        IsGameFinished = false;
        Score = 0;
        m_ui.OnRestartGame();

        Destroy(Player.gameObject);
        DestroyAllSpawnedEnemies();

        StartSpawnPlayer();
        SpawnEnemies(m_enemySpawnLocations);
    }
}
