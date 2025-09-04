using UnityEngine;

public class PlacePointTrigger : MonoBehaviour
{
    [SerializeField]
    private string requiredItemName ;

    [Header("关联的库存管理器")]
    [SerializeField] 
    private InventoryManager inv;
    [SerializeField] 
    private Transform dropOffset;
    public bool itemPlaced = false;

    private bool playerInside = false;

    private void OnEnable()
    {
        PlayerInputHander.OnInteractPressed += HandleInteract;
    }

    private void OnDisable()
    {
        PlayerInputHander.OnInteractPressed -= HandleInteract;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("进入放置区域");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
            Debug.Log("离开放置区域");
        }
    }

    private void HandleInteract(PlayerInputHander handler)
    {
        if(!playerInside) return;
        if (inv == null) return;

        if (playerInside && !itemPlaced)
        {
            TryPlaceItem();
            return;
        }
        Vector3 pos = dropOffset != null ? dropOffset.position : (transform.position + Vector3.right * 0.5f);
        inv.DropSelectedItem(pos);
    }

    private void TryPlaceItem()
    {
        Slot slot = inv.GetSelectedSlot();
        if (slot != null && slot.item != null && slot.item.itemName == requiredItemName)
        {
            Debug.Log($"放置成功: {slot.item.itemName}");
            Vector3 pos = dropOffset != null ? dropOffset.position : transform.position;
            inv.DropSelectedItem(pos);
            itemPlaced = true;
        }
        else
        {
            Debug.Log("放置失败: 没有选中物品");
        }
    }
}
