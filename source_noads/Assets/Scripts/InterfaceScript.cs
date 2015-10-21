using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InterfaceScript : MonoBehaviour
{
    #region [ FIELDS ]

    public static InterfaceScript Instance;

    [SerializeField]
    private Animator waveAnimator;
    [SerializeField]
    private List<Sprite> lifeSprites;

    private int nextWaveParameter = Animator.StringToHash("NextWave");
    private SpriteRenderer spriteRenderer;

    #endregion

    #region [ METHODS]

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        spriteRenderer = renderer as SpriteRenderer;

        Vector2 position = transform.position;
        position.x = ScreenResolution.Instance.WorldBorders.xMin + renderer.bounds.size.x / 2 + 0.165f;
        transform.position = position;
    }

    public void UpdateInterface(int playerLife)
    {
        if (playerLife >= 0)
        {
            StartCoroutine(ChangeHUD(playerLife, 1));
        }
    }

    private IEnumerator ChangeHUD(int index, float waitTime)
    {
        for (float i = 0; i < waitTime; i += Time.deltaTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return 0;
        }

        spriteRenderer.enabled = true;
        spriteRenderer.sprite = lifeSprites[index];

        waveAnimator.SetTrigger(nextWaveParameter);

        if (GameObject.FindObjectOfType<ScoreScript>().isGameOver)
        {
            spriteRenderer.enabled = false;
        }
    }

    #endregion
}
