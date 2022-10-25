using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    private void Start()
    {
        originalSpeed = moveSpeed;
    }
    // Update is called once per frame
    private void Update()
    {
        //Processsing Inputs
        ProcessInputs();
    }

    void FixedUpdate()
    {
        //Processing Calculations
        Move();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Sprint();
        }
        else
        {
            originalSpeed = moveSpeed;
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

    void RotateSelf(Vector3 mousePos)
    {
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        transform.right = direction;
    }
}

