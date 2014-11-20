using UnityEngine;
using System.Collections;
using System;

public enum ObstacleType
{
    Stone = 1,
    Wood = 2,
    Piranha = 3,
    Whirlpool = 4,
    Dam = 5,
    Alligator = 6
}

public class GenericEnemy : MonoBehaviour
{
    #region [ FIELDS ]

    [SerializeField]
    private ObstacleType obstacleType;
    [HideInInspector]
    public float speed = 3f;

    #endregion

    #region [ PROPERTIES ]

    public ObstacleType ObstacleType { get { return obstacleType; } }

    #endregion

    #region [ METHODS ]

    protected virtual void Update()
    {
        transform.Translate(new Vector2(-speed * Time.deltaTime, 0));
        transform.SetPositionZ(transform.position.y);
    }

    #endregion
}