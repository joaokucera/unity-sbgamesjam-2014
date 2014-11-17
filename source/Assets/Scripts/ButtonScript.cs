using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour
{
    [SerializeField]
    private string levelToGo;
    [SerializeField]
    private GUIText pressAnyKeyText = null, creditsText = null;

    private const float BlinkTime = 0.5f;
    private float timeToBlink;

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

        creditsText.transform.Translate(new Vector3(-Time.deltaTime * 0.25f, 0, 0));
        if (creditsText.transform.position.x < 0)
        {
            creditsText.transform.position = new Vector3(3.5f, creditsText.transform.position.y, creditsText.transform.position.z);
        }
    }

    public virtual void Action()
    {
        Application.LoadLevel(levelToGo);
    }
}
