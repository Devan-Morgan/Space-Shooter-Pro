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
    private float _speedSprintMulitplier = 1.5f;
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
    private float _tripleShotCooldown = 4.0f;
    [SerializeField]
    private float _speedBoostCooldown = 4.0f;
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
    private AudioClip _bigExplosionSoundClip;
    [SerializeField]
    private GameObject _explosionPrefab;
    private int _shieldStrength = 3;
    [SerializeField] 
    private GameObject _shieldVisualizerModerateDamage;
    [SerializeField]
    private GameObject _shieldVisualizerHeavyDamage;
    [SerializeField] 
    private int _ammo;
    [SerializeField]
    private GameObject _spaceBlastPrefab;
    private bool _isSpaceBlastActive = false;
    private float _spaceBlastCooldown = 2.0f;
    
    
    
    
    
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
        _ammo = 15;
        _uiManager.UpdateAmmo(_ammo);


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
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
            transform.Translate(direction * (_speed * _speedSprintMulitplier) * Time.deltaTime);
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
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammo > 0)
        {
            _canFire = Time.time + _fireRate;

            if (_isTripleShotActive == true)
            {
                _audioSource.clip = _laserSoundClip;
                Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
                _ammo--;
                _uiManager.UpdateAmmo(_ammo);
            }
            else if (_isSpaceBlastActive == true)
            {
                Instantiate(_spaceBlastPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                _ammo--;
                _uiManager.UpdateAmmo(_ammo);
                _audioSource.clip = _bigExplosionSoundClip;
            }
            else
            {
                _audioSource.clip = _laserSoundClip;
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
                _ammo--;
                _uiManager.UpdateAmmo(_ammo);
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
            _shieldStrength--;
            
            switch (_shieldStrength)
           {
               case 0:
                   _shieldVisualizer.SetActive(false);
                   _shieldVisualizerModerateDamage.SetActive(false);
                   _shieldVisualizerHeavyDamage.SetActive(false);
                   _isShieldActive = false;
                   break;
               case 1:
                   _shieldVisualizerModerateDamage.SetActive(false);
                   _shieldVisualizerHeavyDamage.SetActive(true);
                   break;
               case 2:
                   _shieldVisualizer.SetActive(false);
                   _shieldVisualizerModerateDamage.SetActive(true);
                   break;
               case 3:
                   _shieldVisualizer.SetActive(true);
                   break;
           }
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
        yield return new WaitForSeconds(_tripleShotCooldown);
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
        yield return new WaitForSeconds(_speedBoostCooldown);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
    
    public void ShieldActive()
    {
        _isShieldActive = true;
        //spawn shield visualizer on player and make it move with player
        switch (_shieldStrength)
        {
            case 3:
                _shieldVisualizer.SetActive(true);
                return;

            // else if (_shieldStrength == 2)

            case 2:
                _shieldVisualizerModerateDamage.SetActive(false);
                _shieldVisualizer.SetActive(true);
                _shieldStrength++;
                break;

            // else if (_shieldStrength == 1)

            case 1:
                _shieldVisualizerHeavyDamage.SetActive(false);
                _shieldVisualizerModerateDamage.SetActive(true);
                _shieldStrength++;
                break;

            // else

            case 0:
                _shieldVisualizer.SetActive(true);
                _shieldStrength = 3;
                break;
        }

    }
    
    public void AmmoRefill()
    {
        _ammo = 15;
        _uiManager.UpdateAmmo(_ammo);
    }
    
    public void HealthRefill()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
            
            if (_lives == 2)
            {
                _leftEngine.SetActive(false);
            }
            else if (_lives == 3)
            {
                _rightEngine.SetActive(false);
            }
        }
    }

    public void SpaceBlast()
    {
        //instantiate space blast prefab
        //Instantiate(_spaceBlastPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        _isSpaceBlastActive = true;
        StartCoroutine(SpaceBlastPowerDownRoutine());
        //message console
        Debug.Log("Space Blast Activated");
        
    }
    
    IEnumerator SpaceBlastPowerDownRoutine()
    {
        yield return new WaitForSeconds(_spaceBlastCooldown);
        _isSpaceBlastActive = false;
    }
    
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }

}
