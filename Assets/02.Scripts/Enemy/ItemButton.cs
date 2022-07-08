using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;

public class ItemButton : MonoBehaviour
{
    public int Item_index;
    public ItemGet itemGet;
    public GameDataObject gameDataObject;
    void Start()
    {
        itemGet = GameObject.FindWithTag("Player").GetComponent<ItemGet>();
    }
    public void Clicked_In_Drop()
    {
        if (itemGet.items[Item_index].itemType == ItemType.shortWeapon)
        {
            gameDataObject.shortWeapon.Add(itemGet.items[Item_index]);
            itemGet.items[Item_index] = null;
        }
        else if (itemGet.items[Item_index].itemType == ItemType.longWeapon)
        {
            gameDataObject.longWeapon.Add(itemGet.items[Item_index]);
            itemGet.items[Item_index] = null;
        }
        else if (itemGet.items[Item_index].itemType == ItemType.shoes)
        {
            gameDataObject.shoes.Add(itemGet.items[Item_index]);
            itemGet.items[Item_index] = null;
        }
        else if (itemGet.items[Item_index].itemType == ItemType.top)
        {
            gameDataObject.top.Add(itemGet.items[Item_index]);
            itemGet.items[Item_index] = null;
        }
        else if (itemGet.items[Item_index].itemType == ItemType.bottoms)
        {
            gameDataObject.bottoms.Add(itemGet.items[Item_index]);
            itemGet.items[Item_index] = null;
        }
        Destroy(gameObject);
    }
}
