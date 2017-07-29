﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackHealth : MonoBehaviour {

    [SerializeField]
    Image m_image;

    [SerializeField]
    PlayerController m_player;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        var percentage = (m_player.CurrentHealth / 100) * Constants.TOTAL_HEALTH;
        m_image.fillAmount = (float)percentage / 100;
    }
}