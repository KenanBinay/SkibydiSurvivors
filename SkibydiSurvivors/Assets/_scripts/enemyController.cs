using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyController : MonoBehaviour
{
    public int[] enemyHealths;
    public int enemyHealth_ElementNumber, xpAmount;

    Transform target;
    NavMeshAgent agent;
    SkibidiSpawnManager es;

    public float despawnDistance = 20f;

    public bool returning = false;

    private void Start()
    {
        es = FindObjectOfType<SkibidiSpawnManager>();
        agent = GetComponent<NavMeshAgent>();      
    }

    public void SetTarget(Transform player)
    {
        target = player;
    }

    public void TakeDamage(int amount)
    {
        enemyHealths[enemyHealth_ElementNumber] -= amount;
        if (enemyHealths[enemyHealth_ElementNumber] <= 0)
        {
            Die();
        }
    }
    private void Update()
    {
        if (!returning) Move(); 

        if (!returning && target != null && Vector3.Distance(transform.position, target.position) >= despawnDistance)
        {
            returning = true;
            ReturnEnemy();
        }
        else returning = false; 
    }

    void Move()
    {
        agent.SetDestination(target.transform.position);
    }

    void Die()
    {
        SkibidiSpawnManager es = FindObjectOfType<SkibidiSpawnManager>();
        es.OnEnemyKilled();

        Destroy(gameObject);
    }

    void ReturnEnemy()
    {
        transform.position = target.position + es.relativeSpawnPoints[Random.Range(0
            , es.relativeSpawnPoints.Count)].position;
    }
}
