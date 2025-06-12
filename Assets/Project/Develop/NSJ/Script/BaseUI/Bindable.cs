using System;
using System.Collections.Generic;

namespace NSJ_MVVM
{
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

        /// <summary>
        /// ���ε��� �ݹ��� ȣ���ϰ�, ���� ����� ������ �ݹ��� ȣ���մϴ�.
        /// </summary>
        /// <param name="callback"></param>
        public void Bind(Action<T> callback)
        {
            OnValueChanged += callback;
            callback?.Invoke(Value);
        }

        /// <summary>
        /// ���ε��� �ݹ��� ȣ���ϰ�, �ʱⰪ�� �����մϴ�. ���� ���� ����� ������ �ݹ��� ȣ���մϴ�.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="initialValue"></param>
        public void Bind(Action<T> callback, T initialValue)
        {
            Value = initialValue;
            Bind(callback);
        }

        public void UnBind(Action<T> callback)
        {
            OnValueChanged -= callback;
        }

        public Bindable(T initialValue = default)
        {
            _value = initialValue;
        }
    }
}