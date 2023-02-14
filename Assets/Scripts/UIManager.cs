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
    private int _score;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private GameObject _gameOverText;
    [SerializeField]
    private GameObject _restartText;
    private GameManager _gameManager;
    [SerializeField]
    private Text _ammoText;
    //  private bool _gameOver;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.SetActive(false);
        _restartText.SetActive(false);
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

    public void UpdateScore(int points)
    {
        _score += points;
        _scoreText.text = "Score: " + points.ToString();
    }
    
    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];
        if (currentLives == 0)
        {
           // _GameOverText.SetActive(true);
           GameOver();
           _restartText.SetActive(true);
        }
    }
    
    public void UpdateAmmo(int _ammo)
    {
        _ammoText.text = "Ammo: " + _ammo.ToString();
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
            _gameOverText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
    
}
