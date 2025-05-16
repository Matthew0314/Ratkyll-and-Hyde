
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;

public class CameraControllerMenuTrans : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineVirtualCamera startingCamera;
    [SerializeField] private CinemachineVirtualCamera targetCamera;
    
    [Header("UI Elements")]
    [SerializeField] private Image transitionImage;
    [SerializeField] private TMP_Text transitionText;
    
    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 2.0f;
    [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float imageFadeOutSpeed = 2.0f; // How quickly the UI elements fade out relative to the transition
    [SerializeField] private bool useUnifiedFadeSpeed = true; // If true, both image and text use the same fade speed
    [SerializeField] private float textFadeOutSpeed = 2.0f; // Only used if useUnifiedFadeSpeed is false
    
    [Header("Input Settings")]
    [SerializeField] private bool stillUseYKey = true; // Option to still use Y key as before
    [SerializeField] private float inputDebounceTime = 0.5f; // Prevents multiple inputs from triggering in quick succession
    
    private float transitionTimer = 0f;
    private bool isTransitioning = false;
    private CinemachineBrain cinemachineBrain;
    private Color originalImageColor;
    private Color originalTextColor;
    private float lastInputTime = 0f;
    
    // Input System variables
    private PlayerInput playerInput;
    private InputAction anyButtonAction;

    [SerializeField]  Animator animator; 
    [SerializeField] RuntimeAnimatorController controllerToAssign; 
    [SerializeField] private GameObject objectToReveal;
    private bool objectRevealed = false;
[SerializeField] private bool revealObjectAfterTransition = true;

    void Awake()
    {
        // Set up Input System
        SetupInputSystem();
    }

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

        // Store original colors to preserve RGB values while only changing alpha
        if (transitionImage != null)
        {
            originalImageColor = transitionImage.color;
        }
        else
        {
            Debug.LogWarning("No transition image assigned. Image fade effect will be skipped.");
        }
        
        if (transitionText != null)
        {
            originalTextColor = transitionText.color;
        }
        else
        {
            Debug.LogWarning("No transition text assigned. Text fade effect will be skipped.");
        }
    }
    
    private void SetupInputSystem()
    {
        // Method 1: Using PlayerInput component
        playerInput = gameObject.AddComponent<PlayerInput>();
        playerInput.defaultActionMap = "UI";

        // Create a simple input action asset at runtime for any button press
        var inputActionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
        var actionMap = new InputActionMap("UI");
        
        // Create an action that responds to any button press from any device
        anyButtonAction = actionMap.AddAction("AnyButton", binding: "*/<Button>", interactions: "Press");
        
        inputActionAsset.AddActionMap(actionMap);
        playerInput.actions = inputActionAsset;
        
        // Enable the action and add callback
        anyButtonAction.Enable();
        anyButtonAction.performed += OnAnyButtonPressed;
    }
    
    private void OnAnyButtonPressed(InputAction.CallbackContext context)
    {
        // Check if we're already transitioning or if we need to respect debounce time
        if (!isTransitioning && Time.time - lastInputTime > inputDebounceTime)
        {
            lastInputTime = Time.time;
            
            if (objectRevealed)
            {
                // If object has been revealed, load the level
                SceneManager.LoadScene("LevelOne");
            }
            else
            {
                Debug.LogError("AHHHHHHH");
                StartTransition();
            }
        }
    }
    
    void Update()
    {
        // Legacy input system option - still check for 'Y' key if enabled
        if (stillUseYKey && Input.GetKeyDown(KeyCode.Y) && !isTransitioning)
        {
            if (objectRevealed)
            {
                SceneManager.LoadScene("LevelOne");
            }
            else
            {
                StartTransition();
            }
        }
        
        // Also check for any mouse click or touch input using legacy input system
        if (!isTransitioning && Time.time - lastInputTime > inputDebounceTime)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) ||
                Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began)
            {
                lastInputTime = Time.time;
                if (objectRevealed)
                {
                    SceneManager.LoadScene("LevelOne");
                }
                else
                {
                    StartTransition();
                }
            }
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

            animator.runtimeAnimatorController = controllerToAssign;

        
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
        
        // Fade out image
        if (transitionImage != null)
        {
            // Calculate fade value with fade speed modifier
            float fadeAlpha = Mathf.Lerp(originalImageColor.a, 0f, normalizedTime * imageFadeOutSpeed);
            
            // Update the image color, preserving RGB but changing alpha
            Color newColor = originalImageColor;
            newColor.a = fadeAlpha;
            transitionImage.color = newColor;
        }
        
        // Fade out text
        if (transitionText != null)
        {
            // Use either unified fade speed or text-specific fade speed
            float fadeSpeed = useUnifiedFadeSpeed ? imageFadeOutSpeed : textFadeOutSpeed;
            
            // Calculate fade value with fade speed modifier
            float fadeAlpha = Mathf.Lerp(originalTextColor.a, 0f, normalizedTime * fadeSpeed);
            
            // Update the text color, preserving RGB but changing alpha
            Color newColor = originalTextColor;
            newColor.a = fadeAlpha;
            transitionText.color = newColor;
        }
        
        if (normalizedTime >= 1f)
        {
            isTransitioning = false;
            
            // Ensure UI elements are completely invisible before loading new scene
            if (transitionImage != null)
            {
                Color finalColor = originalImageColor;
                finalColor.a = 0f;
                transitionImage.color = finalColor;
            }
            
            if (transitionText != null)
            {
                Color finalColor = originalTextColor;
                finalColor.a = 0f;
                transitionText.color = finalColor;
            }

            if (objectToReveal != null && revealObjectAfterTransition)
            {
                objectToReveal.SetActive(true);
                objectRevealed = true; 
            }
        }
    }
    
    private void OnDestroy()
    {
        // Clean up input system event subscriptions
        if (anyButtonAction != null)
        {
            anyButtonAction.performed -= OnAnyButtonPressed;
            anyButtonAction.Disable();
        }
    }
}