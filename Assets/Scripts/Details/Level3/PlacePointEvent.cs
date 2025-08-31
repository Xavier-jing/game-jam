using UnityEngine;

public class PlacePointEvent : MonoBehaviour
{
    [Header("关联的放置点")]
    public PlacePointTrigger placePoint;

    [Header("出现的新物品")]
    public GameObject newItem;   // 一开始隐藏，放置正确后激活

    [Header("拾取新物品后触发")]
    public Animator peopleAnimator;
    public GameObject nextItemToAppear;   // 动画后出现的新物品
    public GameObject itemToDisappear;    // 动画后消失的旧物品

    private bool newItemSpawned = false;
    private bool newItemPicked = false;

    private void Start()
    {
        if (newItem != null) 
            newItem.SetActive(false);
        if (nextItemToAppear != null) 
            nextItemToAppear.SetActive(false);
    }

    private void Update()
    {
        if (placePoint != null && placePoint.itemPlaced && !newItemSpawned)
        {
            if (newItem != null)
            {
                newItem.SetActive(true);
                newItemSpawned = true;
                Debug.Log("新物品出现");
            }
        }
        if (newItemSpawned && !newItemPicked && (newItem == null))
        {
            newItemPicked = true;
            Debug.Log("新物品被拾取！");

            if (peopleAnimator != null) {
                peopleAnimator.Play("fadein");
            }

            if (nextItemToAppear != null)
                nextItemToAppear.SetActive(true);

            if (itemToDisappear != null)
                itemToDisappear.SetActive(false);
        }
    }


}
