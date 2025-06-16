using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TimerKey
{
    // 내부에서만 할당되며 외부 생성 불가 (안전)
    internal int ID { get; }
    public int Generation { get; private set; } = 0;

    internal TimerKey(int id)
    {
        ID = id;
    }

    public void UpdateGeneration()
    {
        Generation++;
    }
}