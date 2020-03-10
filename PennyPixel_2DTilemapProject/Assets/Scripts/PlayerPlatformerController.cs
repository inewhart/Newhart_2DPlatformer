using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerPlatformerController : PhysicsObject
{
    public float jumpSpeed = 7;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private int currentlives;
    public GameObject spikes;
    public GameObject fire;
    public Text lives;
    public float maxSpeed = 7;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.DeleteAll ();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentlives = PlayerPrefs.GetInt("lives",3);
    }
    public void Start()
    {
         
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
        if(PlayerPrefs.GetInt("lives",currentlives) <= 0)
        {
            currentlives = 3;
            PlayerPrefs.SetInt("lives",currentlives);
            
            SceneManager.LoadScene("GameOver");
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Spike"))
        {
            currentlives--;
            PlayerPrefs.SetInt("lives",currentlives);
            // spikes.GetComponent<Animation>().Play("Spike trap");
            StartCoroutine("resetPos");
            Debug.Log(PlayerPrefs.GetInt("lives",currentlives));
        }
        if(other.gameObject.CompareTag("fire"))
        {
            
            Instantiate(fire,new Vector3(this.transform.position.x,this.transform.position.y + 5,0),fire.transform.rotation);
            // Debug.Log(PlayerPrefs.GetInt("lives",currentlives));
        }
        if(other.gameObject.CompareTag("Fireball"))
        {
            currentlives--;
            PlayerPrefs.SetInt("lives",currentlives);
            // Debug.Log(PlayerPrefs.GetInt("lives",currentlives));
            Destroy(other);
            StartCoroutine("resetPos");
        }
    }
    IEnumerator resetPos() 
    {
        yield return new WaitForSeconds(.1f);
        this.transform.position = new Vector3(-1.69f,0,0);
        this.velocity = Vector3.zero;
    }
    public void Update()
    {
        // Debug.Log(PlayerPrefs.GetInt("lives",currentlives));
        lives.text = "Lives: " + PlayerPrefs.GetInt("lives",currentlives);
        base.Update();
    }
}
