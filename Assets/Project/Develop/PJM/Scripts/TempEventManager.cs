using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEventManager : MonoBehaviour
{
    public static TempEventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

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
