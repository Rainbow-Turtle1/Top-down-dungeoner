using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int inventoryHeight;
    public int inventoryWidth;
    public InventorySlot[,] inventorySlots;
    
    // Start is called before the first frame update
    void Start()
    {
        inventorySlots = new InventorySlot[inventoryWidth, inventoryHeight];
        
    } 

}
