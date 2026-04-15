using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class AspectRatioLetterbox : MonoBehaviour
{
    public float targetWidth = 9f;
    public float targetHeight = 16f;

    private Camera attachedCamera;

    void Awake()
    {
        attachedCamera = GetComponent<Camera>();
        ApplyViewport();
    }

    void OnEnable()
    {
        ApplyViewport();
    }

    void OnValidate()
    {
        ApplyViewport();
    }

    void Update()
    {
        ApplyViewport();
    }

    void ApplyViewport()
    {
        if (attachedCamera == null)
        {
            attachedCamera = GetComponent<Camera>();
        }

        if (attachedCamera == null || targetWidth <= 0f || targetHeight <= 0f)
        {
            return;
        }

        float targetAspect = targetWidth / targetHeight;
        float windowAspect = (float)Screen.width / Mathf.Max(1, Screen.height);
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1f)
        {
            Rect rect = attachedCamera.rect;
            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0f;
            rect.y = (1f - scaleHeight) * 0.5f;
            attachedCamera.rect = rect;
        }
        else
        {
            float scaleWidth = 1f / scaleHeight;
            Rect rect = attachedCamera.rect;
            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) * 0.5f;
            rect.y = 0f;
            attachedCamera.rect = rect;
        }
    }
}
