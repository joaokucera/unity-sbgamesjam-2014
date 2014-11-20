using UnityEngine;
using System.Collections;

public class ScreenResolution : MonoBehaviour
{
    #region [ FIELDS ]

    public static ScreenResolution Instance;

    private Rect worldBorders, riverBorders;

    #endregion

    #region [ PROPERTIES ]

    public Rect WorldBorders
    {
        get { return worldBorders; }
    }

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

            worldBorders.xMin = -camera.orthographicSize * camera.aspect;
            worldBorders.xMax = camera.orthographicSize * camera.aspect;
            worldBorders.yMin = -camera.orthographicSize;
            worldBorders.yMax = camera.orthographicSize;

            Transform backgroundTransform = GameObject.FindGameObjectWithTag("Background").transform;
            Vector2 backgroundPosition = backgroundTransform.position;
            SpriteRenderer backgroundRenderer = backgroundTransform.renderer as SpriteRenderer;

            Transform terrainTransform = GameObject.FindGameObjectWithTag("Terrain").transform;
            Vector2 terrainPosition = terrainTransform.position;
            //SpriteRenderer terrainRenderer = terrainTransform.renderer as SpriteRenderer;

            riverBorders.xMin = worldBorders.xMin;
            riverBorders.xMax = worldBorders.xMax;
            riverBorders.yMin = terrainPosition.y;// +terrainRenderer.bounds.size.y / 2;
            riverBorders.yMax = backgroundPosition.y - backgroundRenderer.bounds.size.y / 2;
        }
    }

    #endregion
}
