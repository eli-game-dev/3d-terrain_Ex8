using System.Collections;
using UnityEngine;


/**
 * This component moves a player controlled with a CharacterController using the keyboard.
 */
[RequireComponent(typeof(CharacterController))]
public class CharacterKeyboardMover : MonoBehaviour
{
    [Tooltip("Speed of player keyboard-movement, in meters/second")] [SerializeField]
    float speed = 3.5f;

    [SerializeField] float gravity = 9.81f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float jumpHeight = 4f;
    private float currentSpeed;
    private bool isCrawl;
    private Animator animator;
    private CharacterController cc;

    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        isCrawl = false;
    }

    Vector3 velocity;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = speed;
        }

        if (Input.GetButtonDown("Jump") && cc.isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * 3.0f * gravity);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && cc.isGrounded)
        {
            if (isCrawl == false)
            {
                animator.SetBool("isCrawl", true);
                this.transform.GetChild(0).transform.Rotate(45,0,0);
                isCrawl = true;
            }
            else
            {
                this.transform.GetChild(0).transform.Rotate(-45,0,0);
                animator.SetBool("isCrawl", false);
                isCrawl = false;
            }
        }


        if (cc.isGrounded)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            velocity.x = x * currentSpeed;
            velocity.z = z * currentSpeed;
            animator.speed = currentSpeed;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            animator.SetBool("isWalking", true);
        }


        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            animator.SetBool("isWalking", false);
        }

        //Move in the direction you look:
        velocity = transform.TransformDirection(velocity);
        cc.Move(velocity * Time.deltaTime);
    }
}