using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Pool;

public class enemyController : MonoBehaviour
{
    [SerializeField]
    EnemyScriptableObject enemyScriptableObject;
    float speed, damage, enemyHealth;

    public static Transform target;
    public FloatingText FloatingTextPrefab;
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

    public float invincibilityDuration = 0.2f;
    float invincibilityTimer;
    bool isInvincible;

    private Vector3 desiredVelocity, _moveVector;

    PlayerStats player;

    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material material;
    RenderParams _rp;

    private Matrix4x4 _matrices;

    float flashTime = .15f;
    public Color[] colors;

    private ObjectPool<FloatingText> _textPool;

    private void Start()
    {
        enemyHealth = enemyScriptableObject.health;
        damage = enemyScriptableObject.attackDamage;
        speed = enemyScriptableObject.speed;

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        enemySpawner = FindObjectOfType<SkibidiSpawnManager>();
        player = GameObject.Find("_Inventory&PlayerStats").GetComponent<PlayerStats>();

        for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
        {
            colors[i] = skinnedMeshRenderer.materials[i].color;
        }

        minSqrDist = minDist * minDist;

        _rp = new RenderParams(material);
        _matrices = new Matrix4x4();

        _textPool = new ObjectPool<FloatingText>(CreateFloatingText, null, OnPutBackInPool
            , defaultCapacity: 500);
    }

    private FloatingText CreateFloatingText()
    {
        var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity
            , transform);

        return go;
    }

    public void SetTarget(Transform player)
    {
        target = player;
    }

    private void OnPutBackInPool(FloatingText obj)
    {
        obj.gameObject.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        if (!isInvincible)
        {
            if (FloatingTextPrefab)
            {
                ShowFloatingText(amount);
            }

            StartCoroutine(EFlash());

            enemyHealth -= amount;
            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (enemyHealth <= 0)
            {
                Die();
            }
        }
    }

    void ShowFloatingText(float damageAmount)
    {
        //    var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        var go = _textPool.Get();
        go.GetComponent<TextMeshPro>().text = damageAmount.ToString();
        go.Init(_textPool);
    }

    private void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }

        if (!returning && target != null && Vector3.Distance(transform.position, target.position) >= despawnDistance)
        {
            returning = true;
            ReturnEnemy();
        }
        else returning = false;

        Graphics.RenderMesh(_rp, _mesh, 0, _matrices);
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

    IEnumerator EFlash()
    {
        for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
        {
            skinnedMeshRenderer.materials[i].color = Color.white * 10;
        }

        yield return new WaitForSeconds(flashTime);

        for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
        {
            skinnedMeshRenderer.materials[i].color = colors[i];
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(damage);
        }
    }
}
