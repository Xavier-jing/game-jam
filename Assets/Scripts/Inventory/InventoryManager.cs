using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Slot> slots;
    public TextMeshProUGUI itemInfo;

    [Header("丢弃相关")]
    public GameObject itemPrefab;   
    public Transform dropPoint;

    private Slot selectedSlot;

    public void AddItemToInventory(ItemData item)
    {
        foreach (var slot in slots)
        {
            if (slot.item == null)
            {
                slot.SetItem(item);
                return;
            }
        }
    }

    public static void ShowItemInfo(string info)
    {
        var instance = FindObjectOfType<InventoryManager>();
        if (instance != null)
        {
            instance.itemInfo.text = info;
        }
    }

    public void SelectSlot(Slot slot)
    {
        selectedSlot = slot;
        Debug.Log("[InventoryManager] 已选中槽位: " + (slot.item != null ? slot.item.itemName : "空"));
    }

    public Slot GetSelectedSlot()
    {
        Debug.Log("[InventoryManager] GetSelectedSlot: " + (selectedSlot != null ? selectedSlot.item?.itemName : "null"));
        return selectedSlot;
    }


    public void DropSelectedItem(Vector3? overridePos = null)
    {
        if (selectedSlot == null || selectedSlot.item == null) return;

        Debug.Log("[InventoryManager] DropSelectedItem: " + selectedSlot.item.itemName);

        var dropItem = selectedSlot.item;
        selectedSlot.ClearSlot();

        Vector3 pos = overridePos ?? (dropPoint != null ? dropPoint.position : transform.position);
        GameObject worldItem = Instantiate(itemPrefab, pos, Quaternion.identity);
        worldItem.transform.localScale = Vector3.one * 0.1f;

        var iow = worldItem.GetComponent<ItemOnWorld>();
        if (iow != null) iow.SetItem(dropItem, this);

        if (itemInfo) itemInfo.text = "";

        Debug.Log("[InventoryManager] 清空 selectedSlot");
        selectedSlot = null;
    }
}
