using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class TestTimerSystem : MonoBehaviour
{
    [SerializeField] private int delay;

    [SerializeField] private TimerKeyInfo key1;
    [SerializeField] private TimerKeyInfo key2;
    [SerializeField] private TimerKeyInfo key3;
    void Start()
    {
        key1 = TimerSystem.Schedule(2, TestSchedule);

        StartCoroutine(TestRoutine());
    }

    IEnumerator TestRoutine()
    {
        yield return 3f.GetDelay();
      
        yield return 1f.GetDelay();
        key2= TimerSystem.Loop(2,() => TestThird("ㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋ"));
        yield return 3f.GetDelay();
        TimerSystem.Cancel(key2);
        key3 = TimerSystem.Schedule(1, TestSchedule);
        Debug.Log($"key1 : {key1.Key.ID} / {key1.Generation}");
        Debug.Log($"key2 : {key2.Key.ID} / {key2.Generation}");
        Debug.Log($"key3 : {key3.Key.ID} / {key3.Generation}");
    }

    private void TestSchedule()
    {
        Debug.Log("TestSchedule");
    }

    private void TestLoop() 
    {
        Debug.Log("TestLoop");
    }

    private void TestThird(string message)
    {
        Debug.Log($"TestThird : {message}");
    }

}
