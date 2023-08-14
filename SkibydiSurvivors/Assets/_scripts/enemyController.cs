using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AI;

public class enemyController : MonoBehaviour
{
    [SerializeField]
    EnemyScriptableObject enemyScriptableObject;

    int enemyHealth, xpAmount, damage;
    float blinkTimer, speed;

    Transform target;
    NavMeshAgent agent;
    SkibidiSpawnManager es;

    public SkinnedMeshRenderer skinnedMeshRenderer;

    public float despawnDistance = 20f;
    public float blinkIntesity;
    public float blinkDuration;

    public bool returning = false;

    private void Start()
    {
        enemyHealth = enemyScriptableObject.health;
        xpAmount = enemyScriptableObject.xp;
        damage = enemyScriptableObject.attackDamage;
        speed = enemyScriptableObject.speed;

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        es = FindObjectOfType<SkibidiSpawnManager>();
        agent = GetComponent<NavMeshAgent>();

        agent.speed = speed;
    }

    public void SetTarget(Transform player)
    {
        target = player;
    }

    public void TakeDamage(int amount)
    {
        enemyHealth -= amount;
        if (enemyHealth <= 0)
        {
            Die();
        }

        blinkTimer = blinkDuration;
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

        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntesity) + 1f;
        foreach (var materials in skinnedMeshRenderer.materials)
        {
            materials.color = Color.white * intensity;
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
        transform.position = target.position + es.relativeSpawnPoints[Random.Range(0
            , es.relativeSpawnPoints.Count)].position;
    }
}
