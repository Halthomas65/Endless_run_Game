using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public CapsuleCollider collider;

    // private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed = 5.0f;
    public float maxSpeed = 10.0f;

    public float jumpForce = 7.0f;
    public float fallGravity = 20.0f;

    private Rigidbody rb;
    public LayerMask groundLayer;
    public float raycastDistance = 1.1f;

    private int desiredlane = 1; // 0: Left, 1: Middle, 2: Right
    public float laneDistance = 4.0f;

    private bool isGrounded;
    private bool isSliding = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        // controller = GetComponent<CharacterController>();

        // TODO: Reset the game
        // Time.timeScale = 1;  // Used in PlayerManager.cs

    }

    private void FixedUpdate()
    {
        // Game start check
        if (!PlayerManager.isGameStarted)
        {
            return;
        }

        animator.SetBool("isGameStarted", true);

        // Move the player forward
        transform.position += new Vector3(0, 0, forwardSpeed) * Time.deltaTime;
        // controller.Move(Vector3.forward * forwardSpeed * Time.deltaTime);


        // TODO: Game Over check


        // TODO: Increase speed
        if (forwardSpeed < maxSpeed)
            forwardSpeed += 0.1f * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {

        // Check if the player is grounded
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, groundLayer);
        animator.SetBool("isGrounded", isGrounded);

        // Jump
        if ((Input.GetKeyDown(KeyCode.UpArrow) || SwipeManager.swipeUp) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        // Falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.down * fallGravity, ForceMode.Acceleration);
        }

        // TODO: Slide
        if ((Input.GetKeyDown(KeyCode.DownArrow) || SwipeManager.swipeDown) && !isSliding)
        {
            StartCoroutine(Slide());
        }


        // Get lane change input
        if (Input.GetKeyDown(KeyCode.LeftArrow) || SwipeManager.swipeLeft)
        {
            desiredlane--;
            if (desiredlane == -1)
            {
                desiredlane = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || SwipeManager.swipeRight)
        {
            desiredlane++;
            if (desiredlane == 3)
            {
                desiredlane = 2;
            }
        }


        // Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredlane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredlane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, 80 * Time.deltaTime);
    }


    private void Jump()
    {

    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        collider.height = collider.height / 2;
        collider.center = collider.center / 2;

        yield return new WaitForSeconds(1.2f);

        collider.height = collider.height * 2;
        collider.center = collider.center * 2;
        isSliding = false;
        animator.SetBool("isSliding", false);
    }

    // Clliision
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }
}
