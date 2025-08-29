using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Slot> slots;              // 固定格子列表
    public TextMeshProUGUI itemInfo;

    public void AddItemToInventory(ItemData item)
    {
        foreach (var slot in slots)
        {
            if (slot.item == null)         // 找到空格子
            {
                slot.SetItem(item);
                break;
            }
        }
    }

    public static void ShowItemInfo(string info)
    {
        InventoryManager instance = FindObjectOfType<InventoryManager>();
        if (instance != null)
            instance.itemInfo.text = info;
    }
}
