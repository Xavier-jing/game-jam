using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }

    //切换场景
    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(TransitionCoroutine(sceneName));
    }


    public IEnumerator TransitionCoroutine(string newSceneName)
    {
      
        yield return StartCoroutine(ScreenFader.Instance.FadeOut());
        yield return SceneManager.LoadSceneAsync(newSceneName);
        yield return StartCoroutine(ScreenFader.Instance.FadeIn());
    }

}