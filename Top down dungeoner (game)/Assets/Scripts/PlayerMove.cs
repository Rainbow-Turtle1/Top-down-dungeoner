using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public LayerMask hitable;

    public float meleeRange;
    public float nextAttackTime = 0f;
    public float attackSpeed=2f;

    private float dustFreq;
    public float dusttime=2f;
    public float dashcooldown;
    public float dashLenthSec = 1;
   
    public float dashspeed = 1.3f; 
    private int dashtime;
    public int attackDmg;
    public int life; 
   
    public Transform attackPoint;

    private bool dead = false;
    private bool dash = false;

    private Animator anim;
    public Animator camAnimator;
    public Animator HeadAnimator;
    public Animator LegAnimator;
    public Animator ScreenEffect;

    public GameObject trailEffect;
    public GameObject DashEffect;
    public GameObject foot;
    public GameObject head;
    public GameObject player;
    Camera cam;
    public GameObject[] hearts;
    public GameObject PlayerDamaged;

    Vector2 movement;

    public Slider staminaBar;
    public int staminaMax = 500;
    private int currentStamina;
    public float regenTick = 0.2f;
    private Coroutine regen;

    public static PlayerMove instance;

    private void Awake(){
        instance = this;
    }

    private void Start(){
        player = this.gameObject;
        life = hearts.Length;
        currentStamina = staminaMax;
        staminaBar.maxValue = staminaMax;
        staminaBar.value = staminaMax;
    }

    void Update(){
        if(dead == true){
            //dead
            print("player died");
        }else if( life == 1){
            ScreenEffect.SetBool("OneLife", true);

        }
        //process inputs here
        if(Time.time >= nextAttackTime){
            if (Input.GetKeyDown(KeyCode.Space)){
            Attack();
            nextAttackTime = Time.time + 1f / attackSpeed;
            }
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        if (movement.x != 0.0 || movement.y != 0.0){
            if (dustFreq <=0) {
                //Instantiate(trailEffect, foot.transform.position, Quaternion.identity);
                dustFreq=dusttime;
            } else {
                dustFreq -= Time.deltaTime;
            }
        }
        if (movement.x < 0.0){
            transform.localScale = new Vector2(-1f,1f);
            //sr.flipX=true;
        }
        if (movement.x > 0.0){
            transform.localScale = new Vector2(1f,1f);
            //sr.flipX=false;

        }
        movement.y = Input.GetAxisRaw("Vertical");
        //nimator.SetFloat("Speed", movement.sqrMagnitude);
        HeadAnimator.SetFloat("Speed", movement.sqrMagnitude);
        LegAnimator.SetFloat("Speed", movement.sqrMagnitude);
        
        if (Input.GetKeyDown(KeyCode.Mouse1)){
            //Interact();
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100)){
                //what to do if raycast hit something
                Interactable interactable = hit.collider.GetComponent<Interactable>();
            }

        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashcooldown<=0f){
            
            if (currentStamina - 20 >= 0){
                currentStamina = currentStamina - 20;
                staminaBar.value = currentStamina;
                head.SetActive(true);
                Instantiate(DashEffect, foot.transform.position, Quaternion.identity);
                PlayerMove.instance.UseStamina(60);
                moveSpeed=(moveSpeed*dashspeed);
                dash=true;
                dashtime=(Mathf.RoundToInt(60*dashLenthSec));
                camAnimator.SetBool("Dashing", true);
                    if (regen != null){
                        StopCoroutine(regen);
                    }
                regen = StartCoroutine(RegenStamina());
            }
        }
    }

    void FixedUpdate(){
        //use this for actual movement as it is more reliable.
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        if (dashcooldown>0f){
            dashcooldown-=0.1f;
        }
        if (dashtime>0){
            dashtime-=1;
        }
        if (dash==true && dashtime<=0){
            head.SetActive(false);
            Instantiate(DashEffect, foot.transform.position, Quaternion.identity);
            camAnimator.SetBool("Dashing", false);
            dash=false;
            dashcooldown=3f;
            moveSpeed=moveSpeed/dashspeed;
        }

    }
    void Attack(){
        //anim
        //animator.SetTrigger("Attack");
        
        //range detection
        Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, meleeRange, hitable);

        //damage
        foreach(Collider2D enemy in hit){
            enemy.GetComponent<EnemyCode>().TakeDamage(attackDmg);
        }

    }
    void OnDrawGizmosSelected(){

        //if (attackPoint == null){
        //    return;
        //}

        Gizmos.DrawWireSphere(attackPoint.position, meleeRange);
    }
    public void DamagePlayer(int d){ //damageplayer function
        if(life >= 1){
            life -= d;
            anim = hearts[life].GetComponent<Animator>();
            anim.SetTrigger("Damage");
            camAnimator.SetTrigger("playerhit");
            ScreenEffect.SetTrigger("Hit");
            //animator.SetTrigger("Hit");
            anim.SetBool("Empty",true);
            Instantiate(PlayerDamaged, transform.position, Quaternion.identity);
            if(life<1){
                dead = true;
            }

        }
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

    //void Interact(){}
}
