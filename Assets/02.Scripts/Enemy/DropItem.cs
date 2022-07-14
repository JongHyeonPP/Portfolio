using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSpace
{
    public class DropItem : MonoBehaviour
    {
        public Item[] items = new Item[3];
        void Start()
        {
            items = ItemList.instance.GetRandom();
            if (items[0] == null && items[1] == null && items[2] == null)
                Destroy(gameObject);
            Destroy(gameObject, 60f);
        }
    }
}