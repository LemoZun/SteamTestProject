using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private void Start()
    {
        TempEventManager.Instance.OnFoodEaten += DisableFood;
    }

    private void OnDisable()
    {
        TempEventManager.Instance.OnFoodEaten -= DisableFood;
    }

    private void DisableFood()
    {
        gameObject.SetActive(false);
    }
}
