using System.Collections;
using UnityEngine;

public class Manager : Progressive, IDamageable, IHealable
{

    [field: Header("Stats")]
    [SerializeField] private bool _rechargeable = false;
    [SerializeField] private float _rechargeAmount = 5f;
    [SerializeField] private float _rechargeDelay = 3f;
    [SerializeField] private float _rechargeRate = 0.1f;
    private float _timeSinceLastDamage = 0f;

    public override void Awake()
    {
        OnChange.Invoke();
        if (_rechargeable) StartCoroutine(RegeneratorValue());
    }

    private void Update()
    {
        _timeSinceLastDamage += Time.deltaTime;
    }

    private bool CheckMaxValue()
    {
        if (Value >= MaxValue) return true;
        return false;
    }

    private bool CheckMinValue()
    {
        if (Value <= 0) return true;
        return false;
    }

    public void Damage(float damageAmount)
    {
        _timeSinceLastDamage = 0f;

        if (damageAmount > Value)
            damageAmount = Value;

        Value -= damageAmount;

        if (CheckMinValue()) OnBreak();
    }

    public void Heal(float healAmount)
    {
        if (CheckMaxValue())
            return;

        if (healAmount > MaxValue - Value)
            healAmount = MaxValue - Value;
        Value += healAmount;
    }

    private IEnumerator RegeneratorValue()
    {
        if (_timeSinceLastDamage >= _rechargeDelay && !CheckMaxValue())
            Value += _rechargeAmount;

        yield return new WaitForSeconds(_rechargeRate);
        StartCoroutine(RegeneratorValue());
    }

    private void OnBreak()
    {
        
    }
}

public interface IDamageable
{
    void Damage(float amount);
}

public interface IHealable
{
    void Heal(float amount);
}