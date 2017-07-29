using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [SerializeField]
    Slider m_healthSlider;

    [SerializeField]
    Text m_scoreText;

    GameController m_game;

	void Start ()
    {
        m_game = FindObjectOfType<GameController>();
        m_healthSlider.maxValue = (float)Constants.TOTAL_HEALTH;
	}
	
	void Update ()
    {
        m_healthSlider.value = (float)m_game.Player.CurrentHealth;

        //m_scoreText.text = "Score: " + m_game.Player.Score;
    }
}
