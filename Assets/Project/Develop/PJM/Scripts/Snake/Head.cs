using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{

    public void CheckEatOrDie()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, TempLayerList.TailMask | TempLayerList.FoodMask);

        if(hit == null)
            return;
        
        if (hit.gameObject.layer == TempLayerList.Food)
        {
            Debug.Log("밥 먹음");
        }
        
        if (hit.gameObject.layer == TempLayerList.Tail)
        {
            Debug.Log("게임 오버");
        }
        
    }
    /*private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Tail"))
            Debug.Log("꼬리와 충돌함.");
    }*/
}
