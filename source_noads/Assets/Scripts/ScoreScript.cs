using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour
{
    [HideInInspector]
    public bool isGameOver;

    [SerializeField]
    private SpriteRenderer gameOver = null, lifeHUD = null;
    [SerializeField]
    private Animator guiTextLifeAnimator = null;

    private PlayerMovement playerMovementScript;

    void Awake()
    {
        playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        Vector2 position = transform.position;
        position.x = ScreenResolution.Instance.WorldBorders.xMax - renderer.bounds.size.x / 2 - 0.165f;
        transform.position = position;
    }

    void EnableGameOver()
    {
        gameOver.enabled = true;
        lifeHUD.enabled = false;

        guiTextLifeAnimator.SetTrigger("IsGameOver");

        playerMovementScript.OnEndGame();

        GetComponent<GameOver>().enabled = true;
    }
}