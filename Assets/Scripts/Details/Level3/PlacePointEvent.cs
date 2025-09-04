using UnityEngine;

public class PlacePointEvent : MonoBehaviour
{
    [Header("关联的放置点")]
    public PlacePointTrigger placePoint;

    [Header("出现的新物品")]
    public GameObject newItem;   // 预制体
    public Transform newItemPosition; //生成位置

    [Header("拾取新物品后触发")]
    public Animator peopleAnimator;
    public GameObject nextItemToAppear;   // 动画后出现的新物品
    public GameObject itemToDisappear;    // 动画后消失的旧物品

    private bool newItemSpawned = false;
    private bool newItemPicked = false;

    private void Update()
    {
        if (placePoint != null && placePoint.itemPlaced == true && !newItemSpawned)
        {
            if (newItem != null)
            {
                newItem = Instantiate(newItem,newItemPosition.position,newItemPosition.rotation);
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
