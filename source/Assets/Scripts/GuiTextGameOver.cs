using UnityEngine;
using System.Collections;

public class GuiTextGameOver : MonoBehaviour {

    [SerializeField]
    private GUIText textGameOver;

    public void ActiveTextGameOver()
    {
        textGameOver.text = guiText.text;

        guiText.enabled = false;
        textGameOver.enabled = true;
    }
}
