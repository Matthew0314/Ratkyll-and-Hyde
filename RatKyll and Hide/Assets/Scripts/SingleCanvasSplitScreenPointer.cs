using UnityEngine;
using UnityEngine.UI;

public class SingleCanvasSplitScreenPointer : MonoBehaviour
{
    [System.Serializable]
    public class PlayerPointerSetup
    {
        public Camera playerCamera;              
        public RectTransform pointerRectTransform;
        public RectTransform viewportRect; 
    }

    public Transform targetObject;          
    public PlayerPointerSetup[] playerSetups; 
    public float edgeBuffer = 20f;          
    
    private Canvas canvas;
    
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Pointer must be child of a Canvas!");
            enabled = false;
            return;
        }
        
        foreach (var setup in playerSetups)
        {
            if (setup.pointerRectTransform.GetComponent<CanvasGroup>() == null)
            {
                setup.pointerRectTransform.gameObject.AddComponent<CanvasGroup>();
            }
        }
    }
    
    void Update()
    {
        if (targetObject == null) return;
        
        foreach (var setup in playerSetups)
        {
            UpdatePointerForPlayer(setup);
        }
    }
    
    void UpdatePointerForPlayer(PlayerPointerSetup setup)
    {
        Camera camera = setup.playerCamera;
        RectTransform pointerRect = setup.pointerRectTransform;
        RectTransform viewportRect = setup.viewportRect;
        CanvasGroup canvasGroup = pointerRect.GetComponent<CanvasGroup>();
        
        Vector3 targetViewportPosition = camera.WorldToViewportPoint(targetObject.position);
        
        bool isTargetVisible = targetViewportPosition.z > 0 && 
                              targetViewportPosition.x > 0 && targetViewportPosition.x < 1 && 
                              targetViewportPosition.y > 0 && targetViewportPosition.y < 1;
        
        canvasGroup.alpha = isTargetVisible ? 0 : 1;
        
        if (!isTargetVisible)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, targetObject.position);
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viewportRect,
                screenPoint,
                canvas.worldCamera,
                out Vector2 localPoint);
                
            if (targetViewportPosition.z < 0)
            {
                localPoint = -localPoint;
            }
            
            Rect viewportRectDimensions = viewportRect.rect;
            float halfWidth = viewportRectDimensions.width * 0.5f - edgeBuffer;
            float halfHeight = viewportRectDimensions.height * 0.5f - edgeBuffer;
            
            Vector2 direction = localPoint.normalized;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pointerRect.rotation = Quaternion.Euler(0, 0, angle);
            
            Vector2 cappedPosition;
            if (Mathf.Abs(direction.x) / Mathf.Abs(direction.y) > halfWidth / halfHeight)
            {
                cappedPosition = new Vector2(
                    Mathf.Sign(direction.x) * halfWidth,
                    direction.y / Mathf.Abs(direction.x) * Mathf.Sign(direction.x) * halfWidth
                );
            }
            else
            {
                cappedPosition = new Vector2(
                    direction.x / Mathf.Abs(direction.y) * Mathf.Sign(direction.y) * halfHeight,
                    Mathf.Sign(direction.y) * halfHeight
                );
            }
            
            pointerRect.position = viewportRect.TransformPoint(cappedPosition);
        }
    }
}
