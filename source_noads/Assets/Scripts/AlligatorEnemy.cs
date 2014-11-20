using UnityEngine;
using System.Collections;

public class AlligatorEnemy : GenericEnemy
{
    int direction;
    float verticalSpeed = 1.0f;
    Vector2 spriteSize;
    Rect borders;
    Transform playerTransform;
    PlayerHealth playerHealthScript;

    void Start()
    {
        speed *= 1.25f;
        direction = Random.Range(1, 1000);

        if (direction > 500)
            direction = 1;
        else
            direction = -1;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealthScript = playerTransform.GetComponent<PlayerHealth>();

        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteSize = new Vector2(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y);
        borders = ScreenResolution.Instance.RiverBorders;
    }

    protected override void Update()
    {
        base.Update();

        if (playerHealthScript.obstacleType != ObstacleType.Alligator && 
            transform.position.x + renderer.bounds.size.x / 2  > playerTransform.position.x - playerTransform.renderer.bounds.size.x / 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, verticalSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector2(0, verticalSpeed * direction * Time.deltaTime));
        }

        if (transform.position.y + spriteSize.y / 2 > borders.yMax ||
            transform.position.y - spriteSize.y / 2 < borders.yMin)
        {
            direction *= -1;
        }

        if ((transform.position.x + renderer.bounds.size.x / 2) < -(Camera.main.aspect * Camera.main.orthographicSize))
        {
            Destroy(this.gameObject);
        }

    }
}
