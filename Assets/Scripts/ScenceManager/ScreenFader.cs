using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [Header("全屏黑色遮罩")]
    public Image faderImage;

    [Header("淡入淡出时间")]
    public float faderDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (faderImage == null)
                Debug.LogError("ScreenFader: CanvasGroup 未绑定！");
            if (faderImage != null)
            {
                Color c = faderImage.color;
                c.a = 0f;
                faderImage.color = c;                // 初始透明
                faderImage.gameObject.SetActive(false); // 一开始隐藏
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator FadeIn()
    {
        if (faderImage != null)
        {
            yield return faderImage.DOFade(0f, faderDuration).WaitForCompletion();
            faderImage.gameObject.SetActive(false);
        }
    }

    public IEnumerator FadeOut()
    {
        if (faderImage != null)
        {
            faderImage.gameObject.SetActive(true); // 切场景前显示
            yield return faderImage.DOFade(1f, faderDuration).WaitForCompletion();
        }
    }

    public IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        yield return FadeOut();
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return FadeIn();
    }
}
