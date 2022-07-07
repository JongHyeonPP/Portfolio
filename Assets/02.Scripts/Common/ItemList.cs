using System.Collections;
using System.Collections.Generic;
using UMA;
using UnityEngine;

namespace ItemSpace
{
    public class ItemList : MonoBehaviour
    {
        //아이템 목록, 무기는 장착중인 것 넣어야한다.
        public GameObject Sword;
        public GameObject Gun;
        [SerializeField] private Dictionary<string, UMATextRecipe> Shoes;
        [SerializeField] private Dictionary<string, UMATextRecipe> Top;
        [SerializeField] private Dictionary<string, UMATextRecipe> Bottoms;
        private void Awake()
        {
            Init_Shoes();
            Init_Top();
            Init_Bottoms();
        }

        private void Init_Bottoms()
        {
            Bottoms.Add("Pants",Resources.Load<UMATextRecipe>("Bottoms/Pants"));
        }

        private void Init_Top()
        {
            Top.Add("Shirts", Resources.Load<UMATextRecipe>("Top/Shirts"));
        }

        private void Init_Shoes()
        {
            Top.Add("Boots", Resources.Load<UMATextRecipe>("Shoes/Boots"));
        }
        public Item Get(string _name)
        {
            switch (_name)
            {
                case "Sword":
                    return new Weapon("Sword", Random.Range(2, 5), Random.Range(0, 3), Random.Range(0, 3), Random.Range(15, 21), 5, Sword, ItemType.shortWeapon);
                case "Gun":
                    return new Weapon("Gun", Random.Range(1, 3), Random.Range(2, 5), Random.Range(0, 3), Random.Range(10, 16), 5, Gun, ItemType.longWeapon);
                case "Boots":
                    return new Clothes("Boots", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, Shoes["Boots"], ItemType.shoes);
                case "Shirts":
                    return new Clothes("Shirts", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, Top["Shirts"],ItemType.top);
                case "Pants":
                    return new Clothes("Pants", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, Bottoms["Pants"], ItemType.bottoms);
                default:
                    return null;
            }
        }
    }
}