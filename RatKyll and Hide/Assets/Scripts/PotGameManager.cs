using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PotGameManager : MonoBehaviour, IGameManager
{
    private int player1Score = 0;
    private int player2Score = 0;
    private bool gameOver;
    private float gameDuration = 300f; // Five minutes
    private float timer;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI player1ScoreText;
    [SerializeField] TextMeshProUGUI player2ScoreText;
    
    // Game over screen elements
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI winnerText;
    [SerializeField] TextMeshProUGUI finalScoresText;
    [SerializeField] Button restartButton;
    [SerializeField] Button mainMenuButton;
    
    // Animation timing
    [SerializeField] float scoresFadeInTime = 1.0f;
    [SerializeField] float delayBeforeWinnerReveal = 2.0f;
    [SerializeField] float winnerFadeInTime = 1.0f;
    [SerializeField] float delayBeforeButtonsReveal = 2.0f;
    [SerializeField] float buttonsFadeInTime = 1.0f;

    void Start() {
        timer = gameDuration;
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
        
        // Make sure game over panel is initially hidden
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
            
        // Add button listeners
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
            
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    void Update()
    {
        if (!gameOver) {
            UpdateTimer();
        }
    }

    private void UpdateTimer() {
        timer -= Time.deltaTime;

        if (timer <= 0) {
            timer = 0;
            gameOver = true;
            StartGameOverSequence();
        }

        timerText.text = Mathf.CeilToInt(timer).ToString();
    }

    public void IncScore(PlayerController player, IPickUpItem item) {
        if (player.GetPlayerNum() == 0) player1Score += item.Points;
        else player2Score += item.Points;

        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
        Debug.Log("Player1 " + player1Score + " Player2 " + player2Score);
    }
    
    private void StartGameOverSequence() {
        // Pause the game
        Time.timeScale = 0;
        
        // Disable player controls
        DisablePlayerControls();
        
        // Activate the game over panel but make elements invisible
        if (gameOverPanel != null) {
            gameOverPanel.SetActive(true);
            
            // Set up text content
            if (finalScoresText != null) {
                finalScoresText.text = string.Format("Final Scores\nPlayer 1: {0}\nPlayer 2: {1}", 
                                                    player1Score, player2Score);
                finalScoresText.alpha = 0; // Start invisible
            }
            
            if (winnerText != null) {
                if (player1Score > player2Score) {
                    winnerText.text = "Player 1 Wins!";
                } else if (player2Score > player1Score) {
                    winnerText.text = "Player 2 Wins!";
                } else {
                    winnerText.text = "It's a Tie!";
                }
                winnerText.alpha = 0; // Start invisible
            }
            
            // Set buttons invisible
            if (restartButton != null) {
                CanvasGroup restartBtnGroup = restartButton.GetComponent<CanvasGroup>();
                if (restartBtnGroup == null) {
                    restartBtnGroup = restartButton.gameObject.AddComponent<CanvasGroup>();
                }
                restartBtnGroup.alpha = 0;
                restartBtnGroup.interactable = false;
            }
            
            if (mainMenuButton != null) {
                CanvasGroup menuBtnGroup = mainMenuButton.GetComponent<CanvasGroup>();
                if (menuBtnGroup == null) {
                    menuBtnGroup = mainMenuButton.gameObject.AddComponent<CanvasGroup>();
                }
                menuBtnGroup.alpha = 0;
                menuBtnGroup.interactable = false;
            }
            
            // Start the animation sequence
            StartCoroutine(AnimateGameOverSequence());
        }
    }
    
    private IEnumerator AnimateGameOverSequence() {
        // Step 1: Fade in the scores
        float elapsedTime = 0;
        while (elapsedTime < scoresFadeInTime) {
            float normalizedTime = elapsedTime / scoresFadeInTime;
            finalScoresText.alpha = Mathf.Lerp(0, 1, normalizedTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        finalScoresText.alpha = 1; // Ensure it's fully visible
        
        // Step 2: Wait before showing the winner
        yield return new WaitForSecondsRealtime(delayBeforeWinnerReveal);
        
        // Step 3: Fade in the winner text
        elapsedTime = 0;
        while (elapsedTime < winnerFadeInTime) {
            float normalizedTime = elapsedTime / winnerFadeInTime;
            winnerText.alpha = Mathf.Lerp(0, 1, normalizedTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        winnerText.alpha = 1; // Ensure it's fully visible
        
        // Step 4: Wait before showing the buttons
        yield return new WaitForSecondsRealtime(delayBeforeButtonsReveal);
        
        // Step 5: Fade in buttons
        elapsedTime = 0;
        CanvasGroup restartBtnGroup = restartButton.GetComponent<CanvasGroup>();
        CanvasGroup menuBtnGroup = mainMenuButton.GetComponent<CanvasGroup>();
        
        while (elapsedTime < buttonsFadeInTime) {
            float normalizedTime = elapsedTime / buttonsFadeInTime;
            if (restartBtnGroup != null) restartBtnGroup.alpha = Mathf.Lerp(0, 1, normalizedTime);
            if (menuBtnGroup != null) menuBtnGroup.alpha = Mathf.Lerp(0, 1, normalizedTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        
        // Ensure buttons are fully visible and interactable
        if (restartBtnGroup != null) {
            restartBtnGroup.alpha = 1;
            restartBtnGroup.interactable = true;
        }
        if (menuBtnGroup != null) {
            menuBtnGroup.alpha = 1;
            menuBtnGroup.interactable = true;
        }

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
    }
    
    private void DisablePlayerControls() {
        // Find all player controllers and disable them
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players) {
            player.enabled = false;
        }
    }
    
    public void RestartGame() {
        // Reset time scale in case it was changed
        Time.timeScale = 1;
        
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    
    public void GoToMainMenu() {
        // Reset time scale in case it was changed
        Time.timeScale = 1;
        
        // Load the main menu scene
        // Replace "MainMenu" with your actual main menu scene name
        SceneManager.LoadScene("mainMenu");
    }

    public bool GameOver => gameOver;
}
