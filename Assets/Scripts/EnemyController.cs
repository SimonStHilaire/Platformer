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

    public EnemyProjectile ProjectileRef;
    public float MaxSpeed;
    public float FallSpeed;
    public float ShootInterval;

    private Rigidbody2D Body;
    private CapsuleCollider2D Coll2D;

    private int Direction = 1;

    [SerializeField]
    bool Grounded = false;

    float ShootTimer = 0;

    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        Coll2D = GetComponent<CapsuleCollider2D>();

        ShootTimer = ShootInterval;
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

        ShootTimer -= Time.deltaTime;

        /*if(ShootTimer < 0f)
        {
            ShootTimer = ShootInterval;

            if(!Physics2D.Raycast(transform.position, ))
            {

            }
        }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.GetComponent<PlayerProjectile>())
        //    Destroy(gameObject);
    }
}
