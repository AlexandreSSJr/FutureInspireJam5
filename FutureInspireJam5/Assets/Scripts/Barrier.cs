using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Barrier : MonoBehaviour
{
    private float maxHealth = 100f;
    private float health;
    public Slider healthSlider;
    private readonly string gameOverScene = "Menu";

    public void Rebuild() {
        if (health > maxHealth / 2) {
            health = maxHealth;
        } else {
            health += maxHealth / 2;
        }
        
        healthSlider.value = health/maxHealth;
    }

    public void Damage(float dmg) {
        health -= dmg;

        if (health <= 0) {
            SceneManager.LoadScene(gameOverScene);
        }
        
        healthSlider.value = health/maxHealth;
    }

    void Awake() {
        health = maxHealth;
        healthSlider.value = health/maxHealth;
    }
}
