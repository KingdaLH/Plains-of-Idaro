using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public AudioSource aS;
    private Animator anim;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            Move(movingLeft: false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Move(movingLeft: true);
        }
        else if (isGroundedLeft && isGroundedRight)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            anim.SetBool("isJumping", true);
        }
    }

    private void Move(bool movingLeft)
    {
        if (isGroundedLeft && isGroundedRight)
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isJumping", false);
        }

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

    private void Jump()
    {
        if (!isGroundedLeft && !isGroundedRight)
            return;

        rb.velocity = Vector2.up * jumpVelocity;
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Goal"))
        {
            GameManager.instance.LoadNextLevel();
            Debug.Log("Woot!");
        }

        if (col.CompareTag("Enemy"))
        {
            GameManager.instance.Respawn();
        }

        /*if (col.gameObject.CompareTag("Platform"))
        {
            Debug.Log("test");
            rb.isKinematic = true;
            player.transform.parent = col.transform;
        }*/
    }
}
