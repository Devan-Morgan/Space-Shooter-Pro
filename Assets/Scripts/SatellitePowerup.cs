using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatellitePowerup : MonoBehaviour
{
 //   [SerializeField] 
  //  private bool SatelliteActive = false;

   // [SerializeField] 
    //private bool DoesSatelliteHaveAmmo = false;
    
    [SerializeField]
    private GameObject _friendlyMissilePrefab;
    
    private float SatelliteAmmo = 10f;
    
   private Animator animator;
   [SerializeField] 
   private bool isSatelliteActive;
   [SerializeField] 
    private bool doesSatelliteHaveAmmo;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();   
        animator.SetBool("isSatelliteActive", false);
        animator.SetBool("doesSatelliteHaveAmmo", false);
        isSatelliteActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SatelliteAmmo <= 0)
        {
            SatelliteInactive();
            SatelliteAmmo += 10;
        }
        else if(Input.GetKeyDown(KeyCode.Q) && isSatelliteActive == true)
        {
            SatelliteAmmo -= 1;
            float randomX = Random.Range(-9f, 9f);
            Instantiate(_friendlyMissilePrefab,new Vector3(randomX, 11.0f, 0), Quaternion.identity);
        }
    }

    public void SatelliteActivate()
    {
        if (isSatelliteActive == false)
        {
            isSatelliteActive = true;
            animator.SetBool("isSatelliteActive", true);
            animator.SetBool("doesSatelliteHaveAmmo", true);
            //Debug.Log("Satellite Active");
        }
        else if (isSatelliteActive == true)
        {
            SatelliteAmmo += 10;
        }
    }
    
    public void SatelliteInactive()
    {
        //SatelliteActive = false;
        //DoesSatelliteHaveAmmo = false;
        isSatelliteActive = false;
        animator.SetBool("isSatelliteActive", false);
        animator.SetBool("doesSatelliteHaveAmmo", false);
    }
}
