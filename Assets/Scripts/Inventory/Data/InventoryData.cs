using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData" , menuName = "Data/Inventory/InventoryData")]
public class InventoryData : ScriptableObject
{
   public List<ItemData> items = new List<ItemData>();
}
