using UnityEngine;
using UnityEngine.SceneManagement; // Added for scene management functionality

public class GameplaySelectManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P key was pressed");
            LoadLevelOne();
        }
    }
    
    // Function to load LevelOne scene when button is pressed
    public void LoadLevelOne()
    {
        Debug.Log("Loading Level One...");
        GameObject bgMusic = GameObject.FindGameObjectWithTag("MusicPlayer");
        Destroy(bgMusic);
        SceneManager.LoadScene("LevelOne");
    }

}