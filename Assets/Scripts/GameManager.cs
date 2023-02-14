using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    private bool _gameOver;
    // Start is called before the first frame update
    void Start()
    {
        _gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if player presses escape key, quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        if (Input.GetKeyDown(KeyCode.R) && _gameOver == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    public void GameOver()
    {
        _gameOver = true;
    }
}
