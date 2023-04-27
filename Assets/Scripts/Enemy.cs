using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //enemy speed
    [SerializeField] private float _speed = 4.0f;
    private float _minValue = 0.5f;
    private float _maxValue = 4.0f;
    private float randomValue;

    private Player _player;

    private Enemy _enemy;
    
    //private Laser _laser;

   // private Enemy _dodger;

    //private Powerup _powerup;
    //handle to the animator component
    private Animator _anim;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private GameObject _sniperMissilePrefab;
    private int direction = 1;
    private float leftLimit = -9.0f;
    private float rightLimit = 9.0f;
    private float _currentEnemyXPosition;
    private float _currentEnemyYPosition;
    private Vector3 _enemyPosition;
    private float tolerance = 0.25f;
    [SerializeField]
    private float dodgeTolerance = 3.0f;
    private bool _laserCanFire = true;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private bool _isShielded = false;
    [SerializeField] private GameObject _shield;
    private float flashInterval = 0.25f;
    [SerializeField] private float redIntensity = 1.0f;
    private Renderer rend;
    private Color originalColor;
   // private bool _isFlashing = false;
    private float minDistance = 4.0f;
    private float moveSpeed = 5.0f;
   // private bool _isHunting = true;
    private float _dodgeChance;
    [SerializeField]
    private float _dodgeDistance = 15.0f;
    private int _dodgeDirection;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        //set PlayerTransform to the player's transform
        PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();
        // _enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        //_powerup = GameObject.Find("Powerup").GetComponent<Powerup>();
        //assign the animator component
        //null check player
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _anim = GetComponent<Animator>();

        //if the gameobject tag is enemy, start the coroutine
        if (gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(FireLaserRoutine());
            StartCoroutine(PowerUpDestroyer());
        }
        else if (gameObject.CompareTag("Sniper"))
        {
            StartCoroutine(FireSniperRoutine());
        }

        if (gameObject.CompareTag("Shielded Enemy"))
        {
            EnemyShield();
            StartCoroutine(FireLaserRoutine());
        }

        if (gameObject.CompareTag("Suicider"))
        {
            rend = GetComponent<Renderer>();
            originalColor = rend.material.color;
            StartCoroutine(Suicider());
            StartCoroutine(FlashRed());
        }
        if (gameObject.CompareTag("Dodger"))
        {
            StartCoroutine(Dodger());
            StartCoroutine(DodgeChance());
            StartCoroutine(DodgeDirection());
            StartCoroutine(FireLaserRoutine());
        }
    }

    // Update is called once per frame
    void Update()
    {
        randomValue = Random.Range(_minValue, _maxValue);

        //move down at 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        transform.Translate(direction * randomValue * Time.deltaTime, 0, 0);

        if (transform.position.x <= leftLimit)
        {
            direction = 1;
        }
        else if (transform.position.x >= rightLimit)
        {
            direction = -1;
        }

        //if bottom of screen
        //respawn at top with a new random x position
        if (transform.position.y < -5.8f)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            if (_isShielded == false)
            {
                Instantiate(_explosionPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                Destroy(this.gameObject, 0.25f);
            }
            else if (_isShielded == true)
            {
                _isShielded = false;
                EnemyShield();
            }

        }

        if (other.tag == "Laser" || other.tag == "Friendly Missile")
        {
            if (_isShielded == false)
            {
                Instantiate(_explosionPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                Destroy(this.gameObject, 0.25f);
                _player.AddScore(10);
            }
            else if (_isShielded == true)
            {
                _isShielded = false;
                EnemyShield();
            }

            Destroy(other.gameObject);
        }

        if (other.tag == "Space Blast")
        {
            Instantiate(_explosionPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(this.gameObject, 0.25f);
            _player.AddScore(10);
        }
    }

    IEnumerator FireLaserRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 7f));
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -1.05f, 0), Quaternion.identity);
        }
    }

    public void FireLaser()
    {
        if (_laserCanFire == true)
        {
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -1.05f, 0), Quaternion.identity);
            _laserCanFire = false;
            StartCoroutine(LaserCooldown());
        }
    }

    IEnumerator LaserCooldown()
    {
        yield return new WaitForSeconds(3f);
        _laserCanFire = true;
    }

    IEnumerator FireSniperRoutine()
    {

        while (true)
        {
            yield return new WaitForSeconds(4f);
            Instantiate(_sniperMissilePrefab, transform.position + new Vector3(0, -1.05f, 0), Quaternion.identity);
        }
    }

    public void EnemyShield()
    {
        if (_isShielded == false)
        {
            _shield.SetActive(false);
            StartCoroutine(EnemyShieldRechargeRoutine());
        }
        else if (_isShielded == true)
        {
            _shield.SetActive(true);
        }
    }

    IEnumerator EnemyShieldRechargeRoutine()
    {
        yield return new WaitForSeconds(3f);
        _isShielded = true;
        EnemyShield();
    }

    IEnumerator Suicider()
    {

        //loop until object is  destroyed
        while (_player != null)
        {
            float distance = Vector3.Distance(transform.position, PlayerTransform.position);

            if (distance <= minDistance)
            {
                Vector3 targetDirection = PlayerTransform.position - transform.position;
                targetDirection.y = 0.0f;
                targetDirection = targetDirection.normalized;
                transform.position += targetDirection * moveSpeed * Time.deltaTime;
            }

            //yield
            yield return null;
        }
    }

    IEnumerator FlashRed()
    {
        while (true)
        {
            yield return new WaitForSeconds(flashInterval);
            rend.material.color = new Color(redIntensity, 0.0f, 0.0f);
            yield return new WaitForSeconds(flashInterval);
            rend.material.color = originalColor;
        }
    }

    IEnumerator DodgeChance()
    {
        yield return new WaitForSeconds(1.0f);
        //int randomDodgeChance = Random.Range(3, 10);
       float randomDodgeChance = 10;
        _dodgeChance = randomDodgeChance;
        //write a message
       // Debug.Log("Dodge chance is " + _dodgeChance);
        
    }

    IEnumerator Dodger()
    {

        while (true)
        {
            
            GameObject[] _lasers = GameObject.FindGameObjectsWithTag("Laser");
            _enemy = this;

            foreach (GameObject _laser in _lasers)
            {
                Vector3 _dodgerPosition = _enemy.transform.position;
                Vector3 _laserPosition = _laser.transform.position;

                bool isWithinDodgeRange = Mathf.Abs(_dodgerPosition.y - _laserPosition.y) < dodgeTolerance;

                if (isWithinDodgeRange && _dodgerPosition.y > _laserPosition.y && _dodgeChance > 5)
                {
                    //write a message to console
                    Debug.Log("Dodger is dodging");
                    StartCoroutine(Dodge());
                }
            }

            yield return null;
        }

    }

    IEnumerator Dodge()
   {

       for (int i = 0; i < 3; i++)
         {
              //move left or right
             // _dodgeDirection = Random.Range(-1, 1);
              transform.Translate(_dodgeDirection * _dodgeDistance * Time.deltaTime, 0, 0);
              //wait for 1 second
              yield return new WaitForSeconds(0.5f);
         }
   }

    IEnumerator DodgeDirection()
    {
        while (true)
        {
            _dodgeDirection = Random.Range(-1, 2);
            Debug.Log("Dodge direction is " + _dodgeDirection);
            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator PowerUpDestroyer()
    {
        while (true)
        {
            {
                GameObject[] _powerups = GameObject.FindGameObjectsWithTag("Powerup");
                //_enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
                _enemy = this;

                foreach (GameObject _powerup in _powerups)
                {
                    Vector3 _enemyPosition = _enemy.transform.position;
                    Vector3 _powerupPosition = _powerup.transform.position;

                    bool isDirectlyAbove = Mathf.Abs(_enemyPosition.x - _powerupPosition.x) < tolerance;

                    if (isDirectlyAbove && _enemyPosition.y > _powerupPosition.y)
                    {
                        FireLaser();
                    }
                }
            }
            yield return null;
        }
    }

}
