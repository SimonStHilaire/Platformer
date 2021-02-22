using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Gameplay")]
    public float jumpForce;
    public float moveSpeed;
    public float slowDownSpeed;

    [Header("Settings")]
    public LayerMask EnvLayerMask;
    public Animator Anim;

    private Rigidbody2D Body;
    private CapsuleCollider2D Coll2D;
    public float MaxSpeed;
    public float FallSpeed;

    public AudioSource MyAudioSource;

    public Action OnEnemyCollison;

    public Pool ProjectilePool;

    const string JUMP_KEY = "jump";
    const string LAND_KEY = "land";

    bool Jumping = false;

    [SerializeField]
    bool Grounded = false;

    private SpriteRenderer spriteRenderer;

    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        Coll2D = GetComponent<CapsuleCollider2D>();
        Anim = GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        MyAudioSource = GetComponent<AudioSource>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>())
        {
            OnEnemyCollison?.Invoke();
        }
    }

    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalInput) > Mathf.Epsilon)
        {
            spriteRenderer.flipX = (horizontalInput < 0);
            Body.AddForce(new Vector2(horizontalInput, 0) * moveSpeed);
        }
        else if(Grounded)
        {
            float xVelocity = Mathf.Lerp(Body.velocity.x, 0f, slowDownSpeed * Time.deltaTime);
            Body.velocity = new Vector2(xVelocity, Body.velocity.y);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            SoundController.Instance.playAudio("Shoot", MyAudioSource);

            GameObject pro = ProjectilePool.GetObject();

            pro.transform.position = transform.position;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            bool autoAim = false;

            foreach(GameObject enemy in enemies)
            {
                Vector3 direction = enemy.transform.position - transform.position;

                Debug.DrawRay(transform.position, direction, Color.red, 1f);

                if (!Physics2D.Raycast(transform.position, direction, direction.magnitude, EnvLayerMask))
                {
                    pro.GetComponent<Projectile>().Direction = direction.normalized;
                    autoAim = true;
                    break;
                }
            }

            if (!autoAim)
            {
                if (spriteRenderer.flipX)
                    pro.GetComponent<Projectile>().Direction = new Vector3(-1, 0, 0);
                else
                    pro.GetComponent<Projectile>().Direction = new Vector3(1, 0, 0);
            }

            pro.SetActive(true);

        }

        //float verticalInput = Input.GetAxis("Vertical");
        

        if (Input.GetButtonDown("Jump") && Grounded)
        {
            SoundController.Instance.playAudio("Jump", MyAudioSource);
            Body.AddForce(new Vector2(0, jumpForce));
            Anim.SetTrigger(JUMP_KEY);
            Jumping = true;
        }

        Body.velocity = new Vector2(Mathf.Max(Mathf.Min(Body.velocity.x, MaxSpeed), -MaxSpeed), Body.velocity.y);

        Anim.SetFloat("speed", Mathf.Abs(Body.velocity.x));

        //print(Body.velocity.magnitude);

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
