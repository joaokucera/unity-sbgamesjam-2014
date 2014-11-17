using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Spawner : MonoBehaviour
{
    private int waveCount;

    private int incrementador = 0; //incrementa a onda e suas dificuldades
    private float waveTime = 2.5f; //duração da wave
    private float waveTimeCount; //contador
    public float waveProgression = 1.5f;

    //controlam a velocidade de spawn, quanto menor o tempo de spawn, mais próximos os objetos estarão
    private float spawnSpeed;
    private float maxDistance = 3f; //quão longe podem estar
    private float minDistance = 1.5f; //quão perto podem estar
    private float spawnSpeedProgression = 0.2f;

    //velocidade dos itens e do game
    public float maxDeltaItemSpeed = 2f;
    public float deltaItemSpeed = 0;
    public float gameSpeedIncrementor = 1.5f;

    private bool followPlayer;
    private List<Transform> listOfObstacles;
    GameObject[] riverParts;
    Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        spawnSpeed = maxDistance;

        LoadListOfObjects();

        Invoke("SpawnItem", spawnSpeed);

        riverParts = GameObject.FindGameObjectsWithTag("River");
    }

    private void LoadListOfObjects()
    {
        listOfObstacles = Resources.LoadAll<Transform>("Obstacles").ToList();
        listOfObstacles.OrderBy(o => o.GetComponent<GenericEnemy>().ObstacleType);
    }

    void SpawnItem()
    {
        incrementador = Mathf.Clamp(incrementador, 0, listOfObstacles.Count);
        int index = Random.Range(0, incrementador);

        Transform prefab = listOfObstacles[index];

        if (prefab.tag.Contains("Dam"))
        {
            Vector3 positionToSpawn = transform.position;
            Transform dam = Instantiate(prefab, positionToSpawn, Quaternion.identity) as Transform;

            int tilesAmmount = Random.Range(0, 2);
            int tileStartPosition = Random.Range(0, 4);

            if (tilesAmmount > 0)
            {
                Destroy(dam.GetChild(tileStartPosition + 1).gameObject);
            }
            Destroy(dam.GetChild(tileStartPosition).gameObject);

            //------------------------------------------------------incrementa speed para Dam

            GenericEnemy itemScript = dam.GetComponent<GenericEnemy>();

            if (deltaItemSpeed > 0)
            {
                itemScript.speed *= 1.5f / deltaItemSpeed;
            }
        }
        else
        {
            followPlayer = !followPlayer;

            CreateItem(prefab);
        }

        Invoke("SpawnItem", spawnSpeed);
    }

    private void CreateItem(Transform prefab)
    {
        //Spawnando item
        Vector3 positionToSpawn = GetRandomPosition(prefab);

        if (followPlayer)
        {
            positionToSpawn.y = playerTransform.position.y;
        }

        float yMaxLimit = ScreenResolution.Instance.RiverBorders.yMax - prefab.renderer.bounds.size.y / 2;
        if (positionToSpawn.y > yMaxLimit)
        {
            positionToSpawn.y -= (positionToSpawn.y - yMaxLimit);
        }
        float yMinLimit = ScreenResolution.Instance.RiverBorders.yMin + prefab.renderer.bounds.size.y / 2;
        if (positionToSpawn.y < yMinLimit)
        {
            positionToSpawn.y += (positionToSpawn.y - yMaxLimit);
        }

        Transform item = Instantiate(prefab, positionToSpawn, Quaternion.identity) as Transform;
        GenericEnemy itemScript = item.GetComponent<GenericEnemy>();

        //incrementando velocidade baseado no deltaItemSpeed (que deve ser aumentando quando vida diminui)
        if (deltaItemSpeed > 0)
        {
            itemScript.speed *= 1.5f / deltaItemSpeed;
        }

        print("POS: " + item.position.y);
    }

    void Update()
    {
        SpawnIntelligence();
    }

    public void AccelerateItens()
    {
        deltaItemSpeed += gameSpeedIncrementor; //aumenta a velocidade do delta a ser usado

        foreach (GameObject river in riverParts)
        {
            river.GetComponent<ParallaxScript>().parallaxSpeed += gameSpeedIncrementor;
        }
    }

    private void SpawnIntelligence()
    {
        waveTimeCount += Time.deltaTime;

        //se passou uma wave
        if (waveTimeCount > waveTime)
        {
            waveTimeCount = 0f;
            waveCount++;
            waveTime *= waveProgression; //acrescenta progressão ao tempo da 

            if (incrementador < listOfObstacles.Count) //se o incrementador liberará outro item para ser randomizado...
            {
                incrementador++;
            }
            else //se não, são as "pós" waves, que aumentam a dificuldade em si
            {
                spawnSpeed -= spawnSpeedProgression;
            }

            //spawnSpeed = Mathf.Clamp(spawnSpeed, maxSpawnSpeed, initialSpawnSpeed);
            spawnSpeed = Random.Range(minDistance, maxDistance); //randomizar a distancia entre objetos
        }
    }

    /// <summary>
    /// Pega posição random em Y.
    /// </summary>
    private Vector3 GetRandomPosition(Transform itemToSpawn)
    {
        Vector3 position = transform.position;

        position.y = Random.Range(ScreenResolution.Instance.RiverBorders.yMin + itemToSpawn.renderer.bounds.size.x / 2,
                                  ScreenResolution.Instance.RiverBorders.yMax - itemToSpawn.renderer.bounds.size.y / 2);

        return position;
    }

    public void CreateObstacle(GenericEnemy genericEnemy)
    {
        followPlayer = true;

        Transform prefab = listOfObstacles[(int)genericEnemy.ObstacleType - 1];

        CreateItem(prefab);
    }
}
