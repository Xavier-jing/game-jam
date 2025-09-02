using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image slotImage;
    public ItemData item;
    private Button button;
    [SerializeField] 
    private InventoryManager inventoryManager;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    public void SetItem(ItemData newItem)
    {
        item = newItem;
        slotImage.sprite = item != null ? item.itemImage : null;
        slotImage.enabled = item != null;
    }

    public void ClearSlot()
    {
        item = null;
        slotImage.sprite = null;
        slotImage.enabled = false;
    }

    public void OnClick()
    {
        if (item != null)
        {
            InventoryManager.ShowItemInfo(item.itemInfo);
            inventoryManager.SelectSlot(this);
        }
    }
}
