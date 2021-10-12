using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public delegate void TimerTickDelegate(int tick);
    public TimerTickDelegate OnTimerTick;
    public delegate void TimerEndDelegate();
    public TimerEndDelegate OnTimerEnd;

    public int StartingTime = 60;

    public bool IgnorePause = false;

    private bool timerStarted = false;
    private float currentTime;
    private int previousTickTime;

    public void StartTimer()
    {
        timerStarted = true;
        currentTime = StartingTime;
        OnTimerTick?.Invoke(StartingTime);
        previousTickTime = StartingTime;
    }

    public void ResetTimer()
    {
        timerStarted = false;
        currentTime = StartingTime;
        previousTickTime = StartingTime;
        OnTimerTick?.Invoke(StartingTime);
    }

    private void Update()
    {
        if (!timerStarted) return;
        if (GameStateManager.IsPaused && !IgnorePause) return;

        currentTime -= Time.unscaledDeltaTime;

        var tickTime = (int)(currentTime+1f);
        if(tickTime != previousTickTime && tickTime >= 0f)
        {
            OnTimerTick?.Invoke(tickTime);
        }

        if(currentTime <= 0)
        {
            OnTimerEnd?.Invoke();
            timerStarted = false;
        }
    }
}