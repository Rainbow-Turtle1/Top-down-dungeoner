using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //private RaycastHit hit;
    //public SpriteRenderer sr;
    //Camera cam;
    //public GameObject foot;
    //public GameObject head;
    //public GameObject player;
    
    private float moveSpeed = 50f;
    [SerializeField] private Rigidbody2D rb;
    private float dashCooldown;
    private float dashSpeed = 1.3f; 
    private float dashCost = 20f;
    [SerializeField] private int dashDuration;
    private float dashDurationSec = 1;
    private bool dashing = false;

    [SerializeField] private Slider staminaBar;
    [SerializeField] private int staminaMax = 500;
    private float currentStamina;
    private float regenTick = 0.2f;
    private Coroutine regen;

    Vector2 movement;

    //private float dustFreq;
    //private float dusttime=2f;
    //private Animator anim;
    //public Animator camAnimator;
    //public Animator HeadAnimator;
    //public Animator LegAnimator;
    //public Animator ScreenEffect;
    //public GameObject trailEffect;
    //public GameObject DashEffect;
    //Vector2 movement;
    //public static PlayerMovement instance;

    private void Start(){
        //player = this.gameObject;
        //life = hearts.Length;
        currentStamina = staminaMax;
        staminaBar.maxValue = staminaMax;
        staminaBar.value = staminaMax;
    }

    void Update(){
        //process inputs here
        movement.x = Input.GetAxisRaw("Horizontal");
        //if (movement.x != 0.0 || movement.y != 0.0){
            //if (dustFreq <=0) {
                //Instantiate(trailEffect, foot.transform.position, Quaternion.identity);
                //dustFreq=dusttime;
            //} else {
                //dustFreq -= Time.deltaTime;
            //}
        //}
        if (movement.x < 0.0){
            transform.localScale = new Vector2(-1f,1f);
            //sr.flipX=true;
        }
        if (movement.x > 0.0){
            transform.localScale = new Vector2(1f,1f);
            //sr.flipX=false;

        }
        movement.y = Input.GetAxisRaw("Vertical");
        
        //HeadAnimator.SetFloat("Speed", movement.sqrMagnitude);
        //LegAnimator.SetFloat("Speed", movement.sqrMagnitude);
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldown<=0f){
            if (currentStamina - dashCost >= 0){
                //currentStamina = currentStamina - dashCost;
                staminaBar.value = currentStamina;
                //head.SetActive(true);
                //Instantiate(DashEffect, foot.transform.position, Quaternion.identity);
                UseStamina(dashCost);
                moveSpeed=(moveSpeed*dashSpeed);
                dashing=true;
                dashDuration=(Mathf.RoundToInt(60*dashDurationSec));  //60fps in the update function meaning the cooldown needs to be 60x the length in seconds
                //camAnimator.SetBool("Dashing", true);
            }
        }
    }

    void FixedUpdate(){ //use this for actual movement as it is more reliable.
        
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime); //move the player
        
        if (dashCooldown>0f){
            dashCooldown-=0.1f; //if the cooldown more than zero decrease it
        }
        if (dashDuration>0){
            dashDuration-=1; 
        }
        if (dashing==true && dashDuration<=0){
            //head.SetActive(false);
            //Instantiate(DashEffect, foot.transform.position, Quaternion.identity);
            //camAnimator.SetBool("Dashing", false);
            dashing=false;
            dashCooldown=3f;
            moveSpeed=moveSpeed/dashSpeed;
        }

    }
    
    public void UseStamina(float amount){
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
