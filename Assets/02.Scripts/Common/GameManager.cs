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
    public Transform equiped_shortWeapon;//현재 장착 중인 장비들이 있는 Panel
    public Transform equiped_longWeapon;
    public Transform equiped_shoes;
    public Transform equiped_top;
    public Transform equiped_bottoms;
    public GameObject shortWeapon_C;
    public GameObject longWeapon_C;
    public GameObject shoes_C;
    public GameObject top_C;
    public GameObject bottoms_C;
    public CanvasGroup get_f;
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
        Camera.main.GetComponent<ThirdPersonOrbitCamBasic>().enabled = !stop;
    }
    public void Inventory(bool stop)
    {
        if (stop)
        {
            get_f.alpha = 0f;
            inventory.alpha = 1f;
            Cursor.lockState = CursorLockMode.None;
            AddInventory(gameDataObject.shortWeapon);
            AddInventory(gameDataObject.longWeapon);
            AddInventory(gameDataObject.shoes);
            AddInventory(gameDataObject.top);
            AddInventory(gameDataObject.bottoms);
            AddEquip(ItemType.shortWeapon);
            AddEquip(gameDataObject.longWeapon_C);
            AddEquip(gameDataObject.shoes_C);
            AddEquip(gameDataObject.top_C);
            AddEquip(gameDataObject.bottoms_C);
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

    private void AddEquip(ItemType itemType)
    {
        GameObject temp = Instantiate(Resources.Load<GameObject>("Equip_Button_Item"), parent);
        temp.GetComponent<EquipedItem>().item = gameDataObject.shortWeapon_C;
        temp.GetComponentInChildren<Text>().text = list_item[i].name;
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
            ChangeEquip(EquipItem.focused.item.itemType);
    }
    private void ChangeEquip(ItemType itemType)
    {
        //부모를 바꾸기만 하면 안되고 스크립트 상에서도 변수를 교환해줘야 함.
        //1.focused에 있는 item을 GameDataObject의 C에 넣고 C에 있는걸 GameDataObject의 리스트에 넣는다.
        //2.현재 스크립트의 C에 focused의 gameObject가 들어간다.
        //3.focused에 색을 origin으로 바꾸고 focused에 null을 준다.
        switch (itemType)
        {
            case ItemType.shortWeapon:
                shortWeapon_C.transform.SetParent(Content);//장착된 장비의 버튼을 인벤토리로 옮긴다.
                EquipItem.focused.transform.SetParent(equiped_shortWeapon);//인벤토리에서 누른 장비의 버튼을 장착칸으로 옮긴다.
                gameDataObject.shortWeapon.Add(gameDataObject.shortWeapon_C);//데이터오브젝트
                gameDataObject.shortWeapon_C = EquipItem.focused.item as Weapon;//데이터오브젝트
                shortWeapon_C = EquipItem.focused.gameObject;//장비 칸에 들어갈 게임 오브젝트를 변수에 대입
                break;
            case ItemType.longWeapon:
                longWeapon_C.transform.SetParent(Content);
                EquipItem.focused.transform.SetParent(equiped_longWeapon);
                gameDataObject.longWeapon.Add(gameDataObject.longWeapon_C);
                gameDataObject.longWeapon_C = EquipItem.focused.item as Weapon;
                longWeapon_C = EquipItem.focused.gameObject;
                break;
            case ItemType.shoes:
                shoes_C.transform.SetParent(Content);
                EquipItem.focused.transform.SetParent(equiped_shoes);
                gameDataObject.shoes.Add(gameDataObject.shoes_C);
                gameDataObject.shoes_C = EquipItem.focused.item as Clothes;
                shoes_C = EquipItem.focused.gameObject;
                break;
            case ItemType.top:
                top_C.transform.SetParent(Content);
                EquipItem.focused.transform.SetParent(equiped_top);
                gameDataObject.top.Add(gameDataObject.top_C);
                gameDataObject.top_C = EquipItem.focused.item as Clothes;
                top_C = EquipItem.focused.gameObject;
                break;
            case ItemType.bottoms:
                bottoms_C.transform.SetParent(Content);
                EquipItem.focused.transform.SetParent(equiped_bottoms);
                gameDataObject.bottoms.Add(gameDataObject.bottoms_C);
                gameDataObject.bottoms_C = EquipItem.focused.item as Clothes;
                bottoms_C = EquipItem.focused.gameObject;
                break;
        }
        EquipItem.focused.GetComponent<Image>().color = EquipItem.focused.origin_color;
        EquipItem.focused = null;
    }
}
