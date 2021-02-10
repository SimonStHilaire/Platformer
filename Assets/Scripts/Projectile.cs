using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public float Speed;

    [HideInInspector]
    public Vector3 Direction;
    
    void Update()
    {
        transform.position += Time.deltaTime * Direction * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
