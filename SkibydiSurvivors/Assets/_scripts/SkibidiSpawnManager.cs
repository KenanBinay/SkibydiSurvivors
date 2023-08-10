using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SkibidiSpawnManager : MonoBehaviour
{
    public List<GameObject> skibidies = new List<GameObject>();

    [SerializeField] Vector3 spawnArea;
    [SerializeField] float spawnTimer;
    [SerializeField] GameObject player;

    float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            SpawnEnemy();
            timer = spawnTimer;
        }
    }

    void SpawnEnemy()
    {
        Vector3 position = GenerateRandomPosition();

        position += player.transform.position;

        GameObject newEnemy = Instantiate(skibidies[0]);
        newEnemy.transform.position = position;
        newEnemy.GetComponent<enemyController>().SetTarget(player);
        newEnemy.transform.parent = transform;
    }

   Vector3 GenerateRandomPosition()
    {
        Vector3 position = new Vector3();

        float f = UnityEngine.Random.value > 0.5f ? -1f : 1f;
        if (UnityEngine.Random.value > 0.5f)
        {
            position.x = UnityEngine.Random.Range(-spawnArea.x, spawnArea.x);
            position.z = spawnArea.z * f;
        }
        else
        {
            position.z = UnityEngine.Random.Range(-spawnArea.z, spawnArea.z);
        }

        position.y = 0;

        return position;
    }
}
