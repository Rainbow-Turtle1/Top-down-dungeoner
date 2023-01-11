using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // movement speed of the character
    public float speed = 5.0f;

    // dash speed of the character
    public float dashSpeed = 10.0f;

    // amount of stamina required for a dash
    public float dashStamina = 10.0f;

    // current stamina of the character
    private float currentStamina;

    // Update is called once per frame
    void Update()
    {
        // get input from the horizontal axis (left and right arrow keys by default)
        float horizontalInput = Input.GetAxis("Horizontal");

        // get input from the vertical axis (up and down arrow keys by default)
        float verticalInput = Input.GetAxis("Vertical");

        // check if the character has enough stamina to dash
        bool canDash = currentStamina >= dashStamina;

        // check if the player is pressing the dash button
        bool dashInput = Input.GetButtonDown("Dash");

        // calculate the movement speed for this frame
        float movementSpeed = speed;

        // check if the character should dash
        if (canDash && dashInput)
        {
            // decrease the character's stamina
            currentStamina -= dashStamina;
            Debug.Log("Dash");

            // increase the movement speed for this frame
            movementSpeed = dashSpeed;
        }

        // calculate the character's movement vector
        Vector2 movement = new Vector2(horizontalInput, verticalInput) * movementSpeed;

        // move the character by the calculated movement vector
        transform.position += (Vector3)movement * Time.deltaTime;
    }
}