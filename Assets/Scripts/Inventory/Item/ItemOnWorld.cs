using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public ItemData item;
    public InventoryManager inventoryManager;

    private void OnEnable()
    {
        PlayerInputHander.OnInteractPressed += TryPickup;
    }

    private void OnDisable()
    {
        PlayerInputHander.OnInteractPressed -= TryPickup;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 玩家在触发器内，按 E 才收取
        if (collision.CompareTag("Player"))
        {
            currentPlayer = collision.gameObject;
        }
    }

    private GameObject currentPlayer;

    private void TryPickup()
    {
        if (currentPlayer == null) return;
        inventoryManager.AddItemToInventory(item);
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject == currentPlayer)
        {
            currentPlayer = null; 
        }
    }
}
