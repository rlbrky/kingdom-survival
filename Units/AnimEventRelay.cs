using UnityEngine;
using UnityEngine.Events;

public class AnimEventRelay : MonoBehaviour
{
    // Expose a UnityEvent in the Inspector.
    // Drag Drop any functions and it will call them.
    public UnityEvent OnAnimationEvent;

    // This method will be called by the animation event.
    public void RelayAnimationEvent()
    {
        // Invoke all functions assigned to this event.
        OnAnimationEvent?.Invoke();
    }
}
