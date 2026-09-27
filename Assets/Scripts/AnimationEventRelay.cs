using UnityEngine;
using System;
public class AnimationEventRelay : MonoBehaviour
{
    public event Action<string> OnAnimationEvent;

    public void TriggerAnimationEvent(string msg)
    {
        OnAnimationEvent?.Invoke(msg);
    }
}
