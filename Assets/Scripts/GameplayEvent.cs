using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayEvent : MonoBehaviour
{
    public static event Action OnEnemyDestroyedE;
    public static event Action OnTimeIsUpE;
    public static event Action OnDoneMovingE;
    public static event Action OnStartRunningE;
    
    public static void OnEnemyDestroyed()
    {
        OnEnemyDestroyedE?.Invoke();
    }

    public static void OnTimeIsUp()
    {
        OnTimeIsUpE?.Invoke();
    }

    public static void OnDoneMoving()
    {
        OnDoneMovingE?.Invoke();
    }

    public static void OnStartRunning()
    {
        OnStartRunningE?.Invoke();
    }
}
