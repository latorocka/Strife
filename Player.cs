using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Debug = UnityEngine.Debug;


public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] public float dashSpeed;

    public bool _resetJump = false;
    bool isAlive = true;



    Rigidbody2D myRigidBody;
    Animator myAnimator;

   
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAlive) { return; }

        Run();
        FlipSprite();
        Jump();
        Dash();
        Die();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private void Jump()
    {
        
        if (!myRigidBody.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        
        
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            StartCoroutine(ResetJumpRoutine());
            myRigidBody.velocity += jumpVelocityToAdd;
            myAnimator.SetBool("Jumping", true);
        }
        
    }

    private void Dash()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
            Vector2 playerVelocity = new Vector2(controlThrow * dashSpeed, myRigidBody.velocity.y);
            myRigidBody.velocity = playerVelocity;
            myAnimator.SetBool("IsDashing", true);
        }
        
    }

    IEnumerator ResetJumpRoutine()
    {
        _resetJump = true;
        yield return new WaitForSeconds(0.1f);
        _resetJump = false;
    }

    private void Die()
    {
        if (myRigidBody.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
        }
    }

}
