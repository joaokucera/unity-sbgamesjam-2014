using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    //Waves
    private int waveCount;
    private float waveTime = 30.0f;
    private float waveTimeCount;

    private float spawnSpeed = 1f;
    private float maxSpawnSpeed = 0.1f;
    private float speedProgression = 0.1f;

    private int incrementador = 0;
    private List<Transform> listOfObstacles;

    void Start()
    {
        Invoke("SpawnItem", spawnSpeed);
    }

    void SpawnItem()
    {
        incrementador = Mathf.Clamp(incrementador, 0, listOfObstacles.Count);

        int index = Random.Range(0, incrementador);

        Vector3 positionToSpawn = GetRandomPosition(listOfObstacles[index]);
        Instantiate(listOfObstacles[index], positionToSpawn, Quaternion.identity);

        Invoke("SpawnItem", spawnSpeed);
    }

    void Update()
    {
        SpawnIntelligence();
    }

    private void SpawnIntelligence()
    {
        if (spawnSpeed <= maxSpawnSpeed)
            return;

        waveTimeCount += Time.deltaTime;

        //se passou a wave
        if (waveTimeCount > waveTime)
        {
            waveTimeCount = 0.0f;
            waveCount++;

            //primeiramente incrementa o incrementador
            if (incrementador < listOfObstacles.Count)
                incrementador++;
            else
                spawnSpeed -= speedProgression;

            Mathf.Clamp(spawnSpeed, 0, maxSpawnSpeed);
        }
    }

    //pega posição random em Y
    private Vector3 GetRandomPosition(Transform itemToSpawn)
    {
        Vector3 position  = transform.position;
        position.y = Random.Range(-5, 5f);
        return position;
    }
}
