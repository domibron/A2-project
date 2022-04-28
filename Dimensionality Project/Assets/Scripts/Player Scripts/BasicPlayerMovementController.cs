using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using TMPro;

public class BasicPlayerMovementController : MonoBehaviour
{
    float playerHeight = 2f;
    public Transform orientation;

    [Header("Refs")]
    WallRunController wallRun;

    PlayerCameraController camCon;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float airMultiplier = 0.4f;
    float movementMultiplier = 10f;

    [Header("Sprinting")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 15f;
    public float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space; //can set as nothing and use a keybind manager
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag and gravity")]
    public float groundDrag = 6f;
    public float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    bool isGrounded;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;
    Vector3 jumpMoveDirection;

    bool isCrouching = false;
    bool isSliding = false;

    Rigidbody rb;

    RaycastHit slopeHit;

    [Header("Temp Vars and that")]
    public Transform head;
    public Transform cameraforbob;

    public float bobFreq = 5f;
    public float bobHorrAmplitude = 0.1f;
    public float bobVertAmplitude = 0.1f;
    [Range(0, 1)] public float headbobSmoothing = 0.1f;

    public bool isMoving;
    private float walkingTime;
    private Vector3 targetCameraPosition;

    //temp holder
    float c;


    //when called it will send a raycast out and return is true if the vector does not return stright up
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public void Start()
    {
        //rigid body settings are set as well as varibles and the script get the rigidbody
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.drag = 0f;
        wallRun = GetComponent<WallRunController>();
        camCon = GetComponent<PlayerCameraController>();
    }


    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //is grounded check


        //runs myinput funtion and controll drag (more over this farther down)
        MyInput();
        ControllDrag();
        ControlSpeed();

        //allows the player to jump if they are on the ground and presses the jump key
        if (Input.GetKeyDown(jumpKey) && isGrounded && !isSliding)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.C) || Physics.Raycast(transform.position, Vector3.up, (playerHeight / 2) + 0.2f))
        {
            Crouch();
        }
        else
        {
            Stand();
        }

        //sets the direction following the slope
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        //moive direction so the player can control inair movement
        jumpMoveDirection = moveDirection * 0.1f;

        //gets the player's height by getting the scale of the Rigidbody and timesing by 2 as defult is 1
        playerHeight = rb.transform.localScale.y * 2;
        groundDistance = rb.transform.localScale.y * 2 / 5;

        MainHeadBobing();

        print(wallRun.isWallRunning);
    }

    //when called it will grab movement input
    void MyInput()
    {
        //grabs key inputs
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        //combines both inputs into one direction
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void Jump() //when called then the player will jump in the air
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); //add a jump force to the rigid body component.
    }

    void Crouch()
    {
        isCrouching = true;
        rb.transform.localScale = new Vector3(1f, 0.5f, 1f); // sets to crouch size
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundDistance);
    }

    void Stand()
    {
        isCrouching = false;
        rb.transform.localScale = Vector3.one; // sets size back to normal
    }

    private void MainHeadBobing()
    {
        if (isMoving && isGrounded || wallRun.isWallRunning) walkingTime += Time.deltaTime;
        else walkingTime = 0f;

        bobFreq = (moveSpeed > 4.5f) ? 1f * moveSpeed : 4.5f;

        targetCameraPosition = head.position + CalculateHeadBobOffset(walkingTime);

        cameraforbob.position = Vector3.Lerp(cameraforbob.transform.position, targetCameraPosition, headbobSmoothing);

        if ((cameraforbob.position - targetCameraPosition).magnitude <= 0.001) cameraforbob.position = targetCameraPosition;
    }

    private Vector3 CalculateHeadBobOffset(float t)
    {
        float horOffset = 0f;
        float vertOffset = 0f;
        Vector3 Offset = Vector3.zero;

        if (t > 0)
        {
            horOffset = Mathf.Cos(t * bobFreq) * bobHorrAmplitude;
            vertOffset = Mathf.Sin(t * bobFreq * 2f) * bobVertAmplitude;

            Offset = orientation.right * horOffset + orientation.up * vertOffset;
        }

        return Offset;
    }

    void ControlSpeed()
    {
        if (!wallRun.isWallRunning || isGrounded) //DO NOT TOUCH IF STATEMENT UNLESS YOU WANT TO REWORK BOTH THIS AND WALL RINNING SCRIPTS
        {
            float ySpeed = rb.velocity.y;

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (rb.velocity.magnitude > sprintSpeed)
            {
                rb.velocity = rb.velocity.normalized * sprintSpeed;
            }

            rb.velocity = new Vector3(rb.velocity.x, ySpeed, rb.velocity.z);


            //if (isGrounded && Input.GetAxis("Vertical") > 0)
            //{
            //    if (Mathf.Abs(Input.GetAxis("Mouse X")) > 7)
            //    {
            //        moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, Mathf.Abs(Input.GetAxis("Mouse X")) * Time.deltaTime);
            //        print(Mathf.Abs(Input.GetAxis("Mouse X")));
            //    }
            //    else
            //    {
            //        moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, Input.GetAxisRaw("Mouse X") * 2f * Time.deltaTime);
            //    }

            //    isMoving = true;
            //}

            if (isGrounded && Input.GetAxis("Vertical") > 0)
            {
                moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);

                isMoving = true;
            }

            if (isGrounded && Mathf.Abs(Input.GetAxis("Vertical")) > 0 && Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
            {
                moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);

                isMoving = true;
            }

            else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                moveSpeed = walkSpeed / 2;

                isMoving = false;
            }
        }
        else if (wallRun.isWallRunning)
        {
            moveSpeed = walkSpeed * airDrag;
        }

        else if (isCrouching)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed / 2f, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    //when called then drag will be controlled
    void ControllDrag()
    {
        //drag is controlled if in air or ground so the player is not slow falling
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        //calls the Move player funtion
        MovePlayer();
    }

    //when called then the rigid body will get force applyed
    void MovePlayer()
    {
        //if on the ground but not on a slope
        if (isGrounded && !OnSlope())
        {
            //walk speed on flat ground
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }

        //if the player is on the ground and on a slope
        else if (isGrounded && OnSlope())
        {
            //walk speed on slope
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }

        //if the player is not on the ground
        else if (!isGrounded)
        {
            //jumping in mid air force with a downwards force
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
            rb.useGravity = true;
        }
    }
}
