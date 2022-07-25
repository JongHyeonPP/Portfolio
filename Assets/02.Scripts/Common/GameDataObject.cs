using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;
[CreateAssetMenu(fileName = "GameDataSO", menuName = "Create GameData", order = 0)]
public class GameDataObject : ScriptableObject
{
    //능력치들
    public int Level { get; set; } = 1;
    public int Level_max { get; }=30;
    private float exp;
    public float Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
            if (exp > Exp_require)
            {
                exp -= Exp_require;
                Level++;
                Exp_require *= 1.2f;
                GameManager.instance.UpdateLevel(Level);
            }
        }
    }
    public float Exp_require { get; set; } = 10f;
    public int Str { get; set; }
    public int Con { get; set; }
    public int Vit{ get; set; }
    public int Status_max { get; } = 30;
    public int Status_own { get; set; } = 7;
    public float MaxHp { get; set; } = 100;
    public float Hp { get; set; } = 100;
    public float Dam { get; set; } = 5;
    public float Weight { get; set; } = 0;
    public float Max_weight { get; } = 100;
    public float Def { get; set; } = 5;
    //가진 아이템들
    public List<Item> shortWeapon = new List<Item>();
    public List<Item> longWeapon = new List<Item>();
    public List<Item> shoes = new List<Item>();
    public List<Item> top = new List<Item>();
    public List<Item> bottoms = new List<Item>();
    //현재 장착중인 아이템들
    public Weapon shortWeapon_C;
    public Weapon longWeapon_C;
    public Clothes shoes_C;
    public Clothes top_C;
    public Clothes bottoms_C;
}