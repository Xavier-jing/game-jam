using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    public Image slotImage;           // 格子里的图片
    public ItemData item;             // 当前格子对应物品

    public void SetItem(ItemData newItem)
    {
        item = newItem;
        slotImage.sprite = item != null ? item.itemImage : null;
        slotImage.enabled = item != null;  // 没有物品隐藏图片
    }

    public void OnClick()
    {
        if (item != null)
            InventoryManager.ShowItemInfo(item.itemInfo);
    }
}

