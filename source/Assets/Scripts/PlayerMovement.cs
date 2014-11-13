using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    #region [ FIELDS ]

    private Vector2 movement, speed, spriteSize;
    private SpriteRenderer spriteRenderer;

    #endregion

    #region [ METHODS ]

    void Start()
    {
        spriteRenderer = renderer as SpriteRenderer;
        spriteSize = spriteRenderer.bounds.size;

        speed = new Vector2(5, 5);
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        movement = new Vector2(
            inputX * speed.x,
            inputY * speed.y);

        EnforceBounds();
    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = movement;
    }

    private void EnforceBounds()
    {
        Rect borders = ScreenResolution.Instance.RiverBorders;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, borders.xMin + spriteSize.x / 2, borders.xMax + spriteSize.x / 2),
            Mathf.Clamp(transform.position.y, borders.yMin + spriteSize.y / 2, borders.yMax + spriteSize.y / 2),
            transform.position.z
        );
    }

    #endregion
}
