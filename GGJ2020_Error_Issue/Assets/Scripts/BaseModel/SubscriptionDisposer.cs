using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class SubscriptionDisposer : MonoBehaviour
{
    public List<IDisposable> subscriptions = new List<IDisposable>();

    void OnDestroy()
    {
        subscriptions.ForEach(s => s.Dispose());
    }
}
