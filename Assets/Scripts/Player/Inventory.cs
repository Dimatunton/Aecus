using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    static List<string> PlayerInventory = new List<string>();
    public static void addItem(string ItemName)
    {
        //check if item exist
        if (PlayerInventory.Contains(ItemName))
        {
            print("It's already on your inventory");
        }
        else
        {
            PlayerInventory.Add(ItemName);
        }
    }
     
    public static bool isinInventory(string ItemName)
    {
        if (PlayerInventory.Contains(ItemName))
        {
            print("It's on your inventory");
            return true;
        }
        else
        {
            return false;
        }
    }
}
