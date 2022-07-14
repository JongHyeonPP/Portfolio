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
    public Item[] Items;//인벤토리 배열
    [Header("Player Status in Inventory")]
    public Text str;
    public Text con;
    public Text vit;
    public Text dd;
    [Header("Enemy Hp")]
    public CanvasGroup canvasGroup_hp;
    public Image image_hp;
    public Text text_hp;
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
            count = AddInventory(gameDataObject.shortWeapon ,list_item, count);
            count = AddInventory(gameDataObject.longWeapon, list_item, count);
            count = AddInventory(gameDataObject.shoes, list_item, count);
            count = AddInventory(gameDataObject.top, list_item, count);
            count = AddInventory(gameDataObject.bottoms, list_item, count);
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

    private int AddInventory(List<Item> list_item, List<GameObject> list_inventory, int count)
    {
        for (int i = 0; i < list_item.Count; i++)
        {
            list_inventory.Add(Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content));
            list_inventory[list_inventory.Count - 1].transform.GetChild(0).gameObject.GetComponent<Text>().text = list_item[i].name;
            Items[count] = list_item[i];
            list_inventory[i].GetComponent<EquipItem>().Item_index = count++;
        }
        return count;
    }

    public void EmptyInventory()
    {
        gameDataObject.shortWeapon.Clear();
        gameDataObject.longWeapon.Clear();
        gameDataObject.shoes.Clear();
        gameDataObject.top.Clear();
        gameDataObject.bottoms.Clear();
        gameDataObject.shortWeapon_C = null;
        gameDataObject.longWeapon_C = null;
        gameDataObject.shoes_C = null;
        gameDataObject.top_C = null;
        gameDataObject.bottoms_C = null;
    }
    public void EnemyHp(float health, float startHp, LivingEntity livingEntity)
    {
        StartCoroutine(ShowCanvasGroup(canvasGroup_hp));
        image_hp.fillAmount = health / startHp;
        text_hp.text = livingEntity.gameObject.name;
        if (image_hp.fillAmount > 0.6f)
        {
            image_hp.color = Color.green;
        }
        else if (image_hp.fillAmount > 0.3f)
        {
            image_hp.color = Color.yellow;
        }
        else
        {
            image_hp.color = Color.red;
        }
    }
    IEnumerator ShowCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(3f);
        canvasGroup.alpha = 0f;
    }
    public void ShowInventoryStatus(int Item_index)
    {
        str.text = string.Format("STR : {0}", Items[Item_index].str.ToString());
        con.text = string.Format("CON : {0}", Items[Item_index].con.ToString());
        vit.text = string.Format("VIT : {0}", Items[Item_index].vit.ToString());
        //Items[Item_index].
        //dd.text = string.Format("VIT : {0}", Items[Item_index].);
    }
}
