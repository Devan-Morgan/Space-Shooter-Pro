using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    //public or private reference
    //data type (int, floats, bool, strings)
    //every variable has a name
    //optionally, value assigned
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _speedMultiplier = 2f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    //cooldown between shots
    [SerializeField]
    private float _fireRate = 0.5f;
    //next time we can fire
    private float _canFire = -1f;
    //create player lives
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    //variable for is TripleShot active
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private float TripleShotCooldown = 4.0f;
    [SerializeField]
    private float SpeedBoostCooldown = 4.0f;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField] 
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _explosionPrefab;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        //take the current position of the player and assign it a start position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
        
        
    }
    
    // Update is called once per frame
    void Update()
    {
      CalculateMovement();

      FireLaser();


    }

    void CalculateMovement()
    {
        //get input 
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        // moves player up, down, left and right
        if(_isSpeedBoostActive == true)
        {
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);
        }
        else
        {
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        //Player y axis boundary(-3.8, 0)
        
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }
         
        //player x axis boundary with wrap on positive end (-11.3, 11.3)
        
        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        //if space key is pressed
        //spawn a laser above the player
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            _audioSource.clip = _laserSoundClip;
            
            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
            }
            
            //play laser sound using audio source component
            _audioSource.Play();



        }
    }
    
    public void Damage()
    {
        //if shield active
        //take no damage
        //deactivate shield
        
        if(_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        else
        
        _lives--;
        
        _uiManager.UpdateLives(_lives);
        
        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }
        
        if (_lives < 1)
        {
            //communicate with SpawnManager to tell it to stop spawning
            //SpawnManager spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
            //if spawnManager is not null, call on PlayerDeath
            if (_spawnManager != null)
            {
                _spawnManager.OnPlayerDeath();
            }
            
            Instantiate(_explosionPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    
    public void TripleShotActive()
    {
        //tripleshotActive becomes true
        _isTripleShotActive = true;
        
        //start a coroutine to turn off tripleshot after 5 seconds
        StartCoroutine(TripleShotPowerDownRoutine());


    }
    
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(TripleShotCooldown);
        _isTripleShotActive = false;
    }
    
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(SpeedBoostCooldown);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
    
    public void ShieldActive()
    {
        _isShieldActive = true;
        //spawn shield visualizer on player and make it move with player
        _shieldVisualizer.SetActive(true);
    }
    
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }

}
