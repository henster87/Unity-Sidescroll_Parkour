using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject mainCamera;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public Animator playerAnim;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;
    private AudioSource stopMusic;
    private Rigidbody playerRb;
    public GameObject bg;
    private MoveLeft moveLeftBG;
    private MoveLeft moveLeft;

    public float jumpForce;
    public float gravityModifier;
    public bool isGrounded = true;
    public bool isGameOver = false;
    private bool smokePlayed = false;

    public float doubleJumpForce;
    public bool doublejumpused = false;
    public bool dashSpeed = false;

    public float timeModifier = 0.1f;
    public float timeStepModifier = 0.001f;
    private bool isInScene = false;

    // Start is called before the first frame update
    void Start()
    {

        moveLeftBG = bg.GetComponent<MoveLeft>();
        stopMusic = mainCamera.GetComponent<AudioSource>();
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectWithTag("Obstacle") != null && !isInScene)
        {
            isInScene = true;
            moveLeft = GameObject.FindGameObjectWithTag("Obstacle").GetComponent<MoveLeft>();
        }
        else
        {
            isInScene = false;
        }

        if(Input.GetKey(KeyCode.W))
        {
            Time.timeScale = timeModifier;
            Time.fixedDeltaTime = timeStepModifier;
            moveLeftBG.speed = moveLeftBG.slowSpeed;
            if(moveLeft != null)
            {
               moveLeft.speed = moveLeft.slowSpeed; 
            }
        }
        else if(Input.GetKeyUp(KeyCode.W))
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            moveLeftBG.speed = moveLeftBG.normalSpeed;
            if(moveLeft != null)
            {
               moveLeft.speed = moveLeft.normalSpeed; 
            }
        }
        

        if(Input.GetKeyDown(KeyCode.Space ) && isGrounded && !isGameOver)
        {   
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            doublejumpused = false;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && !isGrounded && !doublejumpused && !isGameOver)
        {
            doublejumpused = true;
            playerRb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
            playerAnim.Play("Running_Jump", 3, 0f);
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            dashSpeed = true;
            playerAnim.SetFloat("Speed_Multiplier", 2.0f);
        }
        else if(dashSpeed)
        {
            dashSpeed = false;
            playerAnim.SetFloat("Speed_Multiplier", 1.0f);
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.CompareTag("Ground") && !isGameOver)
        {
            isGrounded = true;
            dirtParticle.Play();
        } 
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over.");
            isGameOver = true;
            if (!smokePlayed)
            {
                playerAudio.PlayOneShot(crashSound, 1.0f);
                explosionParticle.Play();
            }
            dirtParticle.Stop();
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            smokePlayed = true;
            stopMusic.Stop(); 
        }
    }
}
