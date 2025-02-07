
using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    [Header("Base Setup")]
    public float speed = 12f;
    public float turnSpeed = 10;
    public float gravity = 30f;
    public Vector3 moveDirection = Vector3.forward;
    public Vector2 input;
    private Vector3 targetDirection;
    public bool isDead = false;
    public bool isWin = false ;
    private Quaternion freeDirection;

    [Header("References")]
    public Animator animator;
    public GameObject playerBody, playerRagdoll;
    public GameObject Ragdoll;
    public Cinemachine.CinemachineVirtualCamera cinemachine;
    public ParticleSystem blood;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public audiomanager audioManager;

    [Header("UI")]
    public TMPro.TextMeshProUGUI winText;

    [Header("Jump Settings")]
    public float jumpHeight = 5f;  
    public bool canJump = true;
    public bool isJumping = false;
    public float jumpVelocity;
    public float jumpCooldown = 1.5f;
    private bool isJumpOnCooldown = false;
    private bool isInAir = false;
    private bool hasMovedForwardInAir = false;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        audioManager = GetComponent<audiomanager>();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
            
            if(GameManager.Instance.canPlayerMove){
                Movement();
                HandleJump();
                if(GameManager.Instance.isRLGL) {
                    HandleRLGL();
                }
            }
    }

    void HandleRLGL() {
            if(!isWin){
                if(input.x != 0 || input.y != 0 ) {
                    if(!GameManager.Instance.dollController.lightSignal) {
                        GameManager.Instance.dollController.ShootPlayer(transform); 
                    }
                }
            }
        
    }

    void Movement() {

        if(isDead ) return;


        if (isInAir && !hasMovedForwardInAir) {
                input.x = 0; 
                input.y = 1;  
                hasMovedForwardInAir = true;
            } else {
             
                input.x = Input.GetAxis("Horizontal");
                input.y = Input.GetAxis("Vertical");
                
            }


        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);  
        forward.y = 0;  
        Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);   

        Vector3 move = forward * input.y + right * input.x;
        move = move.normalized * speed;

        Vector3 totalMove = move + new Vector3(0, moveDirection.y, 0);

        characterController.Move(totalMove * Time.deltaTime);
        
        if (!characterController.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
            if(isJumping) {
                input = Vector2.zero;
                moveDirection.y = jumpVelocity;
                isJumping = false;
            }
        } else {
            moveDirection.y = 0;
            jumpVelocity = 0;
            if (isJumping) {
                isJumping = false;
                isInAir = false; 
                hasMovedForwardInAir = false;
            }
        }

        

        UpdateTargetDirection();

        if (input != Vector2.zero && targetDirection.magnitude > 0.1f) {
            Vector3 lookDirection = targetDirection.normalized;

            freeDirection = Quaternion.LookRotation(lookDirection, transform.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, freeDirection, turnSpeed * Time.deltaTime);
        }
    }


    public void UpdateTargetDirection() {
        var forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0; 
        var right = Camera.main.transform.TransformDirection(Vector3.right);

        
        targetDirection = forward * input.y + right * input.x;
        OnAnimatorMove();
    }

    void OnAnimatorMove() {
       
        animator.SetFloat("Move", input.magnitude);  
    }
    void HandleJump() {
        if (canJump && !isJumpOnCooldown && Input.GetButtonDown("Jump") && !isJumping && characterController.isGrounded) {
            audioSource.PlayOneShot(audioManager.jumpSFX);
            isJumping = true;
            animator.SetTrigger("isJumping");
            jumpVelocity = Mathf.Sqrt(2 * jumpHeight * gravity); 
            moveDirection = targetDirection.normalized * speed;
            moveDirection.y = jumpVelocity; 
            isInAir = true; 
            // Start the cooldown
            StartCoroutine(JumpCooldown());
        }
    }

    IEnumerator JumpCooldown() {
        isJumpOnCooldown = true;  
        yield return new WaitForSeconds(jumpCooldown);
        isJumpOnCooldown = false; 
    }


    public void Dead() {
        audioSource.PlayOneShot(audioManager.deadSFX);
        playerBody.SetActive(false);
        playerRagdoll.SetActive(true);
        isDead = true;
        cinemachine.Follow = Ragdoll.transform;
        if(GameManager.Instance.isRLGL){
        blood.Play();
        }
        GameManager.Instance.GameOver();
        GameManager.Instance.uiController.infoText.text = "You Lose";
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Finish>()){
            isWin = true;
            winText.gameObject.SetActive(true);
            GameManager.Instance.GameOver();
            GameManager.Instance.uiController.infoText.text = "You Win";
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        Glass glass = hit.gameObject.GetComponent<Glass>();
        if (glass != null && glass.isBroken)
        {
            StartCoroutine(glass.BreakGlassWithDelay(0.1f));
            StartCoroutine(HandleFalling());
        }
    }

    IEnumerator HandleFalling(){
        
        yield return new WaitForSeconds(1.5f);
        gravity = 100;
        isInAir = true;
        hasMovedForwardInAir = true;
        
    }
}


