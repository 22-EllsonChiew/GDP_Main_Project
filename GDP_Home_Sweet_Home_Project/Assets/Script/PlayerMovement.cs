using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float speed;
    public float sprintSpeed;
    public float walkSpeed;

    [Header("Ground")]
    public float playerHeight;
    public LayerMask isGround;
    bool grounded;
    
    public float groundDrag;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    public KeyCode sprintFunction = KeyCode.LeftShift;

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
       

        
        //to check if the player is grounded, it shot a raycast using Vector3.down downwards from player height 0.5f is half of the player height and + 0.2f is the ground so the raycast shots furter down and check if the ground is the layermask they are looking for and this case is isGround.
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, isGround);
        Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f), grounded ? Color.green : Color.red);

        //Drag
        //if the player is grounded, it will add drag value so the player wont slip and hit the wall or anything other thing
        if (grounded)

            rb.drag = groundDrag; //rigibody drag become the same value as the new variable which is groundDrag

        else
            rb.drag = 0f; //rigibody drag become the same value as the new variable which is groundDrag



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
        //make a new Vector3 and name it as faltvel as faltvelocity, with rigibody velocity of.x and .y, it grab play flat velocity 
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //if flatvel.magnitude is greater then speed limit the veloctiy, with new vector3 to limitedvel after the flatvel normalized value multiple with speed to get the max speed
        if (flatVel.magnitude > speed)
        {
            //calculate the max speed then apply the max speed
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); //rigibody velocity = to the new speed limit of .x and .z while .y is the same as its not for jumping
        }
    }

    


}
