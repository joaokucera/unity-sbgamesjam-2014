using UnityEngine;
using System.Collections;

public class Boundary
{
    public Vector2 min = Vector2.zero;
    public Vector2 max = Vector2.zero;
}

public class PlayerJoystick : MonoBehaviour
{
    #region [ FIELDS ]

    private PlayerJoystick[] joysticks;
    private float tapTimeDelta = 0.3f;

    public bool touchPad;
    public Rect touchZone;
    public Vector2 deadZone = Vector2.zero;
    public bool normalize = false;
    public Vector2 position;
    public int tapCount;

    private int lastFingerId = -1;
    private float tapTimeWindow;
    private Vector2 fingerDownPos;

    private GUITexture gui;
    private Rect defaultRect;
    private Boundary guiBoundary = new Boundary();
    private Vector2 guiTouchOffset;
    private Vector2 guiCenter;

    #endregion

    void Start()
    {
        joysticks = FindObjectsOfType<PlayerJoystick>() as PlayerJoystick[];
        gui = GetComponent<GUITexture>();

        defaultRect = gui.pixelInset;

        defaultRect.x += transform.position.x * Screen.width;
        defaultRect.y += transform.position.y * Screen.height;

        Vector3 position = transform.position;
        position.x = 0.0f;
        position.y = 0.0f;
        transform.position = position;

        if (touchPad)
        {
            if (gui.texture)
            {
                touchZone = defaultRect;
            }
        }
        else
        {
            guiTouchOffset.x = defaultRect.width * 0.5f;
            guiTouchOffset.y = defaultRect.height * 0.5f;

            guiCenter.x = defaultRect.x + guiTouchOffset.x;
            guiCenter.y = defaultRect.y + guiTouchOffset.y;

            guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
            guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
            guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
            guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
        }
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void ResetJoystick()
    {
        gui.pixelInset = defaultRect;
        lastFingerId = -1;
        position = Vector2.zero;
        fingerDownPos = Vector2.zero;

        if (touchPad)
        {
            Color color = gui.guiText.color;
            color.a = 0.025f;
            gui.color = color;
        }
    }

    public bool IsFingerDown()
    {
        return (lastFingerId != -1);
    }

    public void LatchedFinger(int fingerId)
    {
        if (lastFingerId == fingerId)
        {
            ResetJoystick();
        }
    }

    void Update()
    {
        var count = Input.touchCount;

        if (tapTimeWindow > 0)
            tapTimeWindow -= Time.deltaTime;
        else
            tapCount = 0;

        if (count == 0)
            ResetJoystick();
        else
        {
            for (var i = 0; i < count; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 guiTouchPos = touch.position - guiTouchOffset;

                var shouldLatchFinger = false;
                if (touchPad)
                {
                    if (touchZone.Contains(touch.position))
                        shouldLatchFinger = true;
                }
                else if (gui.HitTest(touch.position))
                {
                    shouldLatchFinger = true;
                }

                if (shouldLatchFinger && (lastFingerId == -1 || lastFingerId != touch.fingerId))
                {
                    if (touchPad)
                    {
                        Color color = gui.guiText.color;
                        color.a = 0.15f;
                        gui.color = color;

                        lastFingerId = touch.fingerId;
                        fingerDownPos = touch.position;
                    }

                    lastFingerId = touch.fingerId;

                    if (tapTimeWindow > 0)
                    {
                        tapCount++;
                    }
                    else
                    {
                        tapCount = 1;
                        tapTimeWindow = tapTimeDelta;
                    }

                    foreach (PlayerJoystick j in joysticks)
                    {
                        if (j != this)
                            j.LatchedFinger(touch.fingerId);
                    }
                }

                if (lastFingerId == touch.fingerId)
                {
                    if (touch.tapCount > tapCount)
                    {
                        tapCount = touch.tapCount;
                    }

                    if (touchPad)
                    {
                        position.x = Mathf.Clamp((touch.position.x - fingerDownPos.x) / (touchZone.width / 2), -1, 1);
                        position.y = Mathf.Clamp((touch.position.y - fingerDownPos.y) / (touchZone.height / 2), -1, 1);
                    }
                    else
                    {
                        Rect inset = gui.pixelInset;
                        inset.x = Mathf.Clamp(guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x);
                        inset.y = Mathf.Clamp(guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y);
                        gui.pixelInset = inset;
                    }

                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        ResetJoystick();
                }
            }
        }

        if (!touchPad)
        {
            position.x = (gui.pixelInset.x + guiTouchOffset.x - guiCenter.x) / guiTouchOffset.x;
            position.y = (gui.pixelInset.y + guiTouchOffset.y - guiCenter.y) / guiTouchOffset.y;
        }

        var absoluteX = Mathf.Abs(position.x);
        var absoluteY = Mathf.Abs(position.y);

        if (absoluteX < deadZone.x)
        {
            position.x = 0;
        }
        else if (normalize)
        {
            position.x = Mathf.Sign(position.x) * (absoluteX - deadZone.x) / (1 - deadZone.x);
        }

        if (absoluteY < deadZone.y)
        {
            position.y = 0;
        }
        else if (normalize)
        {
            position.y = Mathf.Sign(position.y) * (absoluteY - deadZone.y) / (1 - deadZone.y);
        }
    }
}
