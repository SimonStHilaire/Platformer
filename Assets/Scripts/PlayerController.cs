using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Gameplay")]
    public float jumpForce;
    public float moveSpeed;

    [Header("Settings")]
    public LayerMask EnvLayerMask;

    private Rigidbody2D Body;
    private CapsuleCollider2D Coll2D;
    public float MaxSpeed;
    public float FallSpeed;


    [SerializeField]
    bool Grounded = false;


    // Start is called before the first frame update
    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        Coll2D = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Body.AddForce(new Vector2(horizontalInput, 0) * moveSpeed);
        if (Input.GetButton("Jump") & Grounded)
        {
            Body.AddForce(new Vector2(0,jumpForce));
        }

        Body.velocity = new Vector2(Mathf.Max(Mathf.Min(Body.velocity.x, MaxSpeed), -MaxSpeed), Body.velocity.y);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>())
            Debug.Log("Player touched by an enemy!");
    }

    void Update()
    {
        Grounded = Physics2D.Raycast(transform.position, Vector2.down, Coll2D.size.y + Coll2D.offset.y + 0.1f, EnvLayerMask);
    }
}
