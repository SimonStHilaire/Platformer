using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyController : MonoBehaviour
{
    [Header("Gameplay")]
    public float Speed;
    public bool CanDrop;

    [Header("Settings")]
    public LayerMask EnvLayerMask;

    private Rigidbody2D Body;
    private BoxCollider2D Box;

    private int Direction = 1;

    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        Box = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        Body.position += Vector2.right * Time.deltaTime * Direction * Speed;
    }

    void Update()
    {
        bool grounded = Physics2D.Raycast(transform.position, Vector2.down, Box.size.y + 0.05f, EnvLayerMask);

        if (Direction > 0)
        {
            //Look for env obstacle on my right
            RaycastHit2D castResult = Physics2D.Raycast(transform.position, Vector2.right, Box.size.x + 0.05f, EnvLayerMask);

            if (castResult)
            {
                Direction *= -1;
            }
            else if(grounded && !CanDrop)
            {
                //Look for hole on my right
                castResult = Physics2D.Raycast(transform.position + new Vector3(Box.size.x, 0f, 0f), Vector2.down, Box.size.y + 0.05f, EnvLayerMask);

                if(!castResult)
                {
                    Direction *= -1;
                }
            }
        }
        else
        {
            //Look for env obstacle on my left
            RaycastHit2D castResult = Physics2D.Raycast(transform.position, Vector2.left, Box.size.x + 0.05f, EnvLayerMask);

            if (castResult)
            {
                Direction *= -1;
            }
            else if (grounded && !CanDrop)
            {
                //Look for hole on my left
                castResult = Physics2D.Raycast(transform.position - new Vector3(Box.size.x, 0f, 0f), Vector2.down, Box.size.y + 0.05f, EnvLayerMask);

                if (!castResult)
                {
                    Direction *= -1;
                }
            }
        }
    }
}
