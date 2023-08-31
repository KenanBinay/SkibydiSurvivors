using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class enemyController : MonoBehaviour
{
    [SerializeField]
    EnemyScriptableObject enemyScriptableObject;
    float blinkTimer, speed, damage, xpAmount, enemyHealth;

    public static Transform target;
    SkibidiSpawnManager enemySpawner;

    public SkinnedMeshRenderer skinnedMeshRenderer;

    public float despawnDistance = 20f, rotationSpeed;
    public float blinkIntesity;
    public float blinkDuration;

    public bool returning = false;

    public float minDist = 4.0f;
    public float maxDist = 45.0f;

    private float minSqrDist;
    private float sqrDist;

    private Vector3 desiredVelocity, _moveVector;

    PlayerStats player;

    private void Start()
    {
        enemyHealth = enemyScriptableObject.health;
        xpAmount = enemyScriptableObject.xp;
        damage = enemyScriptableObject.attackDamage;
        speed = enemyScriptableObject.speed;

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        enemySpawner = FindObjectOfType<SkibidiSpawnManager>();
        player = GameObject.Find("_Inventory&PlayerStats").GetComponent<PlayerStats>();

        minSqrDist = minDist * minDist;
    }

    public void SetTarget(Transform player)
    {
        target = player;
    }

    public void TakeDamage(float amount)
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
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntesity) + 1f;
        foreach (var materials in skinnedMeshRenderer.materials)
        {
            materials.color = Color.white * intensity;
        }

        if (!returning && target != null && Vector3.Distance(transform.position, target.position) >= despawnDistance)
        {
            returning = true;
            ReturnEnemy();
        }
        else returning = false;
    }

    void Move()
    {
        sqrDist = Vector3.Distance(transform.position, target.position);
        Vector3 direction = target.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion lookAt = Quaternion.RotateTowards(
        transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        transform.rotation = lookAt;

        // modify desiredVelocity if within range
        if (sqrDist > minSqrDist)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position
              , speed * Time.deltaTime);
        }
    }

    void Die()
    {
        SkibidiSpawnManager es = FindObjectOfType<SkibidiSpawnManager>();
        es.OnEnemyKilled();

        Destroy(gameObject);
    }

    void ReturnEnemy()
    {
        transform.position = target.position + enemySpawner.relativeSpawnPoints[Random.Range(0
            , enemySpawner.relativeSpawnPoints.Count)].position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(damage);
        }
    }
}
