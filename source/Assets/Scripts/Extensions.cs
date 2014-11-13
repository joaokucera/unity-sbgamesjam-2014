using UnityEngine;
using System.Collections;

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


}

