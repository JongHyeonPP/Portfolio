using System.Collections;
using System.Collections.Generic;
using UMA;
using UMA.CharacterSystem;
using UnityEngine;

namespace ItemSpace
{
    public enum ItemType { shortWeapon, longWeapon, shoes, top, bottoms }
    [System.Serializable]
    public class Item
    {
        public string name;
        public int str;
        public int con;
        public int vit;
        
        
        public float weight;//무게
        public float durability;//내구도
        public ItemType itemType;
        public Item()
        {
            durability = 100f;
        }
    }
    [System.Serializable]
    public class Clothes : Item
        {
            public DynamicCharacterAvatar Avatar;
            public UMATextRecipe Recipe;
            public float def;//방어도
            public Clothes(string _name, int _str, int _con, int _vit, float _def, float _weight, UMATextRecipe _Recipe, ItemType _itemType)//이름, 능력치, 텍스쳐, 부위()
            {
                itemType = _itemType;
                Avatar = GameObject.FindGameObjectWithTag("Player").GetComponent<DynamicCharacterAvatar>();
                name = _name;str = _str;con = _con;vit = _vit;def = _def;weight = _weight;Recipe = _Recipe;
            }
        }
    [System.Serializable]
    public class Weapon : Item
    {
        public float damage;//공격력
        public Weapon(string _name, int _str, int _con, int _vit, float _damage, float _weight, ItemType _itemType)
        {
            name = _name; str = _str; con = _con; vit = _vit; damage = _damage; weight = _weight; itemType = _itemType;
        }
    }
}