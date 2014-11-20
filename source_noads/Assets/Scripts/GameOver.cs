using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private string levelToGo;
    [SerializeField]
    private GUIText pressAnyKeyText;

    private const float BlinkTime = 0.5f;
    private float timeToBlink;

    void Start()
    {
        pressAnyKeyText.enabled = true;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Action();
        }

        timeToBlink += Time.deltaTime;
        if (timeToBlink > BlinkTime)
        {
            timeToBlink = 0;
            pressAnyKeyText.enabled = !pressAnyKeyText.enabled;
        }
    }

    public virtual void Action()
    {
        Application.LoadLevel(levelToGo);
    }
}
