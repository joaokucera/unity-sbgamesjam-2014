using UnityEngine;
using System.Collections;

public enum PlayerBehavior
{
    Surfing = 0,
    Crouch = 1,
    Die = 2
}

class PlayerHealth : MonoBehaviour
{
    #region [ FIELDS ]

    private const int StartLife = 3;
    private int currentLife;

    private PlayerBehavior playerBehavior = PlayerBehavior.Surfing;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D currentBoxCollider;
    [HideInInspector]
    public ObstacleType obstacleType;
    private Animator animator;
    private int behaviourIndexParameter = Animator.StringToHash("BehaviourIndex");
    private bool keepIncrease = true;
    private float increaseTime = 0;

    private const float BlinkTime = 0.2f;
    private float timeToBlink;

    private const float LoseMultiplierTime = 4f;
    private float timeToLoseMultiplier;

    private float FirstBonusTimer = 12f;
    private float timeToMultiplierFirstBonus;

    private int multiplier = 1;
    private const int MaxBonus = 5;

    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private BoxCollider2D[] colliders;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private GUIText guiTextMeters;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private Sprite[] multiplierSprites;

    private SpriteRenderer multiplierRenderer;
    private Animator multiplierAnimator;
    private int boomHash = Animator.StringToHash("Boom");
    private ScoreScript scoreScript;
    private GenericEnemy firstObstacleHitScript;

    #endregion

    #region [ PROPERTIES ]

    public int Life { get { return currentLife; } }
    public GenericEnemy FirstObstacleHitScript { get { return firstObstacleHitScript; } }

    #endregion

    #region [ METHODS ]

    void Start()
    {
        scoreScript = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScript>();

        animator = GetComponentInChildren<Animator>();
        spriteRenderer = renderer as SpriteRenderer;
        currentBoxCollider = colliders[0] as BoxCollider2D;
        currentBoxCollider.enabled = true;

        GameObject multiplierObject = GameObject.FindGameObjectWithTag("Multiplier");
        multiplierRenderer = multiplierObject.renderer as SpriteRenderer;
        multiplierAnimator = multiplierObject.GetComponent<Animator>();

        currentLife = StartLife;
    }

    private void ChangeMultiplier(int value)
    {
        // ANIMATOR
        multiplierAnimator.SetTrigger(boomHash);

        timeToMultiplierFirstBonus = 0;
        timeToLoseMultiplier = 0;

        multiplier += value;
        if (value > 0 && multiplier <= MaxBonus)
        {
            SoundEffectsManager.Instance.PlaySfxBonus(Time.deltaTime);
        }
        multiplier = Mathf.Clamp(multiplier, 1, MaxBonus);

        if (multiplier == 1)
        {
            multiplierRenderer.sprite = null;
        }
        else
        {
            int index = multiplier - 1;
            multiplierRenderer.sprite = multiplierSprites[index];
        }
    }

    void Update()
    {
        if (scoreScript.isGameOver)
            return;

        // PONTOS.
        if (keepIncrease)
        {
            increaseTime += multiplier * Time.deltaTime;
            guiTextMeters.text = increaseTime.ToString("0");

            // BÔNUS.
            if (currentLife == StartLife)
            {
                timeToMultiplierFirstBonus += Time.deltaTime;

                if (timeToMultiplierFirstBonus >= FirstBonusTimer)
                {
                    ChangeMultiplier(1);
                }
            }
            else
            {
                timeToLoseMultiplier += Time.deltaTime;

                if (timeToLoseMultiplier >= LoseMultiplierTime)
                {
                    ChangeMultiplier(-1);
                }
            }
        }
        else
        {
            timeToBlink += Time.deltaTime;
            if (timeToBlink > BlinkTime)
            {
                timeToBlink = 0;
                guiTextMeters.enabled = !guiTextMeters.enabled;
            }
        }
    }

    public void KeepIncreasing()
    {
        if (!scoreScript.isGameOver)
        {
            keepIncrease = true;
            guiTextMeters.enabled = true;
        }
    }

    public void ChangeBehavior(PlayerBehavior nextBehavior)
    {
        playerBehavior = nextBehavior;
        animator.SetInteger(behaviourIndexParameter, (int)playerBehavior);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Contains("Obstacle"))
        {
            GenericEnemy obstacleScript;
            if (collider.name.Contains("Dam"))
            {
                obstacleScript = collider.transform.parent.GetComponent<GenericEnemy>();
            }
            else
            {
                obstacleScript = collider.GetComponent<GenericEnemy>();
            }

            if (TryChangeBoard(obstacleScript.ObstacleType))
            {
                if (firstObstacleHitScript == null)
                {
                    firstObstacleHitScript = obstacleScript;
                }

                Vector2 spawnPosition = transform.position;
                spawnPosition.x += renderer.bounds.size.x / 2;

                Instantiate(explosionPrefab, spawnPosition, Quaternion.identity);
                Destroy(obstacleScript.gameObject);

            }
        }
        else if (collider.CompareTag("Wave"))
        {
            SoundEffectsManager.Instance.PlaySfxPlayerOuch(Time.deltaTime);
            GetHurt();
            GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>().AccelerateItens();
        }
    }

    private bool TryChangeBoard(ObstacleType obstacleType)
    {
        if (this.obstacleType == obstacleType)
        {
            ChangeBehavior(PlayerBehavior.Crouch);
            ChangeMultiplier(1);

            return true;
        }
        else
        {
            Camera.main.GetComponent<CameraShake>().Shake();

            SoundEffectsManager.Instance.PlaySfxPlayerCollision(Time.deltaTime);
            SoundEffectsManager.Instance.PlaySfxPlayerOuch(Time.deltaTime * 5);

            if (obstacleType == ObstacleType.Alligator)
            {
                SoundEffectsManager.Instance.PlaySfxAlligatorBite(Time.deltaTime * 2);
            }

            this.obstacleType = obstacleType;
            int index = (int)obstacleType;

            currentBoxCollider.size = colliders[index].size;
            currentBoxCollider.center = colliders[index].center;

            spriteRenderer.sprite = sprites[index];

            GetHurt();

            return false;
        }
    }

    private void GetHurt()
    {
        keepIncrease = false;

        currentLife--;
        InterfaceScript.Instance.UpdateInterface(currentLife);
        ChangeMultiplier(-multiplier + 1);

        ChangeBehavior(PlayerBehavior.Die);

        if (currentLife <= 0)
        {
            scoreScript.isGameOver = true;

            Animator scoreAnimator = GameObject.FindGameObjectWithTag("Score").GetComponent<Animator>();
            scoreAnimator.SetTrigger("IsGameOver");

            SoundEffectsManager.Instance.PlaySfxFail(Time.deltaTime * 10);
        }
    }

    #endregion
}
