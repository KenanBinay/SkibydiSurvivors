using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyController : MonoBehaviour
{
    public int[] enemyHealths;
    public int enemyHealth_ElementNumber, xpAmount;

    GameObject target;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(GameObject player)
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
    }

    void Move()
    {
        agent.SetDestination(target.transform.position);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
