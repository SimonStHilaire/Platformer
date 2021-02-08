using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{
    [Header("Gameplay")]
    public float Speed;
    public bool CanDrop;

    [Header("Settings")]
    public LayerMask EnvLayerMask;

    private Rigidbody2D Body;
    private CapsuleCollider2D Coll2D;
    public float MaxSpeed;
    public float FallSpeed;

    private int Direction = 1;

    [SerializeField]
    bool Grounded = false;

    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        Coll2D = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        Body.AddForce(Vector2.right * Direction * Speed);

        if (!Grounded)
            Body.AddForce(Vector2.down * FallSpeed);

        Body.velocity = new Vector2(Mathf.Max(Mathf.Min(Body.velocity.x, MaxSpeed), -MaxSpeed), Body.velocity.y);
    }

    void Update()
    {
        Grounded = Physics2D.Raycast(transform.position, Vector2.down, Coll2D.size.y + Coll2D.offset.y + 0.1f, EnvLayerMask);

        if (Direction > 0)
        {
            //Look for env obstacle on my right
            RaycastHit2D castResult = Physics2D.Raycast(transform.position, Vector2.right, Coll2D.size.x + 0.05f, EnvLayerMask);

            if (castResult)
            {
                Direction *= -1;
            }
            else if(Grounded && !CanDrop)
            {
                //Look for hole on my right
                castResult = Physics2D.Raycast(transform.position + new Vector3(Coll2D.size.x, 0f, 0f), Vector2.down, Coll2D.size.y + Coll2D.offset.y + 0.1f, EnvLayerMask);

                if(!castResult)
                {
                    Direction *= -1;
                }
            }
        }
        else
        {
            //Look for env obstacle on my left
            RaycastHit2D castResult = Physics2D.Raycast(transform.position, Vector2.left, Coll2D.size.x + 0.05f, EnvLayerMask);

            if (castResult)
            {
                Direction *= -1;
            }
            else if (Grounded && !CanDrop)
            {
                //Look for hole on my left
                castResult = Physics2D.Raycast(transform.position - new Vector3(Coll2D.size.x, 0f, 0f), Vector2.down, Coll2D.size.y + Coll2D.offset.y + 0.1f, EnvLayerMask);

                if (!castResult)
                {
                    Direction *= -1;
                }
            }
        }
    }
}
