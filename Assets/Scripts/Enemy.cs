using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //enemy speed
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    //handle to the animator component
    private Animator _anim;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        //assign the animator component
        //null check player
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        _anim = GetComponent<Animator>();
        
        StartCoroutine(FireLaserRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        //move down at 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
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
            Instantiate(_explosionPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(this.gameObject, 0.25f);
           /* //trigger animation
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.5f);*/

        }
        
        if (other.tag == "Laser")
        {
          /*  _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(other.gameObject);
            Destroy(this.gameObject, 2.5f);*/
          Instantiate(_explosionPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
          Destroy(other.gameObject);
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
           //yield return new WaitForSeconds(Random.Range(2f, 7f));
        }
    }
}
