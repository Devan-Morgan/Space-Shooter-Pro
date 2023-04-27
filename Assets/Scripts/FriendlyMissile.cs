using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMissile : MonoBehaviour
{
    private GameObject target1;
    private GameObject target1Clone;
    private GameObject target2;
    private GameObject target2Clone;
    private GameObject target3;
    private GameObject target3Clone;
    private GameObject target4;
    private GameObject target4Clone;
    private GameObject target5;
    private GameObject target5Clone;
    [SerializeField]
    private GameObject _explosionPrefab;
    private float rotationSpeed = 15.0f;
    [SerializeField]
    private float _speed = 2.5f;
    
    Quaternion rotateToTarget;
    private Vector3 dir;

    private Rigidbody2D rb;
    
    private bool hasTarget = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MissileEvaded());
    }

    // Update is called once per frame
    void Update()
    {
        target1 = GameObject.Find("Enemy");
        target1Clone = GameObject.Find("Enemy(Clone)");
        target2 = GameObject.Find("DreadShip");
        target2Clone = GameObject.Find("DreadShip(Clone)");
        target3 = GameObject.Find("Dodger_Ship");
        target3Clone = GameObject.Find("Dodger_Ship(Clone)");
        target4 = GameObject.Find("Shielded_Ship");
        target4Clone = GameObject.Find("Shielded_Ship(Clone)");
        target5 = GameObject.Find("Suicide_Ship");
        target5Clone = GameObject.Find("Suicide_Ship(Clone)");
        

        if (target1 != null /*&& hasTarget == false*/)
        {
            dir = (target1.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
            rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed);
            hasTarget = true;
        }
        else if (target1Clone != null /*&& hasTarget == false*/)
        {
            dir = (target1Clone.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
            rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed);
            hasTarget = true;
        }
        else if (target2 != null /*&& hasTarget == false*/)
        {
            dir = (target2.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
            rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed);
            hasTarget = true;
        }
        else if (target2Clone != null )
        {
            dir = (target2Clone.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
            rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed);
            hasTarget = true;
        }
        else if (target3 != null )
        {
            dir = (target3.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
            rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed);
            hasTarget = true;
        }
        else if (target3Clone != null )
        {
            dir = (target3Clone.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
            rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed);
            hasTarget = true;
        }
        else if (target4 != null )
        {
            dir = (target4.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
            rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed);
            hasTarget = true;
        }
        else if (target4Clone != null )
        {
            dir = (target4Clone.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
            rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed);
            hasTarget = true;
        }
        else if (target5 != null )
        {
            dir = (target5.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
            rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed);
            hasTarget = true;
        }
        else if (target5Clone != null )
        {
            dir = (target5Clone.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
            rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed);
            hasTarget = true;
        }
        else
        {
            hasTarget = false;
        }

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" || other.tag == "Suicider" || other.tag == "Dodger" || other.tag == "Shielded Enemy" || other.tag == "Sniper")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    
    IEnumerator MissileEvaded()
    {
        yield return new WaitForSeconds(10f);
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
