using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    public float jumpSpeed = 7;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private int lives;
    public GameObject spikes;
    public float maxSpeed = 7;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        lives = 3;
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        if(Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpSpeed;
        }
        else if(Input.GetButtonUp("Jump"))
        {
            if(velocity.y > 0)
            {
                velocity.y = velocity.y * .5f;
            }
        }
        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if(flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        animator.SetBool("grounded",grounded);
        animator.SetFloat("velocityX",Mathf.Abs(velocity.x) / maxSpeed);
        targetVelocity = move * maxSpeed;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Spike"))
        {
            Debug.Log("Hep");
            spikes.GetComponent<Animation>().Play("Spike trap");
            lives--;
            this.transform.position = new Vector3(-1.69f,0,1);
        }
        // if(other.gameObject.CompareTag("Spike"))
        // {
        //     lives--;
        //     Debug.Log("Hep");
        // }
    }
}
