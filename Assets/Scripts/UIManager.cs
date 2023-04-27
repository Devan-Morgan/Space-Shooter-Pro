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
    [SerializeField]
    private Text _thrusterEnergyText;
   // [SerializeField]
    private SatellitePowerup _satellite;
    [SerializeField] 
    private Text _waveCounter;
    [SerializeField]
    private Image _bossHealthBar;
    [SerializeField]
    private GameObject _bossHealthBarVisualizer;

    private float _health;
    //  private bool _gameOver;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.SetActive(false);
        _restartText.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _satellite = GameObject.Find("Satellite_Wings").GetComponent<SatellitePowerup>();
        _waveCounter.text = "Wave 1";
        _waveCounter.gameObject.SetActive(false);
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
        _ammoText.text = "Ammo: " + _ammo.ToString() + "/30";
    }
    
    public void UpdateThrusterEnergy (int _thrusterEnergy)
    {
        _thrusterEnergyText.text = "Thruster Energy: " + _thrusterEnergy.ToString();
    }
    
    public void GameOver()
    {
       // _GameOverText.SetActive(true);
        
        //make game over text turn on and off
        _gameManager.GameOver();
        StartCoroutine(GameOverFlickerRoutine());
    }

    public void WaveCounter(int _wave)
    {
        _waveCounter.text = "Wave " + _wave.ToString();
        StartCoroutine(WaveFLickerRoutine());
    }
    
    IEnumerator WaveFLickerRoutine()
    {
        _waveCounter.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        _waveCounter.gameObject.SetActive(false);
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

    public void HealthManager(float _health)
    {
        _bossHealthBar.fillAmount = _health;
    }

    public void BossHealthBarActive()
    {
        _bossHealthBarVisualizer.gameObject.SetActive(true);
    }

}
