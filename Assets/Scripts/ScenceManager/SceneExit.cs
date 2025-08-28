using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneExit : MonoBehaviour
{
    [Tooltip("需要切换的场景名称")]
    public string newSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TransitionInternal();
        }
    }

    public void TransitionInternal()
    {
        SceneLoader.Instance.TransitionToScene(newSceneName);
    }
}
