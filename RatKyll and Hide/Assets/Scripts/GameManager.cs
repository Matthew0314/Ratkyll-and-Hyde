using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager {
    private bool _gameOver = false;

    public bool GameOver {
        get { return _gameOver; }
        private set { _gameOver = value; }
    }
}
