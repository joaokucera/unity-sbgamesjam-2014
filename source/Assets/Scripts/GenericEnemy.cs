using UnityEngine;
using System.Collections;

public class GenericEnemy : MonoBehaviour
{
    //Animator animator;
    float speed = 5.0f;

    void Start()
    {
        //animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(new Vector2(-speed * Time.deltaTime, 0));

        if ((transform.position.x + renderer.bounds.size.x / 2) < -(Camera.main.aspect * Camera.main.orthographicSize))
        {
            Destroy(this.gameObject);
        }
    }
}
