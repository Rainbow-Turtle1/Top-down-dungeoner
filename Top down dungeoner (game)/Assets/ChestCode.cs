using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestCode : MonoBehaviour
{
    public Animator ChestAnimator;
    public GameObject ChestEmitter;
    public GameObject Lights;
    public GameObject ChestParticle1;
    public int WaitTimeToOpen = 5;

    void OnMouseOver(){
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            ChestAnimator.SetBool("OpeningChest",true);
        }
        if(Input.GetKeyUp(KeyCode.Mouse0)){
            ChestAnimator.SetBool("OpeningChest",false);
        }
    }

    void OnMouseEnter(){
        ChestAnimator.SetBool("Hovering",true);
    }

    void OnMouseExit(){
        ChestAnimator.SetBool("Hovering",false);
    }

    public void OpenTheChest(){
        ChestAnimator.SetBool("ChestIsOpen",true);
        ChestAnimator.SetTrigger("OpenChest");
    }
    public void LightsAndParticles(){
        Instantiate(ChestParticle1, ChestEmitter.transform.position, ChestEmitter.transform.rotation);
        Lights.SetActive(true);
        print("KAZAM");
    }
}
