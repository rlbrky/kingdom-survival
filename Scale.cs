using UnityEngine;
using DG.Tweening;

public class Scale : MonoBehaviour
{
    [SerializeField] private Vector3 desiredScale;
    [SerializeField] private Vector3 desiredRotation;
    [SerializeField] private float scaleTime = 0.5f;
    private Vector3 _originalScale;
    
    void Start()
    {
        OnScale();
    }

    private void OnScale()
    {
        transform.DOScale(desiredScale, scaleTime)
            .SetEase(Ease.InOutSine);
        transform.DORotate(desiredRotation, scaleTime)
            //.SetLoops(-1, LoopType.Restart)
            .SetRelative()
            .SetEase(Ease.Linear);
        /* Loops our scaling to get a breathing effect on this scale code.
         .OnComplete(() =>
        {
            transform.DOScale(_originalScale, scaleTime)
                .SetEase(Ease.OutBounce)
                .SetDelay(1.0f)
                .OnComplete(OnScale);
        });*/
    }
}
