using UnityEngine;
using System.Collections;

public class ScreenResolution : MonoBehaviour
{
    #region [ FIELDS ]

    public static ScreenResolution Instance;

    private Rect screenBorders;
    private Rect riverBorders;

    #endregion

    #region [ PROPERTIES ]

    public Rect RiverBorders
    {
        get { return riverBorders; }
    }

    #endregion

    #region [ METHODS ]

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        screenBorders = new Rect
        (
            -(camera.orthographicSize * camera.aspect),
            -camera.orthographicSize,
            camera.orthographicSize * camera.aspect,
            camera.orthographicSize
        );

        Transform riverTransform = GameObject.FindGameObjectWithTag("River").transform;
        Vector2 riverPosition = riverTransform.position;
        SpriteRenderer riverRenderer = riverTransform.renderer as SpriteRenderer;

        riverBorders.xMin = riverPosition.x - riverRenderer.bounds.size.x / 2;
        riverBorders.xMax = riverPosition.x - riverRenderer.bounds.size.x / 4;
        riverBorders.yMin = riverPosition.y - riverRenderer.bounds.size.y / 2;
        riverBorders.yMax = riverPosition.y + riverRenderer.bounds.size.y / 2;

        // 96 px
        // 160 px
        // 32 px
    }

    #endregion
}
