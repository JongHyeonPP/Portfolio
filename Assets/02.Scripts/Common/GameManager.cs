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
    public Transform Content;//인벤토리 스크롤 안의 콘텐트
    public Transform equiped;//현재 장착 중인 장비들이 있는 Panel
    public GameObject shortWeapon_C;
    public GameObject longWeapon_C;
    public GameObject shoes_C;
    public GameObject top_C;
    public GameObject bottoms_C;
    public CanvasGroup get_f;
    public Item[] Items;//인벤토리 배열
    [Header("Player Status in Inventory")]
    public Text str;
    public Text con;
    public Text vit;
    public Text dd;
    public Text num;
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
        Cursor.visible = false;
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
            AddInventory(gameDataObject.shortWeapon);
            AddInventory(gameDataObject.longWeapon);
            AddInventory(gameDataObject.shoes);
            AddInventory(gameDataObject.top);
            AddInventory(gameDataObject.bottoms);
            AddEquip(shortWeapon_C, gameDataObject.shortWeapon_C);
            AddEquip(longWeapon_C, gameDataObject.longWeapon_C);
            AddEquip(shoes_C, gameDataObject.shoes_C);
            AddEquip(top_C, gameDataObject.top_C);
            AddEquip(bottoms_C, gameDataObject.bottoms_C);
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

    private void AddEquip(GameObject _object, Item _item)
    {
        _object = Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), equiped);
        _object.GetComponent<EquipItem>().item = _item;
        _object.GetComponentInChildren<Text>().text = _item.name;
    }

    private void AddInventory(List<Item> list_item)
    {
        for (int i = 0; i < list_item.Count; i++)
        {
            GameObject temp = Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content);
            temp.GetComponent<EquipItem>().item = list_item[i];
            temp.GetComponentInChildren<Text>().text = list_item[i].name;
        }
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
    public void ShowInventoryStatus(Item item)
    {
        str.text = string.Format("STR : {0}", item.str.ToString());
        con.text = string.Format("CON : {0}", item.con.ToString());
        vit.text = string.Format("VIT : {0}", item.vit.ToString());
        Weapon weapon;
        Clothes clothes;
        if (item.itemType == ItemType.longWeapon || item.itemType == ItemType.shortWeapon)
        {
            weapon = item as Weapon;
            dd.text = string.Format("DAM : {0}", weapon.damage.ToString());
        }
        else
        {
            clothes = item as Clothes;
            dd.text = string.Format("DEF : {0}", clothes.def.ToString());
        }
    }
    public void EquipClick()
    {
        if (EquipItem.focused.item.itemType == ItemType.shortWeapon)//여기 하는 중
        {
            ChangeEquip(gameDataObject.shortWeapon_C);
        }

    }
    private void ChangeEquip(Item equiped_item)
    {
        
    }
}
