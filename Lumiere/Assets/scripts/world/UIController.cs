using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [SerializeField]
    Slider m_healthSlider;

    GameController m_game;

	void Start ()
    {
        m_game = FindObjectOfType<GameController>();
        m_healthSlider.maxValue = (float)Constants.TOTAL_HEALTH;
	}
	
	void Update ()
    {
        m_healthSlider.value = (float)m_game.Player.CurrentHealth;
    }
}
