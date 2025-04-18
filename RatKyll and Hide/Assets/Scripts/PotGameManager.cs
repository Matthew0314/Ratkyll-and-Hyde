using UnityEngine;
<<<<<<< HEAD

public class PotGameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
=======
using TMPro;

// Implement a timer, possibly later on we need a countdown and maybe a scene overview like in Mario Kart
public class PotGameManager : MonoBehaviour, IGameManager
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int player1Score = 0;
    private int player2Score = 0;
    private bool gameOver;
    private float gameDuration = 300f; // Five minutes
    private float timer;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI player1ScoreText;
    [SerializeField] TextMeshProUGUI player2ScoreText;



    void Start() {
        timer = gameDuration;
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
>>>>>>> b6cc0bf6b576ae9c4114342cda1821025f478e88
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        
    }
=======
        UpdateTimer();
    }

    private void UpdateTimer() {

        timer -= Time.deltaTime;

        if (timer <= 0) {
            gameOver = true;
            Debug.Log("Game Over!");
        }

        timerText.text = Mathf.CeilToInt(timer).ToString();
    }

    // Called in CookingPot and used to increment the players score
    public void IncScore(PlayerController player, IPickUpItem item) {
        if (player.GetPlayerNum() == 0) player1Score += item.Points;
        else player2Score += item.Points;

        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
        Debug.Log("Player1 " + player1Score + " Player2 " + player2Score);
    }

    public bool GameOver => gameOver;


>>>>>>> b6cc0bf6b576ae9c4114342cda1821025f478e88
}
