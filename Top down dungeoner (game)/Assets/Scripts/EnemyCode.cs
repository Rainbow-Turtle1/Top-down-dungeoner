using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCode : MonoBehaviour
{
    public Animator animator;
    public int maxHP= 60;
    private int currentHP;
    public GameObject deathEffect;
    public GameObject hitEffect;
    public Animator camAnimator;

    public float despawnTime = 6f;
    public bool Despawn= true;


    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage){
        currentHP -= damage;
        Instantiate(hitEffect, transform.position, Quaternion.identity);

        animator.SetTrigger("Hit");

        if (currentHP<=1){
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            camAnimator.SetTrigger("shake");
            Die();

        }
    }
    
    void Die(){
        animator.SetBool("Dead",true);

        this.enabled = false;

        GetComponent<Collider2D>().enabled = false;
        
        if (Despawn == true){
        Destroy(gameObject,despawnTime);

        }
    }
}
