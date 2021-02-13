using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float Speed;

    [HideInInspector]
    public Vector3 Direction;

    public LayerMask CollisionIgnoreLayer;
    
    void Update()
    {
        transform.position += Time.deltaTime * Direction * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != (int)Mathf.Log(CollisionIgnoreLayer.value, 2))
            Destroy(gameObject);
    }
}
