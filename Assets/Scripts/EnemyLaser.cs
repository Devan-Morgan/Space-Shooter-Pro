using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    //speed variable of 8
    [SerializeField] private float _speed = 8.0f;

    // Update is called once per frame
    void Update()
    {
        //move laser down
        transform.Translate(Vector3.down * _speed * Time.deltaTime);


        if (transform.position.y < -8.0f)
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
        }

    }
}

