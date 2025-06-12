using NSJTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private GameObject instance;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            instance = ObjectPool.Get(prefab);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            ObjectPool.Return(instance);
        }

    }
}
