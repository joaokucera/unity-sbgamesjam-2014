using UnityEngine;
using System.Collections;

public class PiranhaBehavior : GenericEnemy
{
    [SerializeField]
    private PolygonCollider2D[] colliders;
    private int currentColliderIndex = 0;

    public void SetCollider(int index)
    {
        colliders[currentColliderIndex].enabled = false;
        currentColliderIndex = index;
        colliders[currentColliderIndex].enabled = true;
    }
}
