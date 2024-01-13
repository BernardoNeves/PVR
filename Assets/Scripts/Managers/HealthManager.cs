using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HealthInterface
{

    public void Damage(float damageAmount);

    public void Heal(float healAmount);

}

public abstract class HealthManager : MonoBehaviour, HealthInterface
{
    [Header("Health")]
    public float _currentHealth;
    public float _currentMaxHealth;

    [Header("Shield")]
    public float _currentShield;
    public float _currentMaxShield;

    [Header("Shield Regenaration")]
    public float _shieldRechargeAmount;
    public float _shieldRechargeCooldown;

    [Header("Animator")]
    public Animator _animator;

    private float _timeSinceLastDamage = 0f;

    public float Health
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
        }
    }

    public float MaxHealth
    {
        get
        {
            return _currentMaxHealth;
        }
        set
        {
            _currentMaxHealth = value;
        }
    }

    public float Shield
    {
        get
        {
            return _currentShield;
        }
        set
        {
            _currentShield = value;
        }
    }

    public float MaxShield
    {
        get
        {
            return _currentMaxShield;
        }
        set
        {
            _currentMaxShield = value;
        }
    }

    public HealthManager(float health, float maxHealth, float shield, float maxShield)
    {
        _currentHealth = health;
        _currentMaxHealth = maxHealth;
        CheckMaxHealth();

        _currentShield = shield;
        _currentMaxShield = maxShield;
        CheckMaxShield();
    }

    public virtual void Start()
    {
        StartCoroutine(ShieldRegen());
    }

    public virtual void Update()
    {
        _timeSinceLastDamage += Time.deltaTime;
    }

    public virtual void Damage(float damageAmount)
    {
        _animator.SetTrigger("Hit");
        DamageShield(damageAmount);
    }

    public void DamageShield(float damageAmount)
    {
        _timeSinceLastDamage = 0f;
        _currentShield -= damageAmount;
        CheckMinShield();
    }
    public void DamageHealth(float damageAmount)
    {
        _currentHealth -= damageAmount;
        CheckMinHealth();
    }

    public virtual void Heal(float healAmount)
    {
        _currentHealth += healAmount;
        CheckMaxHealth();
    }

    private void CheckMaxShield()
    {
        if (_currentShield > _currentMaxShield)
        {
            _currentShield = _currentMaxShield;
        }
    }

    private void CheckMinShield()
    {
        if (_currentShield <= 0)
        {
            DamageHealth(-_currentShield);
            _currentShield = 0;
        }
    }
    private void CheckMaxHealth()
    {
        if (_currentHealth > _currentMaxHealth)
        {
            _currentHealth = _currentMaxHealth;
        }
    }
    private void CheckMinHealth()
    {
        if (_currentHealth <= 0)
        {
            OnDeath();
        }
    }

    private IEnumerator ShieldRegen()
    {
        if (_timeSinceLastDamage >= _shieldRechargeCooldown)
        {
            _currentShield += _shieldRechargeAmount;
            CheckMaxShield();
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ShieldRegen());
    }

    public virtual void OnDeath()
    {
        _currentShield = 0; 
        _currentHealth = 0;
        _animator.SetTrigger("Die");
        Destroy(gameObject, 1.6f);
    }

}
