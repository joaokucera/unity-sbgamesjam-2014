using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerSpawner : MonoBehaviour
{
    private Vector2 initialPosition;
    private PlayerMovement playerMovement;
    private bool isRespowning = true;
    private Vector2 direction = new Vector2(1, 0);
    private WaveScript waveScript;
    private float waveOffSet = 5;
    private ScoreScript scoreScript;
    private PlayerHealth playerHealthScript;
    private Spawner spawner;

    void Start()
    {
        waveScript = GameObject.FindGameObjectWithTag("Wave").GetComponent<WaveScript>();
        scoreScript = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScript>();
        playerHealthScript = GetComponent<PlayerHealth>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();

        initialPosition = transform.position;
        initialPosition.y = ScreenResolution.Instance.RiverBorders.yMax + ScreenResolution.Instance.RiverBorders.yMin / 1.5f;
        transform.SetPositionY(initialPosition.y);

        playerMovement = gameObject.GetComponent<PlayerMovement>();

        Respawn();
    }

    void Update()
    {
        if (!isRespowning || scoreScript.isGameOver)
        {
            return;
        }

        rigidbody2D.velocity = direction * playerMovement.Speed.x;

        if (transform.position.x >= 0)
        {
            isRespowning = false;
            SendMessage("KeepIncreasing");
            playerMovement.Contralable = true;

            waveScript.gameObject.transform.position = waveScript.initialPosition - (Vector2.right * waveOffSet);
            waveScript.destroyEverything = false;
            waveScript.respawWave = true;

            SoundEffectsManager.Instance.PlaySfxWaterDive(Time.deltaTime * 5);
        }
    }

    public void Respawn(bool enableComponents = false)
    {
        if (scoreScript.isGameOver) return;

        transform.position = initialPosition;
        isRespowning = true;
        print("LIFE: " + playerHealthScript.Life);
        if (playerHealthScript.FirstObstacleHitScript != null)
        {
            spawner.CreateObstacle(playerHealthScript.FirstObstacleHitScript);
            print(playerHealthScript.FirstObstacleHitScript);
        }
        playerMovement.Contralable = false;
        renderer.enabled = true;

        if (enableComponents)
        {
            GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(s => s.enabled = true);
            GetComponentsInParent<Collider2D>()[0].enabled = true;
            GetComponent<PlayerHealth>().ChangeBehavior(PlayerBehavior.Surfing);
        }
    }

    public void DestroyEverything()
    {
        SoundEffectsManager.Instance.PlaySfxWaterDive(Time.deltaTime);

        waveScript.destroyEverything = true;
    }
}
