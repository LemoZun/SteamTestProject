using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bindable<T>
{
    private T _value;

    public T Value
    {
        get { return _value; }
        set
        {
            if (EqualityComparer<T>.Default.Equals(_value, value) == false) // Default를 통해 해당 타입 T에 맞는 기본 비교기 반환
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }
    public Action<T> OnValueChanged;

    public void Bind(Action<T> callback)
    {
        OnValueChanged += callback;
        callback?.Invoke(Value);
    }

    public void Bind(Action<T> callback, T initialValue)
    {
        Value = initialValue;
        Bind(callback);
    }

    public Bindable(T initialValue = default)
    {
        _value = initialValue;
    }
}
