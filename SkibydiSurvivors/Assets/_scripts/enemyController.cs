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

    public float despawnDistance = 20f;

    private void Start()
    {
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
        Move();

        if (target != null && Vector3.Distance(transform.position, target.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
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
        SkibidiSpawnManager es = FindObjectOfType<SkibidiSpawnManager>();
        transform.position = target.position + es.relativeSpawnPoints[Random.Range(0
            , es.relativeSpawnPoints.Count)].position;
    }
}
