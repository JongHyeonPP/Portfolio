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
            Destroy(this, 60f);
        }
    }
}