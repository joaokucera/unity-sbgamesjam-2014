using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ParallaxScript : MonoBehaviour
{
    #region [ FIELDS ]

    private float parallaxSpeed = 5;
    private List<Transform> listTransformParts;
    private Vector2 spriteSize;
    private Camera mainCamera;

    #endregion

    #region [ METHODS ]

    void Start()
    {
        mainCamera = Camera.main;
        listTransformParts = new List<Transform>();

        if (transform.childCount > 0)
        {
            spriteSize = transform.GetChild(0).renderer.bounds.size;

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform backgroundPart = transform.GetChild(i);

                if (i > 0)
                {
                    backgroundPart.transform.position = new Vector3(listTransformParts[i - 1].position.x + spriteSize.x,
                                                                    listTransformParts[i - 1].position.y, listTransformParts[i - 1].position.z);
                }

                listTransformParts.Add(backgroundPart);
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < listTransformParts.Count; i++)
        {
            listTransformParts[i].Translate(Vector3.right * -parallaxSpeed * Time.deltaTime);
        }

        if ((listTransformParts[0].position.x + spriteSize.x / 2) < -(mainCamera.aspect * mainCamera.orthographicSize))
        {
            Transform backgroundPart = listTransformParts[0];

            backgroundPart.SetPositionX(listTransformParts[listTransformParts.Count - 1].position.x + spriteSize.x);

            listTransformParts.RemoveAt(0);
            listTransformParts.Add(backgroundPart);
        }
    }

    #endregion
}