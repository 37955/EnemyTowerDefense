using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float sprintSpeed = 5f;
    public float jumpForce = 5f;
    public Animator animator; // Reference to Animator
    public CharacterController controller;

    private Vector3 velocity;
    private float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    void Update()
    {
        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Stick to the ground
            animator.ResetTrigger("Jump"); // Reset Jump trigger after landing
        }

        // Movement Input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Check if running or walking
        if (Input.GetKey(KeyCode.LeftShift) && moveZ > 0) // Run when Shift + W is pressed
        {
            controller.Move(move * sprintSpeed * Time.deltaTime);
            animator.SetTrigger("Run");  // Trigger the Run animation
            animator.ResetTrigger("Walk"); // Reset Walk if the player is running
        }
        else if (move.magnitude > 0) // Walk when any movement key is pressed
        {
            controller.Move(move * walkSpeed * Time.deltaTime);
            animator.SetTrigger("Walk");  // Trigger the Walk animation
            animator.ResetTrigger("Run");  // Reset Run if the player is walking
        }
        else // If no movement keys are pressed, return to Idle
        {
            animator.ResetTrigger("Walk");
            animator.ResetTrigger("Run");
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);  // Apply jump force
            animator.SetTrigger("Jump");  // Trigger the Jump animation
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
