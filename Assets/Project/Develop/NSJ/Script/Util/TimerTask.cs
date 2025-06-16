using System;
using UnityEngine;

public class TimerTask
{
    public TimerKeyInfo Key;              // 자신을 식별하는 키
    public bool IsLoop;              // 반복 여부
    public float Duration;           // 타이머 길이
    public float Elapsed = 0;            // 누적 시간
    public Action Callback;          // 호출할 콜백

    public TimerTask(float duration, Action callback, bool isLoop = false)
    {
        Duration = duration;
        Callback = callback;
        IsLoop = isLoop;
    }

    /// <summary>
    /// 매 프레임마다 호출됨
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
        Elapsed += deltaTime;
        //Debug.Log(Elapsed);
        if (Elapsed > Duration)
        {
            Invoke();

            if (IsLoop)
            {
                Elapsed = 0; // 반복이면 초기화
            }
            else
            {
                UnRegister(); // 일회성은 제거 요청
            }
        }
    }

    /// <summary>
    /// 키 설정 (등록 시 호출)
    /// </summary>
    /// <param name="key"></param>
    public void SetKey(TimerKeyInfo key)
    {
        Key = key;
    }

    /// <summary>
    /// 콜백 실행
    /// </summary>
    private void Invoke()
    {
        Callback?.Invoke();
    }

    /// <summary>
    /// 매니저에 제거 요청
    /// </summary>
    private void UnRegister()
    {
        TimerSystem.UnRegister(this);
    }
}

