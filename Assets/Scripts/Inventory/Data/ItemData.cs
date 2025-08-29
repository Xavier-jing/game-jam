using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Data/Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    [TextArea]
    public string itemInfo;
    public bool isCollected;
}
