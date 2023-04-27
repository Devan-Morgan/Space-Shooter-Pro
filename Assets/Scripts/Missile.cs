using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private GameObject target;
    [SerializeField]
    private GameObject _explosionPrefab;
    private float rotationSpeed = 2.5f;
    [SerializeField]
    private float _speed = 2.5f;
    
    Quaternion rotateToTarget;
    private Vector3 dir;

    private Rigidbody2D rb;

    void Start()
    {
        target = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(MissileEvaded());
    }

    void Update()
    {
        //if object isn't null
        if (target != null)
        {
            dir = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
            rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed);
        }
        else
        {
            Destroy(this.gameObject);
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

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    
    IEnumerator MissileEvaded()
    {
        yield return new WaitForSeconds(5f);
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
    
    
}
