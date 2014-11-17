using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    #region [ FIELDS ]

    private Vector2 movement, spriteSize, speed = new Vector2(5, 5);
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
        spriteRenderer = renderer as SpriteRenderer;
        spriteSize = spriteRenderer.bounds.size;
    }

    void Update()
    {
        if (!contralable)
            return;

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        movement = new Vector2(
            inputX * speed.x,
            inputY * speed.y);

        EnforceBounds();
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
            Mathf.Clamp(transform.position.x, worldBorders.xMin + spriteSize.x / 2, worldBorders.xMax + spriteSize.x / 2),
            Mathf.Clamp(transform.position.y, riverBorders.yMin - spriteSize.y / 2, riverBorders.yMax - spriteSize.y),
            transform.position.y
        );
    }

    #endregion
}
