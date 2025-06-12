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
                if (EqualityComparer<T>.Default.Equals(_value, value) == false) // Default를 통해 해당 타입 T에 맞는 기본 비교기 반환
                {
                    _value = value;
                    OnValueChanged?.Invoke(_value);
                }
            }
        }
        public Action<T> OnValueChanged;

        /// <summary>
        /// 바인딩된 콜백을 호출하고, 값이 변경될 때마다 콜백을 호출합니다.
        /// </summary>
        /// <param name="callback"></param>
        public void Bind(Action<T> callback)
        {
            OnValueChanged += callback;
            callback?.Invoke(Value);
        }

        /// <summary>
        /// 바인딩된 콜백을 호출하고, 초기값을 설정합니다. 이후 값이 변경될 때마다 콜백을 호출합니다.
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