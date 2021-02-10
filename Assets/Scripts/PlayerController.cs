using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Gameplay")]
    public float jumpForce;
    public float moveSpeed;

    [Header("Settings")]
    public LayerMask EnvLayerMask;
    public Animator Anim;

    private Rigidbody2D Body;
    private CapsuleCollider2D Coll2D;
    public float MaxSpeed;
    public float FallSpeed;

    const string JUMP_KEY = "jump";
    const string LAND_KEY = "land";

    bool Jumping = false;

    [SerializeField]
    bool Grounded = false;


    // Start is called before the first frame update
    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        Coll2D = GetComponent<CapsuleCollider2D>();
        Anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>())
            Debug.Log("Player touched by an enemy!");
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");
        Body.AddForce(new Vector2(horizontalInput, 0) * moveSpeed);

        if (Input.GetButtonDown("Jump") && Grounded)
        {
            Body.AddForce(new Vector2(0, jumpForce));
            Anim.SetTrigger(JUMP_KEY);
            Jumping = true;
        }

        Body.velocity = new Vector2(Mathf.Max(Mathf.Min(Body.velocity.x, MaxSpeed), -MaxSpeed), Body.velocity.y);

        bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, Coll2D.size.y + Coll2D.offset.y + 0.1f, EnvLayerMask);

        if (isGrounded && !Grounded)
        {
            if (Jumping)
            {
                Anim.SetTrigger(LAND_KEY);
            }

            Jumping = false;
        }

        Grounded = isGrounded;
    }   
}
