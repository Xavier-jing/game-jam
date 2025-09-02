using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScenceExitByPlayer : MonoBehaviour
{
    [Header("目标场景")]
    public string targetSceneName;

    [Header("提示UI")]
    public GameObject hintUI;

    public PlayerManager playerManager;

    private GameObject playerInTrigger = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = other.gameObject;
            if (playerInTrigger == playerManager.players[playerManager.CurrentIndex].gameObject)
            {
                if (hintUI != null)
                    hintUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject == playerInTrigger)
        {
            playerInTrigger = other.gameObject;
            if (hintUI != null)
                hintUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInTrigger == playerManager.players[playerManager.CurrentIndex].gameObject && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(SceneLoader.Instance.TransitionCoroutine(targetSceneName));
        }
    }
}
