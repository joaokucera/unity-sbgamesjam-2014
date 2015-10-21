using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    #region [ FIELDS ]

    [SerializeField]
    private PlayerJoystick moveTouchPad;

    private Vector2 movement, spriteSize, speed = new Vector2(4, 4);
    private SpriteRenderer spriteRenderer;
    private bool contralable;

    #endregion

    #region [ PROPERTIES ]

    public Vector2 Speed { get { return speed; } }

    public bool Contralable { set { contralable = value; } }

    #endregion

    #region [ METHODS ]

    void Start()
    {
#if UNITY_EDITOR || UNITY_WEBPLAYER
        moveTouchPad.guiTexture.enabled = false;
#endif

        spriteRenderer = renderer as SpriteRenderer;
        spriteSize = spriteRenderer.bounds.size;
    }

    public void OnEndGame()
    {
        moveTouchPad.Disable();
    }

    void Update()
    {
        if (!contralable)
            return;

        Vector2 input = Vector2.zero;

#if UNITY_EDITOR || UNITY_WEBPLAYER
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
#else
        input = UpdateTouchPad();
#endif

        movement = new Vector2(
            input.x * speed.x,
            input.y * speed.y);

        EnforceBounds();
    }

    private Vector2 UpdateTouchPad()
    {
        var movement = moveTouchPad.position;

        return movement;
    }

    private Vector2 GetAcceleration()
    {
        Vector2 accelerationPosition = Vector2.zero;

        if (Input.accelerationEventCount > 0)
        {
            accelerationPosition = Input.acceleration;
            accelerationPosition.y += 0.75f;

            if (accelerationPosition.sqrMagnitude > 1)
            {
                accelerationPosition.Normalize();
            }
        }

        return accelerationPosition;
    }

    void FixedUpdate()
    {
        if (!contralable)
            return;

        rigidbody2D.velocity = movement;
    }

    private void EnforceBounds()
    {
        Rect riverBorders = ScreenResolution.Instance.RiverBorders;
        Rect worldBorders = ScreenResolution.Instance.WorldBorders;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, worldBorders.xMin + spriteSize.x / 2, worldBorders.xMax - spriteSize.x / 2),
            Mathf.Clamp(transform.position.y, riverBorders.yMin + spriteSize.y / 2, riverBorders.yMax - spriteSize.y / 2),
            transform.position.y
        );
    }

    #endregion
}
