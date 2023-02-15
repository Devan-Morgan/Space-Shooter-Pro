using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBlast : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    
    private CircleCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        
        if (transform.position.y > 8.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
        
        StartCoroutine(IncreaseColliderSizeCoroutine());
    }
    
    private IEnumerator IncreaseColliderSizeCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        _collider.radius *= 2;
    }
}
    

