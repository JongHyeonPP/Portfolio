using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;
using UnityEngine.UI;
public class EquipItem : MonoBehaviour
{
    public int Item_index;
    public GameDataObject gameDataObject;
    public Transform Content;
    public static EquipItem focused;
    public Color origin_color;
    public Text str;
    public Text con;
    public Text vit;
    public Text dd;
    private void Start()
    {
        origin_color = new Color(115f / 255f, 178f / 255f, 229f / 255f,146f/255f);
    }
    public void Clicked_In_Inventory()
    {
        if (focused != null)
            focused.GetComponent<Image>().color = origin_color;
        focused = this;
        Item temp = GameManager.instance.Items[focused.Item_index];


        gameObject.GetComponent<Image>().color = Color.blue;
        EquipClick();
    }

    public void EquipClick()
    {
        if (GameManager.instance.Items[Item_index].itemType == ItemType.shortWeapon)
        {
            if (gameDataObject.shortWeapon_C != null)
            {
                GameObject temp = Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content);
                temp.transform.GetChild(0).gameObject.GetComponent<Text>().text = gameDataObject.shortWeapon_C.name;
            }
            gameDataObject.shortWeapon_C = GameManager.instance.Items[focused.Item_index] as Weapon;
        }

        if (GameManager.instance.Items[Item_index].itemType == ItemType.longWeapon)
        {
            if (gameDataObject.longWeapon_C != null)
            {
                GameObject temp = Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content);
                temp.transform.GetChild(0).gameObject.GetComponent<Text>().text = gameDataObject.longWeapon_C.name;
            }
            gameDataObject.longWeapon_C = GameManager.instance.Items[Item_index] as Weapon;
        }

        if (GameManager.instance.Items[Item_index].itemType == ItemType.shoes)
        {
            if (gameDataObject.shoes_C != null)
            {
                GameObject temp = Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content);
                temp.transform.GetChild(0).gameObject.GetComponent<Text>().text = gameDataObject.shoes_C.name;
            }
            gameDataObject.shoes_C = GameManager.instance.Items[Item_index] as Clothes;
        }

        if (GameManager.instance.Items[Item_index].itemType == ItemType.top)
        {
            if (gameDataObject.top_C != null)
            {
                GameObject temp = Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content);
                temp.transform.GetChild(0).gameObject.GetComponent<Text>().text = gameDataObject.top_C.name;
            }
            gameDataObject.top_C = GameManager.instance.Items[Item_index] as Clothes;
        }

        if (GameManager.instance.Items[Item_index].itemType == ItemType.bottoms)
        {
            if (gameDataObject.bottoms_C != null)
            {
                GameObject temp = Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content);
                temp.transform.GetChild(0).gameObject.GetComponent<Text>().text = gameDataObject.bottoms_C.name;
            }
            gameDataObject.bottoms_C = GameManager.instance.Items[Item_index] as Clothes;
        }
        GameManager.instance.Items[Item_index] = null;
        Destroy(focused.gameObject);
    }
}
