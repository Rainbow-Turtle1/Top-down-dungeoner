using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePlayerShow : MonoBehaviour
{
    public Animator PSanimator;
    public float nextAttackTime = 0f;
    public float attackSpeed=2f;
    
    private Vector2 movement;

    // Update is called once per frame
    void Update(){
        //process inputs here
        if(Time.time >= nextAttackTime){
            if (Input.GetKeyDown(KeyCode.Space)){
            Attack();
            nextAttackTime = Time.time + 1f / attackSpeed;
        }
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        PSanimator.SetFloat("Speed", movement.sqrMagnitude);  
    }

    void Attack(){
        PSanimator.SetTrigger("Attack");
    }
    
}
