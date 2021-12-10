using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    private HealthSystem healthSystem;
    private GameManager gameManager;

    public void Setup(GameManager gameManager, HealthSystem healthSystem, int maxHealth)
    {
        this.healthSystem = healthSystem;
        this.gameManager = gameManager;

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    //Triggerd when health is changed
    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        int health = healthSystem.GetHealth();

        slider.value = health;

        if (health == 0)
        {
            FindObjectOfType<AudioManager>().Play("Loss");
            gameManager.gameHasEnded = true;
            gameManager.hasLost = true;
        }
        Debug.Log(healthSystem.GetHealth().ToString());
    }
}
