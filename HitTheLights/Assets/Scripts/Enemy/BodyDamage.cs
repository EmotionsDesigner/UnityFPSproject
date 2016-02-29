using UnityEngine;
using System.Collections;

public class BodyDamage : MonoBehaviour {
    public ZombieAI Zombie;
    public float damage;
     void OnCollisionEnter(Collision col)
    {
         Debug.Log("Kolizja zombie z " + col.transform.name);
        if (col.transform.tag == "Bullet" && Zombie.health>0)
        {
              Zombie.health -= damage ;
              AudioSource a = GameObject.Find("Hit").GetComponent<AudioSource>();
              a.Play();

        }
        else
        {//zombie natrafi na przeszkode
            Zombie.speed = 0f;
        }
          
        
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //if (Zombie.Health <= 0)
            //Destroy(gameObject);
	}
}
