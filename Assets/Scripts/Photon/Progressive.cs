using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Progressive : MonoBehaviour
{
    [SerializeField] private float _value;
    public Action OnChange;

    public virtual void Awake()
    {
        OnChange?.Invoke();
    }

    public float Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            OnChange?.Invoke();
        }
    }
    [field: SerializeField] public float MaxValue { get; set; }

    public float Ratio { get { return Value / MaxValue; } }

}
