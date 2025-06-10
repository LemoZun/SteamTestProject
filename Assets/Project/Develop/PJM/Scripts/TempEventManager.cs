using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEventManager : SingleTon<TempEventManager>
{
    public event Action OnFoodEaten;
    public event Action OnSnakeDied;

    public void RaiseFoodEaten()
    {
        OnFoodEaten?.Invoke();
    }

    public void RaiseSnakeDied()
    {
        OnSnakeDied?.Invoke();
    }
}
