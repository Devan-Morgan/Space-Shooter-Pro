using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private int Score;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private GameObject _GameOverText;
    [SerializeField]
    private GameObject _RestartText;
    private GameManager _gameManager;
  //  private bool _gameOver;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _GameOverText.SetActive(false);
        _RestartText.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
       // _gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.R) && _gameOver == true)
        {
            SceneManager.LoadScene(0);
        }*/

    }

    public void UpdateScore(int Points)
    {
        Score += Points;
        _scoreText.text = "Score: " + Points.ToString();
    }
    
    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _livesSprites[currentLives];
        if (currentLives == 0)
        {
           // _GameOverText.SetActive(true);
           GameOver();
           _RestartText.SetActive(true);
        }
    }
    
    public void GameOver()
    {
       // _GameOverText.SetActive(true);
        
        //make game over text turn on and off
        _gameManager.GameOver();
        StartCoroutine(GameOverFlickerRoutine());
    }
    
    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _GameOverText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _GameOverText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
    
}
