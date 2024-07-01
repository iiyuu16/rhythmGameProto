using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class rhythmHealthManager : MonoBehaviour
{
    public static rhythmHealthManager Instance;
    public Slider healthSlider;
    public int maxHealth = 100;
    private int currentHealth;

    public TextMeshProUGUI failText;
    private Coroutine healthTimerCoroutine;
    public int healthToAdd = 8;
    public float timeToRegainHealth = 5f;
    private float timeSinceLastMiss;

    void Start()
    {
        Instance = this;
        failText.gameObject.SetActive(false);
        currentHealth = maxHealth;
        UpdateHealthSlider();
        timeSinceLastMiss = 0f;
    }

    public void DeductHealth(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthSlider();

        Debug.Log("Deducted " + amount + " health. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            FailState();
        }
        else
        {
            timeSinceLastMiss = 0f;
            if (healthTimerCoroutine != null)
            {
                StopCoroutine(healthTimerCoroutine);
            }
            healthTimerCoroutine = StartCoroutine(HealthRegenerationTimer());
        }
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthSlider();

        Debug.Log("Added " + amount + " health. Current health: " + currentHealth);
    }

    public void UpdateHealthSlider()
    {
        healthSlider.value = currentHealth;
    }

    void FailState()
    {
        if (failText != null)
        {
            failText.gameObject.SetActive(true);
        }
    }

    IEnumerator HealthRegenerationTimer()
    {
        while (timeSinceLastMiss < timeToRegainHealth)
        {
            yield return new WaitForSeconds(1f);
            timeSinceLastMiss += 1f;
        }

        AddHealth(healthToAdd);
        timeSinceLastMiss = 0f;
    }
}
