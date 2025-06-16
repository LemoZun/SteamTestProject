using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : SingleTon<TimerManager>
{

    private Dictionary<int, TimerTask> _timers;
    private int _idCounter;
    private void Update()
    {

    }
}
