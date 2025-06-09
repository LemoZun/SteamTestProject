using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    /*
    public event Action OnEat;
    public event Action OnDie;
    */

    public void CheckEatOrDie()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, TempLayerList.TailMask | TempLayerList.FoodMask);

        if(hit is null)
            return;
        
        if (hit.gameObject.layer == TempLayerList.Food)
            TempEventManager.Instance.RaiseFoodEaten();
        
        if (hit.gameObject.layer == TempLayerList.Tail)
            TempEventManager.Instance.RaiseSnakeDied();
    }
}
