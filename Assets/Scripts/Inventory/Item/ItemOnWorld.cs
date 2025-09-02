using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public ItemData item;
    public InventoryManager inventoryManager;

    private SpriteRenderer sr;
    //private bool playerInRange = false;
    private PlayerInputHander currentPlayerInRange;
    public static int NearbyCount = 0;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    private void OnEnable()
    {
        PlayerInputHander.OnInteractPressed += TryPickup;
    }

    private void OnDisable()
    {
        PlayerInputHander.OnInteractPressed -= TryPickup;
    }

    private void Start()
    {
        if (item != null && sr != null)
            sr.sprite = item.itemImage;
    }

    /// <summary>
    /// 丢弃时调用，刷新数据和图片
    /// </summary>
    public void SetItem(ItemData newItem, InventoryManager inv)
    {
        item = newItem;
        inventoryManager = inv;

        if (sr != null && item != null)
            sr.sprite = item.itemImage;
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //        playerInRange = true;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NearbyCount++;
            currentPlayerInRange = collision.GetComponent<PlayerInputHander>();
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NearbyCount = Mathf.Max(0, NearbyCount - 1);
           var handler = collision.GetComponent<PlayerInputHander>();
            if (handler == currentPlayerInRange)
            {
                currentPlayerInRange = null;
            }
        }
    }

    private void TryPickup(PlayerInputHander handler)
    {
        //if (!playerInRange) return;
        if (handler != currentPlayerInRange) return;
        if (inventoryManager == null || item == null) return;

        inventoryManager.AddItemToInventory(item);
        Destroy(gameObject);
    }
}
