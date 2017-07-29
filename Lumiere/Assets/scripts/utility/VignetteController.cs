using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class VignetteController : MonoBehaviour
{

    [SerializeField]
    GameController m_game;

    [SerializeField]
    PostProcessingProfile m_profile;

    PlayerController m_player;

    public float MinimumSmoothness;
    public float MaximumSmoothness;

    void Update()
    {
        var percentage = (m_game.Player.CurrentHealth / 100) * Constants.TOTAL_HEALTH;
        var range = MinimumSmoothness + MaximumSmoothness;
        float betweenOneZero = (float)percentage / 100;
        var value = range * betweenOneZero;
        var inverted = 1 - value;

        //Only set if above minimum
        if (inverted > MinimumSmoothness)
        {
            var vigentte = m_profile.vignette.settings;
            vigentte.smoothness = inverted;
            m_profile.vignette.settings = vigentte;
        }
    }

    public void Start()
    {
        SetDefaultValue();
    }

    public void OnDestroy()
    {
        SetDefaultValue();
    }

    void SetDefaultValue()
    {
        var vigentte = m_profile.vignette.settings;
        vigentte.smoothness = MinimumSmoothness;
        m_profile.vignette.settings = vigentte;
    }
}
