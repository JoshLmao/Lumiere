using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    GameObject m_masterGlobalObject;

    [SerializeField]
    GameObject m_playerPrefab;

    public PlayerController Player;

    void Awake()
    {
        DontDestroyOnLoad(m_masterGlobalObject);
    }

	void Start ()
    {
        GameObject playerObject = Instantiate(m_playerPrefab);
        Player = playerObject.GetComponent<PlayerController>();
        Player.CurrentHealth = Constants.TOTAL_HEALTH;
	}
	
	void Update ()
    {
		
	}
}
