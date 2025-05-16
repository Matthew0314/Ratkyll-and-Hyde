using UnityEngine;
using UnityEngine.UI;

public class SingleCanvasSplitScreenPointer : MonoBehaviour
{
    [System.Serializable]
    public class PlayerPointerSetup
    {
        public Camera playerCamera;              // This player's camera
        public RectTransform pointerRectTransform; // UI arrow for this player
        public RectTransform viewportRect;       // RectTransform defining this player's viewport area
    }

    public Transform targetObject;          // Object to track
    public PlayerPointerSetup[] playerSetups; // Array of player setups
    public float edgeBuffer = 20f;          // Buffer from viewport edges
    
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
        
        // Make sure each pointer has a CanvasGroup
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
        
        // Update pointer for each player
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
        
        // Convert target position to viewport coordinates for this camera
        Vector3 targetViewportPosition = camera.WorldToViewportPoint(targetObject.position);
        
        // Check if target is visible in this camera's view
        bool isTargetVisible = targetViewportPosition.z > 0 && 
                              targetViewportPosition.x > 0 && targetViewportPosition.x < 1 && 
                              targetViewportPosition.y > 0 && targetViewportPosition.y < 1;
        
        // Show/hide pointer based on visibility
        canvasGroup.alpha = isTargetVisible ? 0 : 1;
        
        if (!isTargetVisible)
        {
            // Convert world position to screen position for this camera
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, targetObject.position);
            
            // Convert screen position to canvas position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viewportRect,
                screenPoint,
                canvas.worldCamera,
                out Vector2 localPoint);
                
            // If object is behind camera, invert the direction
            if (targetViewportPosition.z < 0)
            {
                localPoint = -localPoint;
            }
            
            // Get viewport rect dimensions
            Rect viewportRectDimensions = viewportRect.rect;
            float halfWidth = viewportRectDimensions.width * 0.5f - edgeBuffer;
            float halfHeight = viewportRectDimensions.height * 0.5f - edgeBuffer;
            
            // Calculate direction from viewport center to target
            Vector2 direction = localPoint.normalized;
            
            // Calculate angle for pointer rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pointerRect.rotation = Quaternion.Euler(0, 0, angle);
            
            // Calculate intersection with viewport edges
            Vector2 cappedPosition;
            if (Mathf.Abs(direction.x) / Mathf.Abs(direction.y) > halfWidth / halfHeight)
            {
                // Intersection with left/right edge
                cappedPosition = new Vector2(
                    Mathf.Sign(direction.x) * halfWidth,
                    direction.y / Mathf.Abs(direction.x) * Mathf.Sign(direction.x) * halfWidth
                );
            }
            else
            {
                // Intersection with top/bottom edge
                cappedPosition = new Vector2(
                    direction.x / Mathf.Abs(direction.y) * Mathf.Sign(direction.y) * halfHeight,
                    Mathf.Sign(direction.y) * halfHeight
                );
            }
            
            // Convert capped position to be relative to viewport rect
            pointerRect.position = viewportRect.TransformPoint(cappedPosition);
        }
    }
}