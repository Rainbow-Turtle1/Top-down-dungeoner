﻿using System.Collections;
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
    
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float dashCooldown = 1f; // a changeable constant for the legth of time before the palyer can dash again.
    private float dashCooldownTimer; // the variable that is decreased every frame to prevern the player fro dashing before the cooldown is up.
    [SerializeField] private float dashSpeed = 5f; 
    [SerializeField] private float dashCost = 20f;
    [SerializeField] private int dashDuration;
    [SerializeField] private float dashDurationSec = 0.4f;
    private bool dashing = false;

    [SerializeField] private Slider staminaBar;
    [SerializeField] private int staminaMax = 500;
    [SerializeField] private int staminaRegenBuffer = 1;
    private float currentStamina;
    [SerializeField] private float regenTick = 0.2f;
    private Coroutine regen;

    Vector2 movement;

    [SerializeField]private Animator camAnimator;
    [SerializeField]private Animator HeadAnimator;
    [SerializeField]private Animator LegAnimator;
    [SerializeField]private GameObject Trails;
    //private float dustFreq;
    //private float dusttime=2f;
    //private Animator anim;
    //public Animator ScreenEffect;
    //public GameObject trailEffect;
    //public GameObject DashEffect;
    //Vector2 movement;
    //public static PlayerMovement instance;

    private void Start(){
        //player = this.gameObject;
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
            transform.localScale = new Vector2(-1f,1f); // flip player when moving in other direction
        }
        if (movement.x > 0.0){
            transform.localScale = new Vector2(1f,1f); // flip player back
        }
        movement.y = Input.GetAxisRaw("Vertical");
        
        HeadAnimator.SetFloat("Speed", movement.sqrMagnitude);
        LegAnimator.SetFloat("Speed", movement.sqrMagnitude);
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer<=0f){
            if (currentStamina - dashCost >= 0){
                dashCooldownTimer = dashCooldown;
                UseStamina(dashCost);
                staminaBar.value = currentStamina;
                moveSpeed=(moveSpeed*dashSpeed);
                dashing=true;
                dashDuration=(Mathf.RoundToInt(60*dashDurationSec));  //60fps in the update function meaning the cooldown needs to be 60x the length in seconds
                //Instantiate(DashEffect, foot.transform.position, Quaternion.identity);
                Trails.SetActive(true);
                camAnimator.SetBool("Dashing", true);
            }
        }
    }

    void FixedUpdate(){ //use this for actual movement as it is more reliable.
        
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime); //move the player
        
        if (dashCooldownTimer>0f){
            dashCooldownTimer-=0.1f; //if the cooldown more than zero decrease it
        }
        if (dashDuration>0){
            dashDuration-=1; 
        }
        if (dashing==true && dashDuration<=0){
            //head.SetActive(false);
            //Instantiate(DashEffect, foot.transform.position, Quaternion.identity);
            camAnimator.SetBool("Dashing", false);
            dashing=false;
            dashCooldownTimer=3f;
            moveSpeed=moveSpeed/dashSpeed;
            Trails.SetActive(false);
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
        yield return new WaitForSeconds(staminaRegenBuffer);

        while(currentStamina < staminaMax){
            currentStamina += 2;
            staminaBar.value = currentStamina;
            yield return new WaitForSeconds(regenTick);;
        }
        regen = null;
    }
}
