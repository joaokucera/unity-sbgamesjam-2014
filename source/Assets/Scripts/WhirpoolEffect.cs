using UnityEngine;
using System.Collections;

public class WhirpoolEffect : MonoBehaviour {
    PlayerMovement playerMovementScript;

    void Start()
    {
        playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Vector2 v = transform.position - playerMovementScript.transform.position;
        v.Normalize();
        playerMovementScript.rigidbody2D.AddForce(v);
    }
}
