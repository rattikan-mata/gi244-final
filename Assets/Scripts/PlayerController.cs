using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections; // เพิ่มเข้ามาสำหรับ Coroutine

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

    
    [Header("UFO Beam Emission Settings")]
    public GameObject ufoBeamCone;      
    [ColorUsage(false, true)] public Color baseBeamColor = Color.cyan;
    
    public float minIntensity = -10f;
    public float maxIntensity = 5f;
    
    public float fadeInDuration = 0.2f;
    public float beamStayDuration = 0.5f;
    public float fadeOutDuration = 0.3f;

    private Material beamMaterial;
    private Coroutine beamCoroutine;

    private Rigidbody rb;
    private InputAction jumpAction;
    private InputAction moveAction;
    private bool isOnGround = true;

    /*private Animator playerAnim;
    private AudioSource playerAudio;*/


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
        Physics.gravity = new Vector3(0, -9.81f * gravityModifier, 0);

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


        
        if (ufoBeamCone != null)
        {
            beamMaterial = ufoBeamCone.GetComponent<Renderer>().material;
            beamMaterial.EnableKeyword("_EMISSION");
            
            SetBeamIntensity(minIntensity);
            ufoBeamCone.SetActive(false);
        }
    }

    void Update()
    {
        if (GameManager.instance == null || !GameManager.instance.isGameActive) return;

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

            if (ufoBeamCone != null)
            {
                if (beamCoroutine != null) StopCoroutine(beamCoroutine);
                beamCoroutine = StartCoroutine(FadeBeamRoutine());
            }
        }
    }

    private IEnumerator FadeBeamRoutine()
    {
        ufoBeamCone.SetActive(true);

        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentIntensity = Mathf.Lerp(minIntensity, maxIntensity, elapsedTime / fadeInDuration);
            SetBeamIntensity(currentIntensity);
            yield return null; 
        }

        SetBeamIntensity(maxIntensity);
        yield return new WaitForSeconds(beamStayDuration);

        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentIntensity = Mathf.Lerp(maxIntensity, minIntensity, elapsedTime / fadeOutDuration);
            SetBeamIntensity(currentIntensity);
            yield return null;
        }

        SetBeamIntensity(minIntensity);
        ufoBeamCone.SetActive(false); 
    }

    private void SetBeamIntensity(float intensity)
    {
        float hdrMultiplier = Mathf.Pow(2f, intensity);
        beamMaterial.SetColor("_EmissionColor", baseBeamColor * hdrMultiplier);
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
        
        /*playerAnim.SetBool("Death_b", true);
        playerAnim.SetInteger("DeathType_int", 1);
        explosionParticle.Play();
        dirtParticle.Stop();
        playerAudio.PlayOneShot(crashSfx);*/
        
    }
}