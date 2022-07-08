using ItemSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;
    public static GameManager instance;
    private CanvasGroup inventory;
    private GameObject button_inventory;
    public GameDataObject gameDataObject;
    public Transform Content;
    public CanvasGroup get_f;
    public Item[] Items;
    public Text str;
    public Text con;
    public Text vit;
    public Text dd;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Items = new Item[100];
        Cursor.visible=false;
        Cursor.lockState = CursorLockMode.Locked;
        button_inventory = GameObject.Find("Button-Inventory");
        inventory = GameObject.Find("Panel-Inventory").GetComponent<CanvasGroup>();
        EmptyInventory();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Inventory(true);
            get_f.alpha = 0f;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Inventory(false);
        }
    }
    public void Pause(bool stop)
    {
        Time.timeScale = (stop) ? 0.0f : 1.0f;
        foreach (var x in GameObject.FindWithTag("Player").GetComponents<GenericBehaviour>())
        {
            x.enabled = !stop;
        }
        //var scripts = playerObj.GetComponents<MonoBehaviour>();

        //foreach (var script in scripts)
        //{
        //    script.enabled = !stop;
        //}
        Camera.main.GetComponent<ThirdPersonOrbitCamBasic>().enabled = !stop;
    }
    public void Inventory(bool stop)
    {
        if (stop)
        {
            inventory.alpha = 1f;
            Cursor.lockState = CursorLockMode.None;
            List<GameObject> list_item = new List<GameObject>();
            int count = 0;
            for(int i=0;i<gameDataObject.shortWeapon.Count;i++)
            {
                list_item.Add(Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content));
                list_item[list_item.Count-1].transform.GetChild(0).gameObject.GetComponent<Text>().text = gameDataObject.shortWeapon[i].name;
                Items[count] = gameDataObject.shortWeapon[i];
                list_item[i].GetComponent<EquipItem>().Item_index = count++;
                
            }
            for (int i = 0; i < gameDataObject.longWeapon.Count; i++)
            {
                list_item.Add(Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content));
                list_item[list_item.Count - 1].transform.GetChild(0).gameObject.GetComponent<Text>().text = gameDataObject.longWeapon[i].name;
                Items[count] = gameDataObject.longWeapon[i];
                list_item[i].GetComponent<EquipItem>().Item_index = count++;
            }
            for (int i = 0; i < gameDataObject.shoes.Count; i++)
            {
                list_item.Add(Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content));
                list_item[list_item.Count - 1].transform.GetChild(0).gameObject.GetComponent<Text>().text = gameDataObject.shoes[i].name;
                Items[count] = gameDataObject.shoes[i];
                list_item[i].GetComponent<EquipItem>().Item_index = count++;
            }
            for (int i = 0; i < gameDataObject.top.Count; i++)
            {
                list_item.Add(Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content));
                list_item[list_item.Count - 1].transform.GetChild(0).gameObject.GetComponent<Text>().text = gameDataObject.top[i].name;
                Items[count] = gameDataObject.top[i];
                list_item[i].GetComponent<EquipItem>().Item_index = count++;
            }
            for (int i = 0; i < gameDataObject.bottoms.Count; i++)
            {
                list_item.Add(Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content));
                list_item[list_item.Count - 1].transform.GetChild(0).gameObject.GetComponent<Text>().text = gameDataObject.bottoms[i].name;
                Items[count] = gameDataObject.bottoms[i];
                list_item[i].GetComponent<EquipItem>().Item_index = count++;
            }
        }
        else
        {
            inventory.alpha = 0f;
            Cursor.lockState = CursorLockMode.Locked;
        }
        button_inventory.SetActive(!stop);
        Cursor.visible = stop;
        Pause(stop);
        inventory.interactable = stop;
    }
    public void EmptyInventory()
    {
        gameDataObject.shortWeapon.Clear();
        gameDataObject.longWeapon.Clear();
        gameDataObject.shoes.Clear();
        gameDataObject.top.Clear();
        gameDataObject.bottoms.Clear();
    }
}
