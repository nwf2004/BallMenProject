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
    }

    void FixedUpdate()
    {
        //Processing Calculations
        Move();
        if (Input.GetKey(KeyCode.LeftShift) && canSprint)
        {
            Sprint();
            sprinting = true;
        }
        else
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

