using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChadCode : MonoBehaviour
{
    void OnMouseOver(){
        if(Input.GetMouseButtonDown(1)){
            //trigger.TriggerDialog();
            FindObjectOfType<ChadDialogeTrigger>().TriggerDialog();
        }
    }
}
