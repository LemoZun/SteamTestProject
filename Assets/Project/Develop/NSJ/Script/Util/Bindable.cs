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
            if (EqualityComparer<T>.Default.Equals(_value, value) == false) // Default�� ���� �ش� Ÿ�� T�� �´� �⺻ �񱳱� ��ȯ
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
