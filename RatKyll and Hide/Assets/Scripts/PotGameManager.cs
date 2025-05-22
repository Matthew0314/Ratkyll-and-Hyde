using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PotGameManager : MonoBehaviour, IGameManager
{
    private int player1Score = 0;
    private int player2Score = 0;
    private bool gameOver = true;
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

    public TextMeshProUGUI countdownText;
    public float expandDuration = 0.5f;
    public float shrinkDuration = 0.2f;
    [SerializeField] SingleCanvasSplitScreenPointer singleCanvasSplitScreenPointer;
    [SerializeField] GameObject arrow1;
    [SerializeField] GameObject arrow2;

    public Image player1Bar;
    public Image player2Bar;
    public TextMeshProUGUI resultText;
    private int totalPoints;
    public GameObject gameOverBox;

    public float fillDuration = 1.5f;
    void Start() {
        timer = gameDuration;
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
        
        // Makes sure game over panel is initially hidden
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
            
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
            // ShowGameOver();
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
        Time.timeScale = 0;
        
        DisablePlayerControls();
        
        if (gameOverPanel != null) {
            gameOverPanel.SetActive(true);
            
            if (finalScoresText != null) {
                finalScoresText.text = string.Format("Final Scores\nPlayer 1: {0}\nPlayer 2: {1}", player1Score, player2Score);
                finalScoresText.alpha = 0;
            }
            
            if (winnerText != null) {
                if (player1Score > player2Score) {
                    winnerText.text = "Player 1 Wins!";
                } else if (player2Score > player1Score) {
                    winnerText.text = "Player 2 Wins!";
                } else {
                    winnerText.text = "It's a Tie!";
                }
                winnerText.alpha = 0;
            }
            
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
        finalScoresText.alpha = 1; 
        
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
        winnerText.alpha = 1; 
        
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
        SceneManager.LoadScene("mainMenu");
    }

    public void StartGame() {
        StartCoroutine(CountdownSequence());
    }

    private IEnumerator CountdownSequence()
    {
        string[] countdownValues = { "3", "2", "1", "Begin!" };
        countdownText.gameObject.SetActive(true);

        foreach (string value in countdownValues)
        {
            countdownText.text = value;
            countdownText.transform.localScale = Vector3.zero;

            
            float t = 0f;
            while (t < expandDuration)
            {
                t += Time.deltaTime;
                float scale = Mathf.Lerp(0f, 1f, t / expandDuration);
                countdownText.transform.localScale = Vector3.one * scale;
                yield return null;
            }

            
            yield return new WaitForSeconds(0.3f);

            
            t = 0f;
            while (t < shrinkDuration)
            {
                t += Time.deltaTime;
                float scale = Mathf.Lerp(1f, 0f, t / shrinkDuration);
                countdownText.transform.localScale = Vector3.one * scale;
                yield return null;
            }

            yield return null;
        }

        
        countdownText.text = "";
        arrow1.gameObject.SetActive(true);
        arrow2.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        player1ScoreText.gameObject.SetActive(true);
        player2ScoreText.gameObject.SetActive(true);
        singleCanvasSplitScreenPointer.enabled = true;
        gameOver = false;
    }

    public bool GameOver => gameOver;

    public void ShowGameOver()
    {
        gameOverBox.SetActive(true);

        
        totalPoints = player1Score + player2Score;

        
        player1Bar.rectTransform.sizeDelta = new Vector2(0, player1Bar.rectTransform.sizeDelta.y);
        player2Bar.rectTransform.sizeDelta = new Vector2(0, player2Bar.rectTransform.sizeDelta.y);

        resultText.text = "";

        StartCoroutine(FillBarsCoroutine());
    }

    private IEnumerator FillBarsCoroutine()
    {
        RectTransform barContainer = player1Bar.transform.parent.GetComponent<RectTransform>();
        float barWidth = barContainer.rect.width;

        float target1 = (player1Score / totalPoints) * barWidth;
        float target2 = (player2Score / totalPoints) * barWidth;

        float elapsed = 0f;

        while (elapsed < fillDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fillDuration);

            float current1 = Mathf.Lerp(0, target1, t);
            float current2 = Mathf.Lerp(0, target2, t);

            player1Bar.rectTransform.sizeDelta = new Vector2(current1, player1Bar.rectTransform.sizeDelta.y);
            player2Bar.rectTransform.sizeDelta = new Vector2(current2, player2Bar.rectTransform.sizeDelta.y);

            yield return null;
        }

        
        player1Bar.rectTransform.sizeDelta = new Vector2(target1, player1Bar.rectTransform.sizeDelta.y);
        player2Bar.rectTransform.sizeDelta = new Vector2(target2, player2Bar.rectTransform.sizeDelta.y);

        ShowResult();
    }

    private void ShowResult()
    {
        if (player1Score > player2Score)
            resultText.text = "Player 1 Wins!";
        else if (player2Score > player1Score)
            resultText.text = "Player 2 Wins!";
        else
            resultText.text = "It's a Tie!";
    }
}
