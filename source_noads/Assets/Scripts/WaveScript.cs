using UnityEngine;
using System.Collections;

public class WaveScript : MonoBehaviour
{
    #region [ FIELDS ]

    private PlayerSpawner playerSpawner;
    private float waveSpeed = 3f;
    private Vector2 direction = new Vector2(1, 0);

    [HideInInspector]
    public Vector2 initialPosition;
    [HideInInspector]
    public bool respawWave;
    [HideInInspector]
    public bool destroyEverything;
    #endregion

    #region [ METHODS ]

    void Start()
    {
        playerSpawner = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSpawner>();

        initialPosition = new Vector2(ScreenResolution.Instance.RiverBorders.xMin + renderer.bounds.size.x / 6,
                                      ScreenResolution.Instance.RiverBorders.yMax + ScreenResolution.Instance.RiverBorders.yMin / 1.8f);
    }

    void Update()
    {
        if (destroyEverything)
        {
            transform.Translate(direction * waveSpeed * Time.deltaTime);

            if ((transform.position.x - renderer.bounds.size.x / 2) > ScreenResolution.Instance.WorldBorders.xMax)
            {
                playerSpawner.Respawn(true);
                destroyEverything = false;
            }
        }
        else if (respawWave)
        {
            collider2D.enabled = false;
            RespawnWave();
        }

    }

    void RespawnWave()
    {
        transform.Translate(direction * waveSpeed * Time.deltaTime);

        if (transform.position.x > initialPosition.x)
        {
            transform.SetPositionX(initialPosition.x);
            respawWave = false;
            collider2D.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Contains("Obstacle"))
        {
            if (collider.name.Contains("Dam"))
            {
                Destroy(collider.transform.parent.gameObject);
            }
            else
            {
                Destroy(collider.gameObject);
            }
        }
    }
    #endregion
}
