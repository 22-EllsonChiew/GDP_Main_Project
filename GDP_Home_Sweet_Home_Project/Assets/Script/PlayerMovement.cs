using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float speed;

    [Header("Ground")]
    public float playerHeight;
    public LayerMask isGround;
    bool grounded;
    public float groundDrag;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;


    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        SpeedControl();

        //shot ray cast down from player height down 0.5f is half of the player height and + 0.2f is the ground so the raycast shots furter down and
        //check if the ground is the layermask they are looking for and this case is isGround.
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, isGround);

        //Drag
        //if player ground, add drag value
        if (grounded)

            rb.drag = groundDrag; // rigibody drag becomes the variable of groundDrag
        else
            rb.drag = 0;// else player not grounded at 0 value to rigibody drag
        
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

    

        rb.AddForce(moveDirection.normalized * speed * 10, ForceMode.Force);
   
    }

    private void SpeedControl()
    {
        //make a new Vector3 and name it as faltvelocity, with rigibody velocity of.x and .y, it grab play flat velocity
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //if flatVelocity.magnitude is greater then speed limit the veloctiy, with new vector3 to limitVelocity after the flatvelocity normalized value multiple with speed to get the max speed
        if (flatVelocity.magnitude > speed)
        {
            //calculate the max speed then apply the max speed
            Vector3 limitVelocity = flatVelocity.normalized * speed;
            //rigibody velocity = to the new speed limit of .x and .z while .y is the same as its not for jumping
            rb.velocity = new Vector3(limitVelocity.x, rb.velocity.y, limitVelocity.z);
        }
    }
}
