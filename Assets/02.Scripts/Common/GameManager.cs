using ItemSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;
    public static GameManager instance;
    private CanvasGroup group_inventory;
    private CanvasGroup group_status;
    private GameObject button_inventory;
    public GameDataObject gameDataObject;
    public Transform Content;//인벤토리 스크롤 안의 콘텐트
    [Header("Equiped Item Panel")]
    public Transform equiped_shortWeapon;//현재 장착 중인 장비들이 있는 Panel
    public Transform equiped_longWeapon;
    public Transform equiped_shoes;
    public Transform equiped_top;
    public Transform equiped_bottoms;
    private GameObject shortWeapon_C;//현재 장착 중인 장비 버튼 오브젝트
    private GameObject longWeapon_C;
    private GameObject shoes_C;
    private GameObject top_C;
    private GameObject bottoms_C;
    public CanvasGroup get_F;
    private List<GameObject> list_inventory;//일괄 destroy를 위한 리스트
    private bool[] isnum;
    [Header("Player Status in Inventory")]
    public Text str_I;
    public Text con_I;
    public Text vit_I;
    public Text dd_I;
    [Header("Player Status in Status")]
    public Text level_S;
    public Text str_S;
    public Text con_S;
    public Text vit_S;
    public Text dam_S;
    public Text def_S;
    [Header("Enemy Hp")]
    public CanvasGroup canvasGroup_hp;
    public Image image_hp;
    public Text text_hp;
    [Header("Exp&Level")]
    public Image image_exp;
    public Text text_level;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        button_inventory = GameObject.Find("Button-Inventory");
        group_inventory = GameObject.Find("Panel-Inventory").GetComponent<CanvasGroup>();
        group_status = GameObject.Find("Panel-Status").GetComponent<CanvasGroup>();
        list_inventory = new List<GameObject>();
        isnum = new bool[100];
        Reset();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(!isPaused)
            Inventory(true); 
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isPaused)
                Status(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Inventory(false);
            Status(false);
            EmptyItemObject();
        }
    }
    public void Pause(bool stop)
    {
        isPaused = stop;
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
            get_F.alpha = 0f;
            group_inventory.alpha = 1f;
            Cursor.lockState = CursorLockMode.None;
            AddInventory(gameDataObject.shortWeapon);
            AddInventory(gameDataObject.longWeapon);
            AddInventory(gameDataObject.shoes);
            AddInventory(gameDataObject.top);
            AddInventory(gameDataObject.bottoms);
            shortWeapon_C = AddEquip(equiped_shortWeapon, gameDataObject.shortWeapon_C);//버튼이 들어갈 부모, 게임 데이터 오브젝트의 정보
            longWeapon_C = AddEquip(equiped_longWeapon, gameDataObject.longWeapon_C);
            shoes_C = AddEquip(equiped_shoes, gameDataObject.shoes_C);
            top_C = AddEquip(equiped_top, gameDataObject.top_C);
            bottoms_C = AddEquip(equiped_bottoms, gameDataObject.bottoms_C );
        }
        else
        {
            group_inventory.alpha = 0f;
            Cursor.lockState = CursorLockMode.Locked;
        }
        button_inventory.SetActive(!stop);
        Cursor.visible = stop;
        Pause(stop);
        group_inventory.interactable = stop;
    }
    public void Status(bool stop)
    {
        if (stop)
        {
            isPaused = true;
            get_F.alpha = 0f;
            group_status.alpha = 1f;
            Cursor.lockState = CursorLockMode.None;
            level_S.text = string.Format("LV : {0}", gameDataObject.Level.ToString());
            str_S.text = string.Format("STR : {0}", gameDataObject.Str.ToString());
            con_S.text = string.Format("CON : {0}", gameDataObject.Con.ToString());
            vit_S.text = string.Format("VIT : {0}", gameDataObject.Vit.ToString());
            dam_S.text = string.Format("DAM : {0}", gameDataObject.Dam.ToString());
            def_S.text = string.Format("DEF : {0}", gameDataObject.Def.ToString());
        }
        else
        {
            group_status.alpha = 0f;
            Cursor.lockState = CursorLockMode.Locked;
            
        }
        group_status.interactable = stop;
        group_status.blocksRaycasts = stop;
        Cursor.visible = stop;
        Pause(stop);
    }

    private void EmptyItemObject()
    {
        foreach (GameObject o in list_inventory)
        {
            Destroy(o);
        }
        list_inventory.Clear();
        Destroy(shortWeapon_C);
        Destroy(longWeapon_C);
        Destroy(top_C);
        Destroy(bottoms_C);
        Destroy(shoes_C);
    }

    private GameObject AddEquip(Transform parent, Item item_c)
    {
        if (item_c==null|| item_c.name == null) return null;
        GameObject temp = Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), parent);
        temp.GetComponent<EquipItem>().item = item_c;
        temp.GetComponent<EquipItem>().isEquiped = true;
        temp.GetComponentInChildren<Text>().text = item_c.name;
        return temp;
    }


        private void AddInventory(List<Item> list_item)
        {
        for (int i = 0; i < list_item.Count; i++)
        {
            GameObject temp = Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content);
            temp.GetComponent<EquipItem>().item = list_item[i];
            temp.GetComponentInChildren<Text>().text = list_item[i].name;
            list_inventory.Add(temp);
        }
    }
    public void Reset()
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

        gameDataObject.Level = 1;
        gameDataObject.Exp = 0;
        gameDataObject.Exp_require = 10;
        gameDataObject.Str = 0;
        gameDataObject.Con = 0;
        gameDataObject.Vit = 0;
        gameDataObject.Status_own = 7;
        gameDataObject.Hp = 100;
        gameDataObject.MaxHp = 100;
        gameDataObject.Dam = 5;
        gameDataObject.Weight = 0;
        gameDataObject.Def = 0;

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
        str_I.text = string.Format("STR : {0}", item.str.ToString());
        con_I.text = string.Format("CON : {0}", item.con.ToString());
        vit_I.text = string.Format("VIT : {0}", item.vit.ToString());
        Weapon weapon;
        Clothes clothes;
        if (item.itemType == ItemType.longWeapon || item.itemType == ItemType.shortWeapon)
        {
            weapon = item as Weapon;
            dd_I.text = string.Format("DAM : {0}", weapon.damage.ToString());
        }
        else
        {
            clothes = item as Clothes;
            dd_I.text = string.Format("DEF : {0}", clothes.def.ToString());
        }
    }
    public void EquipClick()
    {
        if(EquipItem.focused!=null)
            if(!EquipItem.focused.isEquiped)
            ChangeEquip(EquipItem.focused.item.itemType);
    }
    public void OffClick()
    {
        if (EquipItem.focused.isEquiped)
        {
            switch (EquipItem.focused.item.itemType)
            {
                case ItemType.shortWeapon:
                    OffEquip(gameDataObject.shortWeapon, gameDataObject.shortWeapon_C, shortWeapon_C);
                    gameDataObject.shortWeapon_C = null;
                    shortWeapon_C = null;
                    
                    break;
                case ItemType.longWeapon:
                    OffEquip(gameDataObject.longWeapon, gameDataObject.longWeapon_C, longWeapon_C);
                    gameDataObject.longWeapon_C = null;
                    longWeapon_C = null;
                    break;
                case ItemType.top:
                    OffEquip(gameDataObject.top, gameDataObject.top_C, top_C);
                    gameDataObject.top_C = null;
                    top_C = null;
                    break;
                case ItemType.bottoms:
                    OffEquip(gameDataObject.bottoms, gameDataObject.bottoms_C, bottoms_C);
                    gameDataObject.bottoms_C = null;
                    bottoms_C = null;
                    break;
                case ItemType.shoes:
                    OffEquip(gameDataObject.shoes, gameDataObject.shoes_C, shoes_C);
                    gameDataObject.shoes_C = null;
                    shoes_C = null;
                    break;
            }
            if (!list_inventory.Contains(EquipItem.focused.gameObject))
            {
                list_inventory.Add(EquipItem.focused.gameObject);
            }
        }
    }

    private void OffEquip(List<Item> list_item, Item item_c, GameObject object_c)
    {
        list_item.Add(item_c);//리스트에 넣는다
        object_c.transform.SetParent(Content);//버튼을 옮긴다
        EquipItem temp = object_c.GetComponent<EquipItem>();//버튼의 equipitem을 받아온다.
        temp.isEquiped = false;
        gameDataObject.Str -= temp.item.str;
        gameDataObject.Con -= temp.item.con;
        gameDataObject.Vit -= temp.item.vit;
        if (item_c.itemType == ItemType.shortWeapon || item_c.itemType == ItemType.longWeapon)
        {
            Weapon weapon_temp = temp.item as Weapon;
            gameDataObject.Dam -= weapon_temp.damage;
        }
        else
        {
            Clothes clothes_temp = temp.item as Clothes;
            gameDataObject.Def -= clothes_temp.def;
        }
    }

    private void ChangeEquip(ItemType itemType)
    {
        Weapon weapon_temp;
        Clothes clothes_temp;
        //장착 중인게 있을 때와 없을 때를 구분해줘야함.
        switch (itemType)
        {
            case ItemType.shortWeapon:
                if (shortWeapon_C != null)//장착 중인게 있을 때
                {
                    OffEquip(gameDataObject.shortWeapon, gameDataObject.shortWeapon_C, shortWeapon_C);
                }
                EquipItem.focused.transform.SetParent(equiped_shortWeapon);//인벤토리에서 누른 장비의 버튼을 장착칸으로 옮긴다.
                gameDataObject.shortWeapon_C = EquipItem.focused.item as Weapon;//데이터오브젝트
                shortWeapon_C = EquipItem.focused.gameObject;//장비 칸에 들어갈 게임 오브젝트를 변수에 대입
                EquipItem.focused.isEquiped = true;
                gameDataObject.Str += EquipItem.focused.item.str;
                gameDataObject.Con += EquipItem.focused.item.con;
                gameDataObject.Vit += EquipItem.focused.item.vit;
                weapon_temp = EquipItem.focused.item as Weapon;
                gameDataObject.Dam += weapon_temp.damage;
                gameDataObject.shortWeapon.Remove(EquipItem.focused.item);
                
                break;

            case ItemType.longWeapon:
                if (longWeapon_C != null)
                {
                    OffEquip(gameDataObject.longWeapon, gameDataObject.longWeapon_C, longWeapon_C);
                }
                EquipItem.focused.transform.SetParent(equiped_longWeapon);
                gameDataObject.longWeapon_C = EquipItem.focused.item as Weapon;
                longWeapon_C = EquipItem.focused.gameObject;
                EquipItem.focused.isEquiped = true;
                gameDataObject.Str += EquipItem.focused.item.str;
                gameDataObject.Con += EquipItem.focused.item.con;
                gameDataObject.Vit += EquipItem.focused.item.vit;
                weapon_temp = EquipItem.focused.item as Weapon;
                gameDataObject.Dam += weapon_temp.damage;
                gameDataObject.longWeapon.Remove(EquipItem.focused.item);
                break;

            case ItemType.shoes:
                if (shoes_C != null)
                {
                    OffEquip(gameDataObject.shoes, gameDataObject.shoes_C, shoes_C);
                }
                EquipItem.focused.transform.SetParent(equiped_shoes);
                gameDataObject.shoes_C = EquipItem.focused.item as Clothes;
                shoes_C = EquipItem.focused.gameObject;
                EquipItem.focused.isEquiped = true;
                gameDataObject.Str += EquipItem.focused.item.str;
                gameDataObject.Con += EquipItem.focused.item.con;
                gameDataObject.Vit += EquipItem.focused.item.vit;
                clothes_temp = EquipItem.focused.item as Clothes;
                gameDataObject.Def += clothes_temp.def;
                gameDataObject.shoes.Remove(EquipItem.focused.item);
                break;

            case ItemType.top:
                if (top_C != null)
                {
                    OffEquip(gameDataObject.top, gameDataObject.top_C, top_C);
                }
                EquipItem.focused.transform.SetParent(equiped_top);
                gameDataObject.top_C = EquipItem.focused.item as Clothes;
                top_C = EquipItem.focused.gameObject;
                EquipItem.focused.isEquiped = true;
                gameDataObject.Str += EquipItem.focused.item.str;
                gameDataObject.Con += EquipItem.focused.item.con;
                gameDataObject.Vit += EquipItem.focused.item.vit;
                clothes_temp = EquipItem.focused.item as Clothes;
                gameDataObject.Def += clothes_temp.def;
                gameDataObject.top.Remove(EquipItem.focused.item);
                break;

            case ItemType.bottoms:
                if (bottoms_C != null)
                {
                    OffEquip(gameDataObject.bottoms, gameDataObject.bottoms_C, bottoms_C);
                }
                EquipItem.focused.transform.SetParent(equiped_bottoms);
                gameDataObject.bottoms_C = EquipItem.focused.item as Clothes;
                bottoms_C = EquipItem.focused.gameObject;
                EquipItem.focused.isEquiped = true;
                gameDataObject.Str += EquipItem.focused.item.str;
                gameDataObject.Con += EquipItem.focused.item.con;
                gameDataObject.Vit += EquipItem.focused.item.vit;
                clothes_temp = EquipItem.focused.item as Clothes;
                gameDataObject.Def += clothes_temp.def;
                gameDataObject.bottoms.Remove(EquipItem.focused.item);
                break;
        }
        EquipItem.focused.GetComponent<Image>().color = EquipItem.focused.origin_color;
        EquipItem.focused = null;
    }
    public void UpdateLevel(int Level)
    {
        text_level.text = Level.ToString();
    }
    public void UpdateExp(float exp)
    {
        gameDataObject.Exp += exp;
        image_exp.fillAmount = gameDataObject.Exp/gameDataObject.Exp_require;
    }
}
