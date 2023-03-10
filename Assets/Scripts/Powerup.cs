using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour


{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerupID; 
    [SerializeField]
    private AudioClip _powerupSound;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down at speed
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        //if bottom of screen, destroy this object
        if (transform.position.y < -5.5f)
        {
            Destroy(this.gameObject);
        }
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
                }
            }
            Destroy(this.gameObject);
        }
    }
}
