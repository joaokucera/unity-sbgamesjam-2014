using UnityEngine;
using System.Collections;

public class ScenarioSettings : MonoBehaviour
{
    #region [ FIELDS ]

    private Transform backgroundTransform, riverTransform, terrainTransform;

    #endregion

    #region [ METHODS ]

    void Start()
    {
        backgroundTransform = GameObject.FindGameObjectWithTag("Background").transform;
        riverTransform = GameObject.FindGameObjectWithTag("River").transform;
        terrainTransform = GameObject.FindGameObjectWithTag("Terrain").transform;

    }

    #endregion
}
