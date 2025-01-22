using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    [Header("Ground check")]
    public float playerHeight;
    public float groundDrag;
    public LayerMask whatIsGround;
    bool grounded;
   
    
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 movementDirection;
    Rigidbody rb;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }
    private void Update()
    {
        //check if the player is grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        InputSystem();
        if (grounded)
        {
            rb.drag = groundDrag;
            Debug.Log("player is touching the ground");
        }
        else
        {
            rb.drag = 0;
            Debug.Log("player is not touching the ground");
        }
        SpeedControl();
    }
    private void FixedUpdate()
    {
        movePlayer();
    }
    private void InputSystem()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump&& grounded) 
        {
            readyToJump = true;

            JumpSystem();
            Invoke(nameof(resetJump), jumpCooldown);
        }
    }

    private void movePlayer()
    {
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(movementDirection.normalized * movementSpeed * 10f, ForceMode.Force);
        }
       
        else if (!grounded)
        {
            rb.AddForce(movementDirection.normalized * movementSpeed * 5f * airMultiplier, ForceMode.Force);
        }
            
        
    }

    private void SpeedControl()
    {
        Vector3 flatvelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit the velocity

        if (flatvelocity.magnitude>movementSpeed)
        {
            Vector3 limitvelocity = flatvelocity.normalized * movementSpeed;
            rb.velocity = new Vector3(limitvelocity.x, rb.velocity.y, limitvelocity.z);
        }
    }

    private void JumpSystem()
    {
        //reset y value
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void resetJump()
    {
        readyToJump = true;
    }

}
