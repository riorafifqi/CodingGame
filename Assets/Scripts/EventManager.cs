using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class EventManager
{
    public static event Action OnMovementFinishE;
    public static event Action OnResetLevelE;

    public static void OnMovementFinish()
    {
        OnMovementFinishE?.Invoke();
    }

    public static void OnResetLevel()
    {
        OnResetLevelE?.Invoke();
    }
}
