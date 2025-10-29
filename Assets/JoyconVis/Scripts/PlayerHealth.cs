using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public List<Image> healthDots;
    private int currentHealth;
    public PlayerDeath dm;
    public static bool isDead = false;

    void Start()
    {
        currentHealth = healthDots.Count;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("it happened");
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log("Player died");
            dm.DeathTime();
            isDead = true;
        }
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < healthDots.Count; i++)
        {
            healthDots[i].enabled = i < currentHealth;
        }
    }
}
