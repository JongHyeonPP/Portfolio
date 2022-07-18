using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;
using UnityEngine.UI;
public class ItemObject : MonoBehaviour
{
    public Item item;
    public Color origin_color;
    private void Start()
    {
        origin_color = new Color(115f / 255f, 178f / 255f, 229f / 255f, 146f / 255f);
    }
    
}
