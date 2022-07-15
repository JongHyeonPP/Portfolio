using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;
using UnityEngine.UI;
public class EquipItem : MonoBehaviour
{
    public Item item;
    public GameDataObject gameDataObject;
    public Transform Content;
    public static EquipItem focused;
    public Color origin_color;
    private void Start()
    {
        origin_color = new Color(115f / 255f, 178f / 255f, 229f / 255f,146f/255f);
    }
    public void Clicked_In_Inventory()
    {
        if (focused != null)
            focused.GetComponent<Image>().color = origin_color;
        focused = this;
        gameObject.GetComponent<Image>().color = Color.blue;
        GameManager.instance.ShowInventoryStatus(item);
    }
}
