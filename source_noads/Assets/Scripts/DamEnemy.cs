using UnityEngine;
using System.Collections;

public class DamEnemy : GenericEnemy
{
    #region [ FIELDS ]

    private Vector2 damSpriteSize;

    #endregion

    #region [ METHODS ]

    void Start()
    {
        if (transform.childCount > 0)
        {
            damSpriteSize = transform.GetChild(0).renderer.bounds.size;
        }
    }

    protected override void Update()
    {
        transform.Translate(new Vector2(-speed * Time.deltaTime, 0));

        if ((transform.position.x + damSpriteSize.x / 2) < -(Camera.main.aspect * Camera.main.orthographicSize))
        {
            Destroy(gameObject);
        }

    }

    #endregion
}
