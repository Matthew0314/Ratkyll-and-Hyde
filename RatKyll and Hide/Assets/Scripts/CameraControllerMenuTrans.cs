using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class CameraControllerMenuTrans : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineVirtualCamera startingCamera;
    [SerializeField] private CinemachineVirtualCamera targetCamera;
    
    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 2.0f;
    [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private float transitionTimer = 0f;
    private bool isTransitioning = false;
    private CinemachineBrain cinemachineBrain;
    
    void Start()
    {
        // Ensure starting camera has higher priority initially
        if (startingCamera != null && targetCamera != null)
        {
            startingCamera.Priority = 20;
            targetCamera.Priority = 10;
        }
        
        // Get reference to the CinemachineBrain component
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        
        if (cinemachineBrain == null)
        {
            Debug.LogError("No CinemachineBrain found on Main Camera!");
        }
    }
    
    void Update()
    {
        // Check for 'Y' key press
        if (Input.GetKeyDown(KeyCode.Y) && !isTransitioning)
        {
            StartTransition();
        }
        
        // Handle transition if in progress
        if (isTransitioning)
        {
            HandleTransition();
        }
    }
    
    private void StartTransition()
    {
        if (startingCamera == null || targetCamera == null)
        {
            Debug.LogError("Camera references not set!");
            return;
        }
        
        isTransitioning = true;
        transitionTimer = 0f;
        
        // Swap priorities to trigger the transition
        startingCamera.Priority = 10;
        targetCamera.Priority = 20;
    }
    
    private void HandleTransition()
    {
        transitionTimer += Time.deltaTime;
        float normalizedTime = transitionTimer / transitionDuration;
        
        // Apply easing curve
        float curveValue = transitionCurve.Evaluate(normalizedTime);
        
        // You can add additional effects during transition here
        // For example, modify FOV, dutch angle, or noise settings
        
        if (normalizedTime >= 1f)
        {
            isTransitioning = false;
            SceneManager.LoadScene("gameplaySelect");
        }
    }
}