using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;
using UnityEngine.UI;
public class EquipItem : ItemObject
{
    public static EquipItem focused;
    public void Clicked_In_Inventory()
    {
        if (focused != null)
            focused.GetComponent<Image>().color = origin_color;
        focused = this;
        gameObject.GetComponent<Image>().color = Color.blue;
        GameManager.instance.ShowInventoryStatus(item);
    }
}
