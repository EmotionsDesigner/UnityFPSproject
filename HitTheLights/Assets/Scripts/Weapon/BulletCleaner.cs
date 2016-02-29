using UnityEngine;
using System.Collections;

public class BulletCleaner : MonoBehaviour {
    public GameObject ShootMark;
    bool destroyBullet = true;
    void OnCollisionEnter(Collision col)
    {
     
       
        if (name != "BulletBase")
        {       
            //jeżeli kula trafi w martwego już wroga(leżącego na ziemi, to nie zostaje zniszczona) 
            if (col.transform.tag == "Enemy")
            {            
                  ZombieAI Zombie = col.gameObject.GetComponentInParent<ZombieAI>();
                  if (Zombie.health <= 0)
                       destroyBullet = false;                    
            }
            else
                destroyBullet = true;
              
            
            //usunięcie pocisku
            if (destroyBullet)
            {
               //ślad po strzale
                 Quaternion rotation = new Quaternion(0,0,0,0);
                 Instantiate(ShootMark, transform.position,rotation);
                Destroy(gameObject);
            }
                 
        }
         

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
