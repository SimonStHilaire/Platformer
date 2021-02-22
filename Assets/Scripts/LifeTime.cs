using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    public float Life;

    // Update is called once per frame
    void Update()
    {
        Life -= Time.deltaTime;

        if (Life < 0)
            Destroy(gameObject);
    }
}
