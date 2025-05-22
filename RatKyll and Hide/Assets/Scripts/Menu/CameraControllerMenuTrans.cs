
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
    [SerializeField] private float imageFadeOutSpeed = 2.0f;
    [SerializeField] private bool useUnifiedFadeSpeed = true;
    [SerializeField] private float textFadeOutSpeed = 2.0f;
    
    [Header("Input Settings")]
    [SerializeField] private bool stillUseYKey = true;
    [SerializeField] private float inputDebounceTime = 0.5f;
    
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
        SetupInputSystem();
    }

    void Start()
    {
        if (startingCamera != null && targetCamera != null)
        {
            startingCamera.Priority = 20;
            targetCamera.Priority = 10;
        }

        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

        if (cinemachineBrain == null)
        {
            Debug.LogError("No CinemachineBrain found on Main Camera!");
        }

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

        var inputActionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
        var actionMap = new InputActionMap("UI");
        
        anyButtonAction = actionMap.AddAction("AnyButton", binding: "*/<Button>", interactions: "Press");
        
        inputActionAsset.AddActionMap(actionMap);
        playerInput.actions = inputActionAsset;
        
        anyButtonAction.Enable();
        anyButtonAction.performed += OnAnyButtonPressed;
    }
    
    private void OnAnyButtonPressed(InputAction.CallbackContext context)
    {
        if (!isTransitioning && Time.time - lastInputTime > inputDebounceTime)
        {
            lastInputTime = Time.time;
            
            if (objectRevealed)
            {
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
        
        startingCamera.Priority = 10;
        targetCamera.Priority = 20;
    }
    
    private void HandleTransition()
    {
        transitionTimer += Time.deltaTime;
        float normalizedTime = transitionTimer / transitionDuration;
        
        float curveValue = transitionCurve.Evaluate(normalizedTime);
        
        if (transitionImage != null)
        {
            float fadeAlpha = Mathf.Lerp(originalImageColor.a, 0f, normalizedTime * imageFadeOutSpeed);
            
            Color newColor = originalImageColor;
            newColor.a = fadeAlpha;
            transitionImage.color = newColor;
        }
        
        if (transitionText != null)
        {
            float fadeSpeed = useUnifiedFadeSpeed ? imageFadeOutSpeed : textFadeOutSpeed;
            
            float fadeAlpha = Mathf.Lerp(originalTextColor.a, 0f, normalizedTime * fadeSpeed);
            
            Color newColor = originalTextColor;
            newColor.a = fadeAlpha;
            transitionText.color = newColor;
        }
        
        if (normalizedTime >= 1f)
        {
            isTransitioning = false;
            
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
        if (anyButtonAction != null)
        {
            anyButtonAction.performed -= OnAnyButtonPressed;
            anyButtonAction.Disable();
        }
    }
}
