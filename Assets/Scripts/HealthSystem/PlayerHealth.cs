using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private CameraShaker shaker;

    [Header("Health")]
    [SerializeField,Range(100,200)] private float startHealth;

    [Header("References")]
    [SerializeField] private SpriteRenderer playerSprite;

    [Header("On Damage Parameters")]
    [SerializeField] private float changedColorTime = .2f;
    public static Action<float> OnTakeDamage;

    public static Action OnDie;

    private Health health;
    public float CurrentHealth => health.CurrentHealth;
    private void Awake()
    {
        health = new Health(startHealth);
        //Debug.Log($"Starting health is: {startHealth}");
    }
    public void TakeDamage(float dmgAmount)
    {
        //shaker.ShakeCamera(1f,1f);
        health.CurrentHealth -= dmgAmount;
        Debug.Log("Damaging Player");
        OnTakeDamage?.Invoke(health.CurrentHealth);
        //For future references.
        //StartCoroutine(damageFlash());
        if(health.CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void HealUnit(float healAmount)
    {
        health.CurrentHealth -= healAmount;
    }

    private void Die()
    {
        Debug.Log("Player Died");
        //Destroy(this.gameObject);
        OnDie?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    

    private IEnumerator damageFlash()
    {
        playerSprite.color = new Color(1,0,0,0.5f);
        yield return new WaitForSeconds(changedColorTime);
        playerSprite.color = Color.white;
    }

}
