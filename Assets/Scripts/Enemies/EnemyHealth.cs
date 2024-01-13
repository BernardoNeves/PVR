using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealth : HealthManager
{
    [Header("UI")]
    public Healthbar _healthbar;
    public Healthbar _shieldbar;
    public GameObject damageText;

    private EnemySpawner enemySpawner;


    public override void Start()
    {
        base.Start();
        enemySpawner = GameManager.instance.enemySpawner;
    
    }

    public override void Damage(float damageAmount)
    {
        base.Damage(damageAmount);
        if(damageText)
            ShowDamageText(damageAmount);
    }

    public override void Update()
    {
        base.Update();
        _healthbar.SetCurrent(Health);
        _healthbar.SetMax(MaxHealth);

        if (_shieldbar)
        {
            _shieldbar.SetCurrent(Shield);
            _shieldbar.SetMax(MaxShield);
        }
    }

    public EnemyHealth(float health, float maxHealth, float shield, float maxShield) : base(health, maxHealth, shield, maxShield)
    {
        _currentHealth = health;
        _currentMaxHealth = maxHealth;
        _currentShield = shield;
        _currentMaxShield = maxShield;

        if (_currentHealth > _currentMaxHealth)
        {
            _currentHealth = _currentMaxHealth;
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();
        enemySpawner.enemyCount--;
    }

    public void ShowDamageText(float damageAmount)
    {
        GameObject text = Instantiate(damageText, transform.position, Quaternion.identity, transform);
        text.GetComponent<TMP_Text>().text = damageAmount.ToString();
     
    }
}