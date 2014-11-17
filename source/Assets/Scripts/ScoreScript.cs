using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour 
{
    public bool isGameOver;

    [SerializeField]
    private SpriteRenderer gameOver = null, lifeHUD = null;
    [SerializeField]
    private Animator guiTextLifeAnimator;

    void EnableGameOver()
    {
        gameOver.enabled = true;
        lifeHUD.enabled = false;

        guiTextLifeAnimator.SetTrigger("IsGameOver");

        GetComponent<GameOver>().enabled = true;
    }

}
