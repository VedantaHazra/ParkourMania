using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] public float rotationSpeed = 500f;
    [SerializeField] private MainCameraController MCC;
    Quaternion requireRotation;
    private float movementSpeed;
    private float movementAmount;
    bool playerControl = true;
    [SerializeField] float jumpSpeed;
    Vector3 movementDirection;
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    public Transform playerCamera;
    public float gravity = -2f;
    public float iceSlideSpeed = 2f;
    //public Rigidbody rb;
    public float jumpForce;


    [Header("Player Animator")]
    [SerializeField] private Animator animator;

    [Header("Player Collison and Gravity")]
    [SerializeField] private CharacterController CC;
    [SerializeField] private Vector3 surfaceCheckOffset;
    [SerializeField] private LayerMask surfaceLayer;
    [SerializeField] private LayerMask bounceLayer;
    [SerializeField] private LayerMask iceLayer;
    [SerializeField] private LayerMask envLayer;
    [SerializeField] private float surfaceCheckRadius = 0.1f;
    public bool onSurface;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private Vector3 moveDir;
    bool bounce = false;
    bool ice = false;
    bool env = false;
    bool isControl;

    [Header("Score")]
    public ScoreUI scoreUI;
    public AudioSource audioSource;
    public AudioClip coinSound;
    public SceneManagerScript sceneManager;

    void Update()
    {
         
         CheckJump();
         PlayerMovement();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Exit...");
            Application.Quit();
        }

        
       
       // PlayerAnimation();
       if(!ice)
       {
        SurfaceCheck();

       }

       if(env)
       {
        sceneManager.EndMenu();
       }
        
        CheckGravity();
        

        BlocksCode();
    }

    void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg + playerCamera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
        transform.rotation = Quaternion.Euler(0f,angle,0f);



        
        movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal)+Mathf.Abs(vertical));

        var movementInput = (new Vector3(horizontal,0,vertical)).normalized;

        if(movementInput!=Vector3.zero && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            Walk();
        }
        else if(movementInput!= Vector3.zero && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Run();
        }
        else
        {
            Idle();
        }
        if(!isControl)
        {
            Idle();
        }

        movementDirection = MCC.flatRotation*movementInput;
        if(bounce)
        {

           // movementDirection.y += jumpSpeed*Time.deltaTime;
            //CC.Move(movementDirection*Time.deltaTime);

            //rb.velocity = new Vector3(0,jumpForce*Time.deltaTime,0);
        }
        if(ice)
        {
            //animator.SetBool("Sliding", true);
            isControl=false;
            movementDirection = transform.forward * iceSlideSpeed;
            onSurface = true;
            CheckJump();
        }
        else{
            isControl = true;
            //animator.SetBool("Sliding", false);
        }
        CC.Move(movementDirection*Time.deltaTime);

        if(movementSpeed!=0)
        {
        requireRotation = Quaternion.LookRotation(movementDirection);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, requireRotation, rotationSpeed*Time.deltaTime);

        
    }

    private void Idle()
    {
        movementSpeed = 0f;
        animator.SetBool("Running", false);
    }
    private void Walk()
    {
        movementSpeed = walkSpeed;
        animator.SetBool("Running", true);
    }
    private void Run()
    {
        movementSpeed = runSpeed;
        animator.SetBool("Running", true);
    }


    void SurfaceCheck()
    {
        onSurface = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius, surfaceLayer);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius);
    }

    private void CheckGravity()
    {
        if(bounce)
        {
            gravity = 300f;
        }
        else{ gravity = -1f;}
        if(onSurface)
        {
            gravity = -1f;
            fallingSpeed = -0.5f;
        }
        else{
            fallingSpeed += gravity * Time.deltaTime;
        }
        
        movementDirection.y = fallingSpeed;
        CC.Move(movementDirection*Time.deltaTime);

    }

    public void SetControl(bool hasControl)
    {
        this.playerControl = hasControl;
        CC.enabled = hasControl;

        if(!hasControl)
        {
            animator.SetBool("Running",false);
            //animator.SetFloat("movementValue", 0f);
            requireRotation = transform.rotation;
        }
    }

    public void CheckJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && onSurface)
        {
            animator.SetTrigger("Jump");
        }
        else{
            animator.ResetTrigger("Jump");
        }
            
    }


    void BlocksCode()
    {
        bounce = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius, bounceLayer);
        ice = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius, iceLayer);
        env = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius, envLayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Coin")
        {
            Destroy(other.gameObject);
            audioSource.PlayOneShot(coinSound);
            scoreUI.UpdateScore();
        }
    }
    
}
