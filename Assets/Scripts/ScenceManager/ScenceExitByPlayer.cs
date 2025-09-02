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

    private bool playerInTrigger = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            if (hintUI != null)
                hintUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            if (hintUI != null)
                hintUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInTrigger && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(SceneLoader.Instance.TransitionCoroutine(targetSceneName));
        }
    }
}
