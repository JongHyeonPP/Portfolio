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
        public GameObject Pistol;
        [SerializeField] private Dictionary<string, UMATextRecipe> Shoes;
        [SerializeField] private Dictionary<string, UMATextRecipe> Top;
        [SerializeField] private Dictionary<string, UMATextRecipe> Bottoms;
        private void Awake()
        {
            Shoes = new Dictionary<string, UMATextRecipe>();
            Top = new Dictionary<string, UMATextRecipe>();
            Bottoms = new Dictionary<string, UMATextRecipe>();
            Init_Shoes();
            Init_Top();
            Init_Bottoms();
        }

        private void Init_Bottoms()
        {
            Bottoms.Add("Pants",Resources.Load<UMATextRecipe>("Bottoms/Pants"));
            Bottoms.Add("Jeans", Resources.Load<UMATextRecipe>("Bottoms/Jeans"));
        }

        private void Init_Top()
        {
            Top.Add("Shirts", Resources.Load<UMATextRecipe>("Top/Shirts"));
            Top.Add("Shirts", Resources.Load<UMATextRecipe>("Top/Coat"));
        }

        private void Init_Shoes()
        {
            Top.Add("Boots", Resources.Load<UMATextRecipe>("Shoes/Boots"));
            Top.Add("Sneakers", Resources.Load<UMATextRecipe>("Shoes/Sneakers"));
        }
        public Item Get(string _name)
        {
            switch (_name)
            {
                //근거리 무기
                case "Sword":
                    return new Weapon("Sword", Random.Range(2, 5), Random.Range(0, 3), Random.Range(0, 3), Random.Range(15, 21), 5, Sword, ItemType.shortWeapon);
                //원거리 무기
                case "Pistol":
                    return new Weapon("Pistol", Random.Range(1, 3), Random.Range(2, 5), Random.Range(0, 3), Random.Range(10, 16), 5, Pistol, ItemType.longWeapon);
                //신발
                case "Boots":
                    return new Clothes("Boots", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, Shoes["Boots"], ItemType.shoes);
                case "Sneakers":
                    return new Clothes("Sneakers", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, Shoes["Sneakers"], ItemType.shoes);
                //상의
                case "Shirts":
                    return new Clothes("Shirts", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, Top["Shirts"],ItemType.top);
                case "Coat":
                    return new Clothes("Coat", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, Top["Coat"], ItemType.top);
                //하의
                case "Pants":
                    return new Clothes("Pants", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, Bottoms["Pants"], ItemType.bottoms);
                case "Jeans":
                    return new Clothes("Jeans", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, Bottoms["Jeans"], ItemType.bottoms);
                default:
                    return null;
            }
        }
    }
}