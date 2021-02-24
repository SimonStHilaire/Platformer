using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXLifeTime : MonoBehaviour
{
    private float Life;

    private void Start()
    {
        Life = gameObject.GetComponent<ParticleSystem>().main.duration;
    }

    // Update is called once per frame
    void Update()
    {
        Life -= Time.deltaTime;

        if (Life < 0)
            Destroy(gameObject);
    }
}
