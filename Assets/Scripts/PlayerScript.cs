using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform groundCheckLeft;
    [SerializeField] private Transform groundCheckRight;
    //[SerializeField] private GameObject player;

    private bool isGroundedLeft;
    private bool isGroundedRight;
    public static bool isOnLastLevel;
    public static bool isDead;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public AudioSource aS;
    public AudioSource rip;
    private Animator anim;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        isDead = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        anim.SetBool("isWalking", false);

        if (Input.GetKey(KeyCode.D))
            Move(movingLeft: false);
        else if (Input.GetKey(KeyCode.A))
            Move(movingLeft: true);

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (!isGroundedLeft && !isGroundedRight) 
            anim.SetBool("isJumping", true);
        else
            anim.SetBool("isJumping", false);
    }

    private void Move(bool movingLeft)
    {
        if (isDead == false)
        {
            anim.SetBool("isWalking", true);

            if (movingLeft)
            {
                transform.Translate(-movementSpeed * Time.deltaTime, 0, 0);
                sr.flipX = true; //Can change to false
            }
            else
            {
                transform.Translate(movementSpeed * Time.deltaTime, 0, 0);
                sr.flipX = false; //Can change to true
            }
        }
    }

    private void Jump()
    {
        if (!isGroundedLeft && !isGroundedRight && !isDead)
            return;
        if (isDead)
            return;
        
        {rb.velocity = Vector2.up * jumpVelocity;}
        aS.Play();
        
    }

    private void FixedUpdate()
    {
        isGroundedLeft = Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground"));
        isGroundedRight = Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground"));

        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }
    
    public static IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(3);
        GameManager.instance.Respawn();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Goal"))
        {
            GameManager.instance.LoadNextLevel();
            Debug.Log("Woot!");
        }

        if (col.CompareTag("Enemy"))
        {
            if (isDead) return;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            isDead = true;
            rip.Play();
            StartCoroutine(DeathSequence());
        }
    }
}
