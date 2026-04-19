using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float gravityModifier;
    /*public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSfx;
    public AudioClip crashSfx;*/

    [Header("Lane Settings")]
    public float laneWidth = 3f;
    public float laneSwitchSpeed = 10f;

    [Header("Jump Cooldown")]
    public float jumpCooldown = 3f;

    private Rigidbody rb;
    private InputAction jumpAction;
    private InputAction moveAction;
    private bool isOnGround = true;

    /*private Animator playerAnim;
    private AudioSource playerAudio;*/

    public bool gameOver = false;

    private int currentLane = 1;
    private float targetX;

    private float lastJumpTime = -99f;
    public float JumpCooldownRemaining => Mathf.Max(0f, jumpCooldown - (Time.time - lastJumpTime));
    public bool CanJump => JumpCooldownRemaining <= 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        /*playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();*/
    }

    void Start()
    {
        Physics.gravity *= gravityModifier;

        jumpAction = InputSystem.actions.FindAction("Jump");
        moveAction = InputSystem.actions.FindAction("Move");

        jumpAction.Enable();
        moveAction.Enable();

        currentLane = 1;
        targetX = GetLaneX(currentLane);

        Vector3 startPos = transform.position;
        startPos.x = targetX;
        transform.position = startPos;

        lastJumpTime = -jumpCooldown;

        gameOver = false;
    }

    void Update()
    {
        if (gameOver) return;

        HandleLaneSwitching();
        HandleJump();
        SmoothMoveTolane();
    }

    private void HandleLaneSwitching()
    {
        bool pressedLeft = Keyboard.current != null && (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame);
        bool pressedRight = Keyboard.current != null && (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame);

        if (pressedLeft && currentLane > 0)
        {
            currentLane--;
            targetX = GetLaneX(currentLane);
        }
        else if (pressedRight && currentLane < 2)
        {
            currentLane++;
            targetX = GetLaneX(currentLane);
        }
    }

    private void HandleJump()
    {
        if (jumpAction.triggered && isOnGround && CanJump)
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            isOnGround = false;
            lastJumpTime = Time.time;

            /*playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSfx);*/
        }
    }

    private void SmoothMoveTolane()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, targetX, Time.deltaTime * laneSwitchSpeed);
        transform.position = pos;
    }

    private float GetLaneX(int lane)
    {
        return (lane - 1) * laneWidth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            //dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            /*playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSfx);*/
        }
    }
}
