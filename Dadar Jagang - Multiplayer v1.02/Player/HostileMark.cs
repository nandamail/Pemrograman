using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileMark : MonoBehaviour
{
    public static HostileMark instance;

    public int currentHealth;

    public int maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        if (UI.instance != null)
        {
            UI.instance.healthSlider.value = currentHealth;
            UI.instance.healthText.text = "Health: " + currentHealth + "/" + maxHealth;
        }
        else
        {
            Debug.LogWarning("UI instance is null. Pastikan UI diatur dengan benar.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageHostile(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        UI.instance.healthSlider.value = currentHealth;
        UI.instance.healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }

    public void HealPlayer(int heal)
    {
        currentHealth += heal;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UI.instance.healthSlider.value = currentHealth;
        UI.instance.healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}