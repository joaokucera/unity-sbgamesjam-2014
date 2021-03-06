﻿using UnityEngine;
using System.Collections;

public class ScenarioSettings : MonoBehaviour
{
    #region [ FIELDS ]

    private Transform backgroundTransform, riverTransform, terrainTransform;
    private float xOffset = 5f;

    #endregion

    #region [ METHODS ]

    void Start()
    {
        // BACKGROUND
        backgroundTransform = GameObject.FindGameObjectWithTag("Background").transform;
        SpriteRenderer backgroundRenderer = backgroundTransform.renderer as SpriteRenderer;

        Vector2 backgroundPosition = backgroundTransform.position;
        backgroundPosition.x = ScreenResolution.Instance.WorldBorders.xMin + backgroundRenderer.bounds.size.x / 2;
        backgroundPosition.y = ScreenResolution.Instance.WorldBorders.yMax - backgroundRenderer.bounds.size.y / 2;
        backgroundTransform.position = backgroundPosition;

        // RIVERS
        Vector2 lastRiverPosition = Vector2.zero;
        for (int i = 5; i > 0; i--)
        {
            riverTransform = GameObject.FindGameObjectWithTag("River" + i).transform;
            SpriteRenderer riverRenderer = riverTransform.renderer as SpriteRenderer;

            Vector2 riverPosition = riverTransform.position;
            riverPosition.x = ScreenResolution.Instance.WorldBorders.xMin + riverRenderer.bounds.size.x / 2;

            if (i == 5)
            {
                riverPosition.y = backgroundPosition.y - backgroundRenderer.bounds.size.y / 2 - riverRenderer.bounds.size.y / 2;
            }
            else
            {
                riverPosition.y = lastRiverPosition.y - riverRenderer.bounds.size.y;
            }

            riverTransform.position = riverPosition;
            lastRiverPosition = riverPosition;
        }

        // TERRAIN
        terrainTransform = GameObject.FindGameObjectWithTag("Terrain").transform;
        SpriteRenderer terrainRenderer = terrainTransform.renderer as SpriteRenderer;

        Vector2 terrainPosition = terrainTransform.position;
        terrainPosition.x = ScreenResolution.Instance.WorldBorders.xMin + terrainRenderer.bounds.size.x / 2;
        terrainPosition.y = ScreenResolution.Instance.WorldBorders.yMin + terrainRenderer.bounds.size.y / 2;
        terrainTransform.position = terrainPosition;

        // Inicia os parallax.
        StartParallax();

        // Inicia o spawner.
        StartSpawner(GameObject.FindGameObjectWithTag("River3").transform.position.y);
    }

    private void StartSpawner(float yStartPosition)
    {
        GameObject spawnerObject = GameObject.FindGameObjectWithTag("Spawner");

        Vector2 spawnerPosition = spawnerObject.transform.position;
        spawnerPosition.x = ScreenResolution.Instance.WorldBorders.xMax + xOffset;
        spawnerPosition.y = yStartPosition;
        spawnerObject.transform.position = spawnerPosition;

        spawnerObject.GetComponent<Spawner>().enabled = true;
    }

    private void StartParallax()
    {
        ParallaxScript[] parallaxScripts = GetComponentsInChildren<ParallaxScript>();
        foreach (var item in parallaxScripts)
        {
            item.enabled = true;
        }
    }

    #endregion
}
