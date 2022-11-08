using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private FieldOfView fieldOfView;
    //Player Walking Stuff
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDir;

    //Player Sprinting Stuff
    private float originalSpeed;
    public float sprintSpeed;

    //Tracking The Sprint
    [SerializeField] Slider slider;
    public float stamina;
    public float maxStamina;
    public float staminaDrain;
    public float recovery;

    //Sprint cooldown
    public float cd;
    public float originalcd;
    public bool sprinting;
    public bool canSprint;

    [Header("Audio")]
    public AudioSource audioSource;

    public AudioSource walkSource;
    public AudioSource sprintSource;

    //public AudioClip walkClip;
    //public AudioClip sprintClip;
    public AudioClip breathClip;

    public bool isMoving = false;
    public bool isSprinting = false;

    private void Start()
    {
        originalSpeed = moveSpeed;
        sprinting = false;
        canSprint = true;

        cd = originalcd;

        stamina = maxStamina;
    }
    // Update is called once per frame
    private void Update()
    {
        //Processsing Inputs
        ProcessInputs();

        slider.value = stamina / 100;

        if (sprinting == true && canSprint) //Currently Sprinting
        {
            stamina -= staminaDrain * Time.deltaTime; //Stamina drains
        }
        

        if (stamina <= 0) //Empty Stamina
        {
            sprinting = false;
            canSprint = false;
            audioSource.PlayOneShot(breathClip, .5f); //When you are out of stamina breathe heavily
        }

        if (!canSprint)
        {
            cd -= 1 * Time.deltaTime;
            
        }
        
        if(cd <= 0)
        {
            canSprint = true;
            cd = originalcd;
        }

        if (sprinting == false) //If you can't sprint
        {
            stamina += recovery * Time.deltaTime; //Recover stamina
        }



        if (stamina >= maxStamina) //If you're at max Stamina you can't go over that amount
        {
            stamina = maxStamina;
            
        }

        //Movement Sound Code
        if (!sprinting && rb.velocity.x != 0 || rb.velocity.y != 0 && !sprinting) //If you aren't sprinting but you are moving
        {
            isMoving = true;
        } else
        {
            isMoving = false;
        }
        if (isMoving)
        {
            if (!walkSource.isPlaying)
            {
                walkSource.Play();
            }
        } else
        {
            walkSource.Stop();
        }

        //Sprinting and sound
        if (sprinting && rb.velocity.x != 0 || rb.velocity.y != 0 && sprinting) //If you aren't sprinting but you are moving
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
        if (isSprinting)
        {
            if (!sprintSource.isPlaying)
            {
                sprintSource.Play();
            }
        }
        else
        {
            sprintSource.Stop();
        }
    }

    void FixedUpdate()
    {
        //Processing Calculations
        Move();

       
        if (Input.GetKey(KeyCode.LeftShift) && canSprint) //If you are sprinting
        {
            Sprint();
            sprinting = true;
            
        }
        else //If you aren't sprinting
        {
            originalSpeed = moveSpeed; 
            sprinting = false;
            
        }

    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDir.x * originalSpeed, moveDir.y * originalSpeed);
    }

    void Sprint()
    {
        originalSpeed = sprintSpeed;
    }

}

