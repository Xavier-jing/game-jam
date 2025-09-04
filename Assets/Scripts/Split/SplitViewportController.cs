using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 分割线控制器 - 实现可拖动的水平分割线，将屏幕分为左右两个相机视口
/// 左侧相机：左边界固定为0，右边界等于分割线位置
/// 右侧相机：右边界固定为1，左边界等于分割线位置
/// </summary>
[DisallowMultipleComponent]
public class SplitViewportController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("相机设置")]
    [Tooltip("左侧相机 - 显示过去/左侧内容")]
    public Camera leftCamera;
    [Tooltip("右侧相机 - 显示未来/右侧内容")]
    public Camera rightCamera;

    [Header("UI 组件")]
    [Tooltip("Canvas 组件，用于坐标转换")]
    public Canvas canvas;
    [Tooltip("分割线的 RectTransform 组件")]
    public RectTransform divider;

    [Header("行为设置")]
    [Tooltip("分割线位置比例 (0-1)，0=全左，1=全右")]
    [Range(0f, 1f)]
    public float splitRatio = 0.5f;

    [Tooltip("接近边缘时自动吸附到满屏；0 表示关闭吸附")]
    [Range(0f, 0.2f)]
    public float edgeSnap = 0.02f;

    [Tooltip("分割线最小位置比例")]
    [Range(0f, 1f)]
    public float minRatio = 0.1f;

    [Tooltip("分割线最大位置比例")]
    [Range(0f, 1f)]
    public float maxRatio = 0.9f;

    [Header("强制设置")]
    [Tooltip("强制持续更新相机视口，防止被其他脚本覆盖")]
    public bool forceUpdateViewports = true;

    [Header("摄像机运动系数")]
    [SerializeField] private float CameraMoveCo;

    // 私有变量
    private bool isDragging = false;
    private Vector2 lastMousePosition;
    private Rect lastLeftCameraRect;
    private Rect lastRightCameraRect;

    void Reset()
    {
        divider = GetComponent<RectTransform>();
        if (!canvas) canvas = GetComponentInParent<Canvas>();
    }

    void Awake()
    {
        // 确保组件引用存在
        if (!divider) divider = GetComponent<RectTransform>();
        if (!canvas) canvas = GetComponentInParent<Canvas>();

        // 初始化分割线位置和相机视口
        ApplySplitRatio(splitRatio);
        PositionDividerByRatio(splitRatio);

        // 记录初始设置
        if (leftCamera) lastLeftCameraRect = leftCamera.rect;
        if (rightCamera) lastRightCameraRect = rightCamera.rect;
    }
    Vector3 leStartV3, riStartV3;

    float cameraSize;
    void Start()
    {
        cameraSize = leftCamera.orthographicSize;
        leStartV3 = leftCamera.transform.position - new Vector3 (cameraSize*(1- splitRatio),0,0);
        riStartV3 = rightCamera.transform.position - new Vector3(cameraSize * splitRatio, 0, 0);
        // 确保相机设置正确
        ValidateCameras();
        // 强制应用一次设置
        ApplySplitRatio(splitRatio);
    }

    void Update()
    {
        // 如果启用了强制更新，检查相机视口是否被其他脚本修改
        if (forceUpdateViewports)
        {
            CheckAndRestoreViewports();
        }

        if (Application.isPlaying)
        {
            ApplySplitRatio(splitRatio);
            PositionDividerByRatio(splitRatio);

            // 强制更新一次，确保设置生效
            if (forceUpdateViewports)
            {
                CheckAndRestoreViewports();
            }
        }
    }

    /*
     * Right Camera x @ 0.5f ： -25.5
     * Left Camera x @ 0.5f : -58.8
     * 
     * Right Camera x @ 0f -29.6
     * Right Camera x @ 1f -21.4
     * 
     * Left Camera x @ 0f -62.9
     * Left Camera x @ 1f - 54.7
     */

    /// <summary>
    /// 检查并恢复相机视口设置
    /// </summary>
    void CheckAndRestoreViewports()
    {
        if (leftCamera != null)
        {
            Rect expectedLeftRect = new Rect(0f, 0f, splitRatio, 1f);
            //if (leftCamera.rect != expectedLeftRect)
            {
                leftCamera.transform.position = new Vector3(-63.4f + splitRatio * (8.2f), leStartV3.y, leStartV3.z);
                leftCamera.rect = expectedLeftRect;
                Debug.Log($"SplitViewportController: 恢复左侧相机视口设置 {expectedLeftRect}");
            }
        }

        if (rightCamera != null)
        {
            Rect expectedRightRect = new Rect(splitRatio, 0f, 1f - splitRatio, 1f);
            //if (rightCamera.rect != expectedRightRect)
            {
                // old method
                //rightCamera.transform.position = new Vector3(riStartV3.x + splitRatio * cameraSize * CameraMoveCo, riStartV3.y, riStartV3.z);
                rightCamera.transform.position = new Vector3(-29.1f + splitRatio * (8.2f), riStartV3.y, riStartV3.z);

                rightCamera.rect = expectedRightRect;
                Debug.Log($"SplitViewportController: 恢复右侧相机视口设置 {expectedRightRect}");
            }
        }
    }

    /// <summary>
    /// 开始拖拽时的处理
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        lastMousePosition = eventData.position;

        // 可以在这里添加拖拽开始的特效或音效
        Debug.Log("开始拖拽分割线");
    }

    /// <summary>
    /// 拖拽过程中的处理
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        // 计算鼠标在屏幕上的归一化位置 (0-1)
        float screenX = Mathf.Clamp(eventData.position.x, 0f, Screen.width);
        float normalizedX = Mathf.InverseLerp(0f, Screen.width, screenX);

        // 应用限制范围
        normalizedX = Mathf.Clamp(normalizedX, minRatio, maxRatio);

        // 更新分割比例
        splitRatio = normalizedX;

        // 应用新的分割比例
        ApplySplitRatio(splitRatio);
        PositionDivider(screenX);
    }

    /// <summary>
    /// 结束拖拽时的处理
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        // 边缘吸附逻辑
        if (edgeSnap > 0f)
        {
            if (splitRatio < edgeSnap)
            {
                splitRatio = 0f;
            }
            else if (1f - splitRatio < edgeSnap)
            {
                splitRatio = 1f;
            }

            ApplySplitRatio(splitRatio);
            PositionDividerByRatio(splitRatio);
        }

        Debug.Log($"拖拽结束，分割比例: {splitRatio:F2}");
    }

    /// <summary>
    /// 应用分割比例到相机视口
    /// </summary>
    /// <param name="ratio">分割比例 (0-1)</param>
    void ApplySplitRatio(float ratio)
    {
        // 左侧相机：左边界固定为0，右边界等于分割线位置
        if (leftCamera != null)
        {
            leftCamera.rect = new Rect(0f, 0f, ratio, 1f);
            lastLeftCameraRect = leftCamera.rect;
        }

        // 右侧相机：右边界固定为1，左边界等于分割线位置
        if (rightCamera != null)
        {
            rightCamera.rect = new Rect(ratio, 0f, 1f - ratio, 1f);
            lastRightCameraRect = rightCamera.rect;
        }
    }

    /// <summary>
    /// 根据屏幕坐标定位分割线
    /// </summary>
    /// <param name="screenX">屏幕X坐标</param>
    void PositionDivider(float screenX)
    {
        if (!divider || !canvas) return;

        // 将屏幕坐标转换为Canvas坐标
        RectTransform canvasRect = canvas.transform as RectTransform;
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            new Vector2(screenX, Screen.height * 0.5f),
            uiCamera,
            out localPoint
        );

        // 更新分割线位置，保持Y坐标不变
        divider.anchoredPosition = new Vector2(localPoint.x, divider.anchoredPosition.y);
    }

    /// <summary>
    /// 根据比例定位分割线
    /// </summary>
    /// <param name="ratio">分割比例 (0-1)</param>
    void PositionDividerByRatio(float ratio)
    {
        PositionDivider(ratio * Screen.width);
    }

    /// <summary>
    /// 验证相机设置
    /// </summary>
    void ValidateCameras()
    {
        if (leftCamera == null || rightCamera == null)
        {
            Debug.LogWarning("SplitViewportController: 请确保左右相机都已设置！");
            return;
        }

        // 输出当前相机设置信息
        Debug.Log($"SplitViewportController: 左侧相机 {leftCamera.name} 当前视口: {leftCamera.rect}");
        Debug.Log($"SplitViewportController: 右侧相机 {rightCamera.name} 当前视口: {rightCamera.rect}");
        Debug.Log($"SplitViewportController: 目标分割比例: {splitRatio}");
    }

    /// <summary>
    /// 供外部代码或动画事件调用的设置分割比例方法
    /// </summary>
    /// <param name="ratio">分割比例 (0-1)</param>
    public void SetSplitRatio(float ratio)
    {
        splitRatio = Mathf.Clamp(ratio, minRatio, maxRatio);
        ApplySplitRatio(splitRatio);
        PositionDividerByRatio(splitRatio);

        // 强制更新一次，确保设置生效
        if (forceUpdateViewports)
        {
            CheckAndRestoreViewports();
        }
    }

    /// <summary>
    /// 获取当前分割比例
    /// </summary>
    /// <returns>当前分割比例 (0-1)</returns>
    public float GetSplitRatio()
    {
        return splitRatio;
    }

    /// <summary>
    /// 重置分割线到中间位置
    /// </summary>
    public void ResetToCenter()
    {
        SetSplitRatio(0.5f);
    }

    /// <summary>
    /// 设置分割线到最左侧
    /// </summary>
    public void SetToLeft()
    {
        SetSplitRatio(0f);
    }

    /// <summary>
    /// 设置分割线到最右侧
    /// </summary>
    public void SetToRight()
    {
        SetSplitRatio(1f);
    }

    /// <summary>
    /// 强制重置相机视口设置
    /// </summary>
    public void ForceResetViewports()
    {
        Debug.Log("SplitViewportController: 强制重置相机视口设置");
        ApplySplitRatio(splitRatio);
        CheckAndRestoreViewports();
    }

    /// <summary>
    /// 输出当前相机视口状态（用于调试）
    /// </summary>
    public void LogViewportStatus()
    {
        if (leftCamera != null)
        {
            Debug.Log($"左侧相机 {leftCamera.name}: {leftCamera.rect}");
        }
        if (rightCamera != null)
        {
            Debug.Log($"右侧相机 {rightCamera.name}: {rightCamera.rect}");
        }
        Debug.Log($"当前分割比例: {splitRatio}");
    }

}
