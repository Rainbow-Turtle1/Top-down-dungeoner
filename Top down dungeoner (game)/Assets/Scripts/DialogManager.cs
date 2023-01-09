using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public GameObject TextBox;
    
    private Queue<string> Lines;

    void Start () {
        Lines = new Queue<string>();


    }
    public void StartDialog(Dialog dialog){
        print("player is talking to "+ dialog.name);

        nameText.text=dialog.name;

        Lines.Clear();

        TextBox.SetActive(true);



        foreach (string Line in dialog.Lines){
            Lines.Enqueue(Line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine(){
        if (Lines.Count == 0){
            EndDialog();
            return;
        }

        string Line = Lines.Dequeue();
        dialogueText.text = Line;


    }
    void EndDialog(){
        Debug.Log("End of conversation");
        TextBox.SetActive(false);
    }
}

