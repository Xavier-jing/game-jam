using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [Header("全屏黑色遮罩")]
    public CanvasGroup faderCanvasGroup;

    [Header("淡入淡出时间")]
    public float faderDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (faderCanvasGroup == null)
                Debug.LogError("ScreenFader: CanvasGroup 未绑定！");
            if (faderCanvasGroup != null)
            {
                faderCanvasGroup.alpha = 0f;                // 初始透明
                faderCanvasGroup.gameObject.SetActive(false); // 一开始隐藏
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator FadeIn()
    {
        if (faderCanvasGroup != null)
        {
            faderCanvasGroup.gameObject.SetActive(false); // Fade完成隐藏
            yield return faderCanvasGroup.DOFade(0f, faderDuration).WaitForCompletion();
        }
    }

    public IEnumerator FadeOut()
    {
        if (faderCanvasGroup != null)
        {
            faderCanvasGroup.gameObject.SetActive(true); // 切场景前显示
            yield return faderCanvasGroup.DOFade(1f, faderDuration).WaitForCompletion();
        }
    }

    public IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        yield return FadeOut();
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return FadeIn();
    }

}
