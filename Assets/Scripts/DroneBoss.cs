using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBoss : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private GameObject _sniperMissilePrefab;
    // [SerializeField] private GameObject _shield;
    
    [SerializeField] private float Zpos = 0f;
    [SerializeField] private float Ypos = -1.05f;
    [SerializeField] private float Xpos = 0f;
    [SerializeField] private float _speed = 8.0f;
    private float direction = 1;
    private bool LaserHell = false;
    private float leftLimit = -9.0f;
    private float rightLimit = 9.0f;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private Vector3 dir;
    private Rigidbody2D rb;

    private bool MissileStorm = false;
    Quaternion rotateToTarget;
    private float rotationSpeed = 2.5f;
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Player _player;

    private float _health = 1f;
    private UIManager _uiManager;
    private float _attackChoice;
    // Start is called before the first frame update
    void Start()
    {
        
       _player = GameObject.Find("Player").GetComponent<Player>();
       target = GameObject.Find("Player");
       _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
       rb = GetComponent<Rigidbody2D>();
       _uiManager.HealthManager(_health);
       _uiManager.BossHealthBarActive();
         StartCoroutine(MoveToAttackPOS());
        //StartPosition();
        //StartRotation();
        //StartCoroutine(LaserHellManager());
        //StartCoroutine(MissileStormManager());
        //StartCoroutine(AttackManager());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveToAttackPOS()
    {
        Debug.Log("MoveToAttackPOS");
        while (Mathf.Abs(4 - transform.position.y) > 0.2f)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            Debug.Log("Working");
            yield return null;
        }
        rb.velocity = Vector2.zero;
        StartPosition();
        StartRotation();
        StartCoroutine(AttackManager());
    }

    private void StartPosition()
    {
        _startPosition = this.transform.position;
    }
    
    private void StartRotation()
    {
        _startRotation = this.transform.rotation;
    }

    IEnumerator LaserHellManager()
    {
        LaserHell = true;
        StartCoroutine(FireLaserRoutine());
        StartCoroutine(LaserHellMovement());
        yield return new WaitForSeconds(10.0f);
        LaserHell = false;
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(ReturnToStartPosition());
        StartCoroutine(AttackManager());
    }
    
    IEnumerator FireLaserRoutine()
    {
        //laser firing code
        //wait for 5 seconds
        yield return new WaitForSeconds(0.5f);
        while (LaserHell == true)
        {
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(Xpos, Ypos, Zpos), Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator LaserHellMovement()
    {
        while (LaserHell == true)
        {
            transform.Translate(direction * _speed * Time.deltaTime, 0, 0);

            if (transform.position.x <= leftLimit)
            {
                direction = 1;
            }
            else if (transform.position.x >= rightLimit)
            {
                direction = -1;
            }
            yield return null;
        }
    }
    
    IEnumerator MissileStormManager()
    {
        MissileStorm = true;
        StartCoroutine(FireMissileRoutine());
        StartCoroutine(MissileStormMovement());
        yield return new WaitForSeconds(10.0f);
        MissileStorm = false;
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(ReturnToStartPosition());
        StartCoroutine(ReturnToStartRotation());
        StartCoroutine(AttackManager());
    }

    IEnumerator MissileStormMovement()
    {
        while (MissileStorm == true)
        {
            dir = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
            yield return null;
        }
    }

    IEnumerator FireMissileRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        while (MissileStorm == true)
        {
            Instantiate(_sniperMissilePrefab, transform.position + new Vector3(Xpos, Ypos, Zpos), Quaternion.identity);
            yield return new WaitForSeconds(2.00f);
        }
    }

    IEnumerator ReturnToStartPosition()
    {
        while(Mathf.Abs(_startPosition.x - transform.position.x) > 0.2f) //|| _startPosition.y - transform.position.y < 1.0f)
        {
            dir = (_startPosition - transform.position).normalized;
            rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed);
           yield return null;
        }
        rb.velocity = Vector2.zero;
    }

    IEnumerator ReturnToStartRotation()
    {
        while(_startRotation != transform.rotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _startRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
    }

    IEnumerator AttackManager()
    {
        
            yield return new WaitForSeconds(0.5f);
            _attackChoice = Random.Range(1, 3);
            if (_attackChoice == 1)
            {
                StartCoroutine(LaserHellManager());
            }
            else if (_attackChoice == 2)
            {
                StartCoroutine(MissileStormManager());
            }
            
            yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" || other.tag == "Friendly Missile")
        {
            if (_health > 0)
            {
                _health -= 0.025f;
                _uiManager.HealthManager(_health);
                //Debug.Log("Health: " + _health);
            }
            else
            {
                Instantiate(_explosionPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                _player.AddScore(100);
                _uiManager.GameOver();
                Destroy(this.gameObject, 0.25f);
            }
            Destroy(other.gameObject);
        }
    }
}
