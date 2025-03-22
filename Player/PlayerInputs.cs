using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs instance;

    [SerializeField] private LayerMask buildLayer;

    private void OnEnable()
    {
        instance = this;
        EnableTouchStuff();
    }

    private void OnDisable()
    {
        DisableTouchStuff();
    }

    private void Awake()
    {
        instance = this;
    }
    
    private void Touch_onFingerDown(Finger touchedFinger)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(touchedFinger.screenPosition.x, touchedFinger.screenPosition.y, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildLayer))
        {
            hit.transform.GetComponent<Tile>().TileClicked();
            //crosshairSprite.position = touchedFinger.screenPosition;
            //PlayerSC.instance.transform.LookAt(hit.point, Vector3.up);
        }
        else
        {
            Player_Build.instance.CloseUI();
        }
    }

    private void Touch_onFingerUp(Finger lostFinger)
    {
        
    }

    public void EnableTouchStuff()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += Touch_onFingerDown;
        ETouch.Touch.onFingerUp += Touch_onFingerUp;
        //ETouch.Touch.onFingerMove += Touch_onFingerMove;
    }

    public void DisableTouchStuff()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown -= Touch_onFingerDown;
        ETouch.Touch.onFingerUp -= Touch_onFingerUp;
        //ETouch.Touch.onFingerMove -= Touch_onFingerMove;
        EnhancedTouchSupport.Disable();
    }
}
