using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;
[CreateAssetMenu(fileName = "GameDataSO", menuName = "Create GameData", order = 0)]
public class GameDataObject : ScriptableObject//
{
    //능력치들
    public int Level { get; set; } = 1;
    public int level_max { get; }=30;
    public float Exp { get; set; }
    public float Exp_require { get; set; } = 10;
    public int Str { get; set; }
    public int con { get; set; }
    public int vit{ get; set; }
    public int status_max { get; } = 30;
    public int status_own { get; set; } = 7;
    public float maxHp { get; set; } = 100;
    public float hp { get; set; } = 100;
    public float maxHp_default = 100;
    public float damage { get; set; } = 5;
    public float weight { get; set; } = 0;
    public float max_weight { get; } = 100;
    public float def { get; set; } = 0;
    //가진 아이템들
    public List<Item> shortWeapon = new List<Item>();
    public List<Item> longWeapon = new List<Item>();
    public List<Item> shoes = new List<Item>();
    public List<Item> top = new List<Item>();
    public List<Item> bottoms = new List<Item>();
    //현재 장착중인 아이템들
    public Weapon _shortWeapon;
    public Weapon _longWeapon;
    public Cloth _shoes;
    public Cloth _top;
    public Cloth _bottoms;
}