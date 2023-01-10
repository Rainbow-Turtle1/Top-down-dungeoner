using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private Rigidbody2D rb;
    [SerializeField]private SpriteRenderer sr;
    public GameObject player;
    
    // movement speed of the character
    public float speed = 40.0f;

    // dash speed of the character
    public float dashSpeed = 120.0f;
    private int dashtime;
    private float dashcooldown;

    // amount of stamina required for a dash
    public float dashStamina = 10.0f;

    private float currentStamina;
    [SerializeField]private Slider staminaBar;

    public int staminaMax = 500;
    public float regenTick = 0.2f;
    private Coroutine regen;

    Vector2 movement;
    
    private void Start(){
        player = this.gameObject;
        //life = hearts.Length;
        currentStamina = staminaMax;
        staminaBar.maxValue = staminaMax;
        staminaBar.value = staminaMax;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");// get input axis

        bool canDash = currentStamina >= dashStamina;// check if the character has enough stamina to dash

        bool dashInput = Input.GetButtonDown("Dash"); // check if the player is pressing the dash button
        
        float movementSpeed = speed;// calculate the movement speed for this frame
        
        if (canDash && dashInput)// check if the character should dash
        {
            currentStamina -= dashStamina;// decrease the character's stamina
            Debug.Log("Dash");
            movementSpeed = dashSpeed;// increase the movement speed for this frame

            if (currentStamina - 20 >= 0){
                currentStamina = currentStamina - 20;
                staminaBar.value = currentStamina;
                //Instantiate(DashEffect, foot.transform.position, Quaternion.identity);
                PlayerMove.instance.UseStamina(60);
                speed=(speed*dashSpeed);
                //dashtime=(Mathf.RoundToInt(60*dashLenthSec));
                //camAnimator.SetBool("Dashing", true);
                    if (regen != null){
                        StopCoroutine(regen);
                    }
                regen = StartCoroutine(RegenStamina());
            }
        }

        Vector2 movement = new Vector2(horizontalInput, verticalInput) * movementSpeed;  

        transform.position += (Vector3)movement * Time.deltaTime;// move the character by the calculated movement vector
    }

        void FixedUpdate(){
        //use this for actual movement as it is more reliable.
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        if (dashcooldown>0f){
            dashcooldown-=0.1f;
        }
        if (dashtime>0){
            dashtime-=1;
        }
        
        //head.SetActive(false);
        //Instantiate(DashEffect, foot.transform.position, Quaternion.identity);
        //camAnimator.SetBool("Dashing", false);
        //dash=false;
        //dashcooldown=3f;
        //speed=moveSpeed/dashspeed;
        

    }

    public void UseStamina(int amount){
        if (currentStamina - amount >= 0){
            currentStamina = currentStamina - amount;
            staminaBar.value = currentStamina;
            if (regen != null){
                StopCoroutine(regen);
            }
            regen = StartCoroutine(RegenStamina());
        }
    }

    private IEnumerator RegenStamina(){
        yield return new WaitForSeconds(1);

        while(currentStamina < staminaMax){
            currentStamina += 2;
            staminaBar.value = currentStamina;
            yield return new WaitForSeconds(regenTick);;
        }
        regen = null;
    }
}