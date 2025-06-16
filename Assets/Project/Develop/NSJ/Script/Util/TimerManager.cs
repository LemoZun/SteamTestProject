using System;
using System.Collections.Generic;
using UnityEngine;

public struct TimerSlot
{
    public TimerKey Key;
    public TimerTask Task;

    public TimerSlot(TimerKey key, TimerTask task)
    {
        this.Key = key;
        this.Task = task;
    }
}
public struct TimerKeyInfo
{
    public TimerKey Key;
    public int Generation;

    public TimerKeyInfo(TimerKey key, int generation)
    {
        this.Key = key;
        this.Generation = generation;
    }
}

public class TimerManager : SingleTon<TimerManager>
{
    // 등록된 모든 타이머를 저장
    // private Dictionary<TimerKey, TimerTask> _tasks = new Dictionary<TimerKey, TimerTask>();

    // ID 생성용 카운터 (현재 사용 안 함: TimerKey 직접 new 함)
    private int _idCounter;

    private List<TimerSlot> _tasks = new List<TimerSlot>();

    private Queue<int> _freeIndex = new Queue<int>();

    // 제거 대기 중인 타이머 목록
    private List<TimerTask> _removes = new List<TimerTask>();

    private void Update()
    {
        ProcessRemove();      // 제거 요청된 타이머 처리
        ProcessTaskUpdate();  // 모든 타이머 갱신
    }

    /// <summary>
    /// 1회성 타이머 등록
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public TimerKeyInfo Schedule(float delay, Action callback)
    {
        TimerTask task = new TimerTask(delay, callback);
        return Register(task);
    }

    /// <summary>
    /// 반복 타이머 등록
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public TimerKeyInfo Loop(float delay, Action callback)
    {
        TimerTask task = new TimerTask(delay, callback, true);
        return Register(task);
    }

    /// <summary>
    /// 외부에서 타이머 제거 요청
    /// </summary>
    /// <param name="task"></param>
    public void UnRegister(TimerTask task)
    {
        _removes.Add(task);
    }

    /// <summary>
    /// 특정 키의 타이머 취소
    /// </summary>
    /// <param name="key"></param>
    public void Cancel(TimerKeyInfo key)
    {
        int keyID = key.Key.ID;
        TimerSlot slot = _tasks[keyID];

        if (slot.Key.Generation != key.Generation)
            return;
        slot.Task = null;
       
        _tasks[keyID] = slot;

        _freeIndex.Enqueue(keyID);
    }

    /// <summary>
    /// 모든 타이머 제거
    /// </summary>
    public void ClearAll()
    {
        _tasks.Clear();
    }

    /// <summary>
    /// 타이머 등록 및 키 할당
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    private TimerKeyInfo Register(TimerTask task)
    {
        // 기존에 null 처리된 키가 있다면 재사용
        int id = 0;

        int curGenerator = 0;
        TimerSlot slot = default;

        if (_freeIndex.Count > 0)
        {
            id = _freeIndex.Dequeue();
            slot = _tasks[id];
            slot.Key.UpdateGeneration();
            slot.Task = task;
            _tasks[id] = slot;
        }
        else
        {
            TimerKey key = new TimerKey(_tasks.Count);
            slot = new TimerSlot(key, task);
            _tasks.Add(slot);
        }
       

        curGenerator = slot.Key.Generation;
        TimerKeyInfo keyInfo = new TimerKeyInfo(slot.Key, curGenerator);

        task.SetKey(keyInfo);

        return keyInfo;
    }

    /// <summary>
    /// 제거 대기 리스트 처리
    /// </summary>
    private void ProcessRemove()
    {
        for (int i = 0; i < _removes.Count; i++)
        {
            TimerKeyInfo removeKey = _removes[i].Key;
            Cancel(removeKey); // 실제 딕셔너리에서 null 처리
        }
        _removes.Clear();
    }

    /// <summary>
    /// 타이머들 갱신
    /// </summary>
    private void ProcessTaskUpdate()
    {
        foreach (TimerSlot task in _tasks)
        {
            if (task.Task == null)
                continue;

            task.Task.Update(Time.deltaTime);
        }
    }
}
