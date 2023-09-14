using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FloatingText : MonoBehaviour
{
    public float DestroyTime = 3f;
    public Vector3 Offset = new Vector3(0, 2, 0);
    public Vector3 RandomizeIntesity = new Vector3(0.5f, 0, 0);
    private ObjectPool<FloatingText> _pool;

    public void Init(ObjectPool<FloatingText> pool)
    {
        gameObject.SetActive(true);
        _pool = pool;
    }

    void Start()
    {
        //  Destroy(gameObject, DestroyTime);

        StartCoroutine(destroyTime());

        transform.localPosition += Offset;
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntesity.x, RandomizeIntesity.x),
            Random.Range(-RandomizeIntesity.y, RandomizeIntesity.y),
            Random.Range(-RandomizeIntesity.z, RandomizeIntesity.z));
    }

    private void LateUpdate()
    {
        var cameraToLookAt = Camera.main;
        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
    }

    IEnumerator destroyTime()
    {
        yield return new WaitForSeconds(DestroyTime);

        _pool.Release(this);
    }
}