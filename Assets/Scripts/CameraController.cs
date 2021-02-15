using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(BoxCollider2D))]
public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public Transform Player;

    public BoxCollider2D PlayerZone;
    public float LerpFactor;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Player)
        {
            //Vector3 viewportPos = Camera.main.WorldToViewportPoint(Player.position);

            if (Player.position.x < (transform.position.x + PlayerZone.offset.x - PlayerZone.size.x * .5f) || Player.position.x > (transform.position.x + PlayerZone.offset.x + PlayerZone.size.x * .5f) ||
                Player.position.y < (transform.position.y + PlayerZone.offset.y - PlayerZone.size.y * .5f) || Player.position.y > (transform.position.y + PlayerZone.offset.y + PlayerZone.size.x * .5f))
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(Player.position.x - PlayerZone.offset.x, Player.position.y - PlayerZone.offset.y, transform.position.z), LerpFactor);
            }
        }
    }
}
