using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoomTrigger : MonoBehaviour
{
    public PlacePointTrigger placePoint;   
    public Animator boomAnimator;
    public GameObject oldSceneRoot;
    public GameObject newSceneRoot;

    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
            playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && placePoint != null && placePoint.itemPlaced == true)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                TriggerBoom();
            }
        }
    }

    private void TriggerBoom()
    {
         boomAnimator.SetTrigger("Boom");
         oldSceneRoot.SetActive(false);
         newSceneRoot.SetActive(true);
    }
}
