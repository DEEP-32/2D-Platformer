using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    TMP_Text healthText;

    private void Awake()
    {
        healthText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        healthText.text = "Health : " + 100;
    }
    private void OnEnable()
    {
        PlayerHealth.OnTakeDamage += UpdateHealth;
    }
    private void OnDisable()
    {
        PlayerHealth.OnTakeDamage -= UpdateHealth;
    }

    private void UpdateHealth(float currentHealth)
    {
        healthText.text = "Health : "+ currentHealth.ToString("00");
    }
}
