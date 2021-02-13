using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolLifeTime : MonoBehaviour
{

    public float Life = 1.0f;
    private float SpawnTime;
    Pool m_pool;

    public void Init()
    {
        SpawnTime = Time.time;
    }

    public void SetPool(Pool assignedPool)
    {
        m_pool = assignedPool;
    }

    private void Awake()
    {
        SpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - SpawnTime) >= Life)
        {
            gameObject.transform.parent.GetComponent<Pool>().FreeObject(gameObject);
            gameObject.SetActive(false);
            if (m_pool)
            {
                m_pool.FreeObject(gameObject);
            }
        }
    }
}