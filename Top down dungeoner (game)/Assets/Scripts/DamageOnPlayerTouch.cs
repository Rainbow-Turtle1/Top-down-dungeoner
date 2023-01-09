using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnPlayerTouch : MonoBehaviour
{
    private bool PlayerTouching=false;
    private float timetonextdamage;
    public float tickspeed = 10f;
    public int Damage = 1;
    
    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            PlayerTouching= true ;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            PlayerTouching= false ;
        }
    }
    void FixedUpdate(){
        if (PlayerTouching==true && timetonextdamage<=0){
            FindObjectOfType<PlayerMove>().DamagePlayer(Damage);
            timetonextdamage = tickspeed;
        }
        if (timetonextdamage > 0f){
            timetonextdamage-=0.1f;
        }
    }

}
