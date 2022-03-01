using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //Player's Components
    #region

    //Player Rigidbody
    private Rigidbody2D rb;

    //Player Animator
    public Animator playerAnimator;

    //Box Collider
    public CapsuleCollider2D playerCollider;

    #endregion

    //Checking Region Transforms
    #region

    //Transform for Feet, to check if on ground
    public Transform feetPosition;

    //Transform for WallCheck, to check if on touching Wall
    public Transform wallCheck;

    //Transform for Head, to check if head is touching ceiling
    public Transform headPosition;

    #endregion

    //Bools
    #region

    //True if on Ground
    private bool isGrounded;

    //True if Can Grab Wall
    private bool canGrab;

    //True if Is Grabbing Wall
    private bool isGrabbing;

    //True if Head Touches Ceiling
    private bool isCeiled;

    //True is Player has a Double Jump Remaining
    private bool canDJ;

    //True is Player is Alive
    private bool isAlive = true;

    //True if Player is facing Right
    private bool isFacingRight = true;

    //True if Player is Crouching
    public bool isCrouching = false;

    //True if Player Can Move
    private bool canMove = true;

    #endregion

    //Floats
    #region

    //Movement Speed
    public float moveSpeed;

    //Jumping Force
    public float jumpForce;

    //Size of Checking Regions
    public float checkRadius;

    //Falling Speeds, Slowest to fastest
    public float fallMultiplier;
    public float lowJumpMultiplier;

    //Time For Input to be disabled after Wall Jump
    private float wallJumpTime = .2f;

    //Counts down after Wall Jump to reinable input
    private float wallJumpCounter;

    #endregion

    //LayerMask defines what counts as Ground
    public LayerMask whatIsGround;

    //Player Atk Offsets
    #region

    public Vector3 standingAttackOffset;

    public Vector3 crouchingAttackOffset;

    #endregion

    //Awake is called before Start, which is before the first frame update
    void Awake()
    {
        //Get Set Player Rigidbody
        rb = GetComponent<Rigidbody2D>();

        //Get Set Player Animator
        playerAnimator = GetComponent<Animator>();

        //Get Set Player Collider
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wallJumpCounter <= 0 && isAlive && canMove)
        {
            //Input For Moving Left and Right
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);

            //Inputs for Other Actions
            if (isGrounded)
            {
                canDJ = true;
                //Transition to Crouch and from
                if (Input.GetKeyDown("down") || Input.GetKeyDown("s"))
                {
                    changeCrouch();
                }
                //Transition to Jump
                if (Input.GetKeyDown("space"))
                {
                    enterJump(isGrabbing);
                }
                //Transition to Atk
                if (Input.GetKeyDown("z"))
                {
                    playerAnimator.SetTrigger("Attack");
                }
            }
            else if (!isGrounded && !isGrabbing)
            {
                if (canDJ && Input.GetKeyDown("space"))
                {
                    canDJ = false;
                    enterJump(isGrabbing);
                }
            }

            //For Falling
            if (rb.velocity.y < -0.1)
            {
                if (isCrouching)
                    exitCrouch();
            }

            //For Wall Grabbing
            #region
            isGrabbing = false;
            if (canGrab && !isGrounded)
            {
                if ((transform.localScale.x > 0f && Input.GetAxisRaw("Horizontal") > 0) || (transform.localScale.x < 0 && Input.GetAxisRaw("Horizontal") < 0))
                {
                    FindObjectOfType<AudioManager>().Play("wallAttach");
                    isGrabbing = true;
                }
            }

            if (isGrabbing)
            {
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;

                canDJ = true;

                if (Input.GetKeyDown("space"))
                {
                    wallJumpCounter = wallJumpTime;

                    enterJump(isGrabbing);
                    rb.gravityScale = 1f;
                    isGrabbing = false;
                }
            }
            else
            {
                rb.gravityScale = 1f;
            }
            #endregion

            //Code For Turning Character Around
            if (rb.velocity.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (rb.velocity.x < 0 && isFacingRight)
            {
                Flip();
            }

            //Checking Region Checks
            #region

            //Check if Player is Grounded
            isGrounded = Physics2D.OverlapCircle(feetPosition.position, checkRadius, whatIsGround);

            //Check if Player is Touching Wall
            canGrab = Physics2D.OverlapCircle(wallCheck.position, checkRadius, whatIsGround);

            //Check if Player is touching Ceiling
            isCeiled = Physics2D.OverlapCircle(headPosition.position, checkRadius, whatIsGround);

            #endregion

        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }

        //Animator Region
        #region

        //Sets Animator Parameter 'Speed'
        playerAnimator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        //Sets Animator Parameter 'Jumping'
        playerAnimator.SetFloat("VerticalVelocity", rb.velocity.y);

        //Sets Animator Parameter for 'isOnGround'
        playerAnimator.SetBool("isOnGround", isGrounded);

        //Sets Animator Parameter for 'isCrouching'
        playerAnimator.SetBool("isCrouching", isCrouching);

        //Sets Wall Grab Anim
        playerAnimator.SetBool("isWallGrabbing", isGrabbing);

        #endregion
    }

    void FixedUpdate()
    {
        //For changing jump height based upon if player holds the Jump Button or taps it
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier) * Time.deltaTime;
        }
    }

    void enterCrouch()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, 1.6f);
        playerCollider.offset = new Vector2(playerCollider.offset.x, -1.7f);
        moveSpeed /= 2;
        isCrouching = true;
    }

    void exitCrouch()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, 2.4f);
        playerCollider.offset = new Vector2(playerCollider.offset.x, -1.3f);
        moveSpeed *= 2;
        isCrouching = false;
    }

    void enterJump(bool isWallGrabbing)
    {
        if (!isCeiled)
        {
            if (isCrouching)
            {
                exitCrouch();
            }

            playerAnimator.SetTrigger("Jump");

            if (isWallGrabbing)
            {
                rb.velocity = new Vector2(-Input.GetAxisRaw("Horizontal") * moveSpeed, jumpForce);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            FindObjectOfType<AudioManager>().Play("Jump");
        }
    }

    //Change Crouch State
    void changeCrouch()
    {
        if (!isCrouching)
            enterCrouch();
        else if (isCrouching && !isCeiled)
            exitCrouch();
        else
            return;
    }

    //Flips character
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        standingAttackOffset.x *= -1;
        crouchingAttackOffset.x *= -1;
    }

    //For Changing if player can move
    public void canMoveFunction()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
        canMove = !canMove;
    }

    //Player Health Management
    public void isAliveFunction()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        isAlive = false;
        playerCollider.enabled = false;
    }

    //Just for visualizing hitboxes
    void OnDrawGizmosSelected()
    {
        //Draw White Sphere for Standing Attack Range
        Vector3 pos = transform.position;
        pos += transform.right * standingAttackOffset.x;
        pos += transform.up * standingAttackOffset.y;

        Gizmos.DrawWireSphere(pos, .75f);

        //Draw Red Sphere for crouch Attack range
        Gizmos.color = Color.red;
        Vector3 posC = transform.position;
        posC += transform.right * crouchingAttackOffset.x;
        posC += transform.up * crouchingAttackOffset.y;

        Gizmos.DrawWireSphere(posC, .75f);

        // Draw a yellow circle at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(feetPosition.position, checkRadius);

        // Draw a blue circle at the transform position
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wallCheck.position, checkRadius);

        // Draw a green circle at the transform position
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(headPosition.position, checkRadius);
    }
}
