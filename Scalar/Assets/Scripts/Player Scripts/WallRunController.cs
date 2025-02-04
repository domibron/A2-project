using UnityEngine;

public class WallRunController : MonoBehaviour
{
    public bool isWallRunning { get; private set; } = false;
    public float tilt { get; private set; }
    private bool isJumping = false;
    private bool detectedInput = false;
    private bool isPlayerFalling = false;
    public PlayerController playerController;
    public Transform orientation;
    public Camera cam;
    public Rigidbody rb;

    [Header("Wall Running Checks")]
    public float wallDistance;
    public LayerMask wallMask;
    public float minimumJumpHeight;
    public float minimumSpeed;
    public float maxSpeed;

    public bool isNoClipEnabled = false;

    [Header("Wall Running Forces")]
    public float wallRunGravity;
    public float beforeWallRunGravity;
    public float wallRunJumpForce;
    public float wallRunSpeed;
    public float wallRunDesiredHeight;
    public float timeUntilGravityIsApplied;
    public float wallRunCounterGravity;

    [Header("Camera settings")]
    public float FOV;
    public float wallRunFOV;
    public float wallRunFOVTime;
    public float camTilt;
    public float camTiltTime;

    private float time = 0f;
    private bool wallLeft = false;
    private bool wallRight = false;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;

    void Update() // runs all wallrun functions every frame
    {
        if (isNoClipEnabled) rb.useGravity = false; // stops the gravity and wall running.
        if (isNoClipEnabled) return;

        minimumSpeed = 0 * playerController.PlayerHeight / 2;
        WallRun(); // called the wall run manager
    }

    void WallRun() // this manages the wall running
    {
        CheckWall(); // calles the check wall to see what side the wall is on
        if (CanWallRun()) // check if the player can wall run by runnung the check
        {
            if (wallLeft || wallRight) // checks if the player has a wall to their side
            {
                if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))) // fuck that && !detectedInput && !isPlayerFalling
                {
                    detectedInput = true;
                    StartWallRun(); // starts the wall run
                }
                //else if (detectedInput && !isPlayerFalling) // if the player has no input, stop the wall run
                //{
                //    StartWallRun();
                //}
                //else if (!detectedInput && isPlayerFalling)
                //{
                //    return;
                //}
                else
                {
                    StopWallRun(); // stops the wall run
                }
                //StartWallRun(); // if the player has a wall to the side they will begin to wall run
            }
            else // if not then they will not or stop wall running
            {
                StopWallRun(); // calles the stop wall running
            }
        }
        else // this stops the player from wall running when if they're touching the wall still
        {
            StopWallRun(); // calles the stop wall run function
        }
    }

    bool CanWallRun() // runs a ground check so low valuse means the player can wall run
    {
        // got rid of this rb.velocity.magnitude > minimumSpeed && because it was causing the player to not wall run when they were falling fast enough.
        if (Input.GetAxis("Vertical") > 0 && Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight * playerController.PlayerHeight / 2) == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void CheckWall() // checks what side the wall is on
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, rb.transform.localScale.x * wallDistance, wallMask); // sends out a raycast to the left and sets left to ture if hit
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, rb.transform.localScale.x * wallDistance, wallMask); // sends out a raycast to the right and sets right to true if hit
    }

    void StartWallRun() // the wall running script
    {
        //print(rb.velocity.y);

        isWallRunning = true;

        if (Input.GetKeyDown(KeyCode.Space)) // checks if the player presses space
        {
            Vector3 wallRunJumpDirection;
            if (wallLeft) // if the wall is to the left and space is pressed
            {
                wallRunJumpDirection = transform.up + leftWallHit.normal; // this will return the direction of the face of the wall
            }
            else // if the wall is to the right and space is pressed
            {
                wallRunJumpDirection = transform.up + rightWallHit.normal; // this will return the direction of the face of the wall
            }
            rb.velocity = new Vector3(rb.velocity.x * playerController.PlayerHeight / 2, wallRunDesiredHeight * playerController.PlayerHeight / 2, rb.velocity.z); // will set y > 0 or it will make the player fall
            rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100 * playerController.PlayerHeight / 2, ForceMode.Force); // this applies the jump force (left and right)
            isJumping = true;
        }

        if (isJumping) return;

        float ySpeed = rb.velocity.y; // this saves the y velocity.

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // the y velocity.

        // clamps to max speed.
        if (rb.velocity.magnitude > maxSpeed * playerController.PlayerHeight / 2)
        {
            rb.velocity = rb.velocity.normalized * wallRunSpeed * playerController.PlayerHeight / 2;
        }

        rb.velocity = new Vector3(rb.velocity.x, ySpeed, rb.velocity.z); // re-adds the y velocity.

        //rb.AddForce(orientation.forward * wallRunSpeed);
        //rb.useGravity = false; // this stop normal gravity of pulling the player down at 9.81f allowing for custom gravity.
        //time += Time.deltaTime; // base time duration of wall running.
        //rb.AddForce(Vector3.down * (wallRunGravity * time) * playerController.PlayerHeight / 2, ForceMode.Acceleration); // this applies the custom gravity to the player

        time += Time.deltaTime; // base time duration of wall running. You need this for this to work!!!!

        float time2 = time - timeUntilGravityIsApplied + 1; // time for gravity removing access time.

        if (time <= timeUntilGravityIsApplied + 1)
        {
            if (rb.velocity.y <= 0) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * wallRunCounterGravity, rb.velocity.z);

            else if (rb.velocity.y > 0) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);

            rb.AddForce(Vector3.down * beforeWallRunGravity * playerController.PlayerHeight / 2, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(Vector3.down * (wallRunGravity * (time2 < 1 ? 1 : time2)) * playerController.PlayerHeight / 2, ForceMode.Acceleration); // this applies the custom gravity to the player
            isPlayerFalling = true;
        }
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFOV, wallRunFOVTime * Time.deltaTime); // this will lerp from the defult fov to the wall run fov over desired time.

        if (wallLeft) // checks what side the wall is.
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime); // this tilts the camera to the desired tilt opposite to the wall.
        else if (wallRight) // checks what side the wall is
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime); // this tilts the camera to the desired tilt opposite to the wall

        //rb.AddForce(orientation.forward * wallRunSpeed, ForceMode.Acceleration); // this makes the player move forward because if you stop you fall

    }

    void StopWallRun() // the stop wall run function
    {
        time = 1f;

        isWallRunning = false;
        isJumping = false;
        detectedInput = false;
        if (CanWallRun()) isPlayerFalling = true;
        else isPlayerFalling = false;


        // rb.useGravity = true; // turns back on the normal gravity

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, FOV, wallRunFOVTime * Time.deltaTime); // sets fov back to normal
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime); // set the tilt back to normal
    }
}