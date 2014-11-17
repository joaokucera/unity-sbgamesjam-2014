using UnityEngine;
using System.Collections;
using System;

public static class Extensions
{
    public static void SetPositionY(this Transform t, float newYValue)
    {
        t.transform.position = new Vector3(t.position.x, newYValue, t.position.z);
    }

    public static void SetPositionX(this Transform t, float newXValue)
    {
        t.transform.position = new Vector3(newXValue, t.position.y, t.position.z);
    }

    public static void SetPositionZ(this Transform t, float newZValue)
    {
        t.transform.position = new Vector3(t.position.x, t.position.y, newZValue);
    }
}