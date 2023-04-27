using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Powerup : MonoBehaviour


{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerupID; 
    [SerializeField]
    private AudioClip _powerupSound;
    [SerializeField] 
    private AudioClip _explosionSound;
    private float _currentXPosition;
    private float _currentYPosition;
    [SerializeField]
    private Enemy _enemy; 
    [SerializeField]
    private float _rotateSpeed = 0f;
    private GameObject SatellitePowerup;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down at speed
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);

        //if bottom of screen, destroy this object
        if (transform.position.y < -5.5f)
        {
            Destroy(this.gameObject);
        }
        
        Vector3 thisObjectPos = transform.position;

        // set the currentXPosition variable to the x position of the object
        _currentXPosition = thisObjectPos.x;

        // set the currentYPosition variable to the y position of the object
        _currentYPosition = thisObjectPos.y;

       // _enemy.ComparePos(_currentXPosition, _currentYPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if player collides with powerup, destroy powerup
        if (other.CompareTag("Player"))
        {
            //play _powerupSound at point
            AudioSource.PlayClipAtPoint(_powerupSound, transform.position);
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoRefill();
                        break;
                    case 4:
                        player.HealthRefill();
                        break;
                    case 5:
                        player.SpaceBlast();
                        break;
                    case 6:
                        player.EMP();
                        break;
                    case 7:
                        player.Satellite();
                        break;
                }
            }
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Enemy Laser"))
        {
            AudioSource.PlayClipAtPoint(_explosionSound, transform.position);
            Destroy(other.GameObject());
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Laser") && gameObject.tag == "EMP" )
        {
            AudioSource.PlayClipAtPoint(_explosionSound, transform.position);
            Destroy(other.GameObject());
            Destroy(this.gameObject);
        }
    }
}
