
//This file is no longer in use, it should be moved to the archive
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameplaySelectManager : MonoBehaviour
{
    
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
    
    
    public void LoadLevelOne()
    {
        Debug.Log("Loading Level One...");
        GameObject bgMusic = GameObject.FindGameObjectWithTag("MusicPlayer");
        Destroy(bgMusic);
        SceneManager.LoadScene("LevelOne");
    }

}
