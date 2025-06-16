using System;

public static class TimerSystem
{
    /// <summary>
    /// 일회성 타이머 등록
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static TimerKeyInfo Schedule(float delay, Action callback)
    {
        return TimerManager.Instance.Schedule(delay, callback);
    }

    /// <summary>
    /// 반복 타이머 등록
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static TimerKeyInfo Loop(float interval, Action callback)
    {
        return TimerManager.Instance.Loop(interval, callback);
    }

    /// <summary>
    /// 제거 요청
    /// </summary>
    /// <param name="task"></param>
    public static void UnRegister(TimerTask task)
    {
        TimerManager.Instance.UnRegister(task);
    }

    /// <summary>
    /// 특정 타이머 취소
    /// </summary>
    /// <param name="key"></param>
    public static void Cancel(TimerKeyInfo key)
    {
        TimerManager.Instance.Cancel(key);
    }

    /// <summary>
    /// 전체 초기화
    /// </summary>
    public static void ClearAll()
    {
        TimerManager.Instance.ClearAll();
    }
}

