using UnityEngine;
using System.Collections;
using Pathfinding;

public class ZombieAI : MonoBehaviour {
    public CharacterController controller;

    //referencja do wyszukiwacza ścieżki 
    public AIPath AIPath;

    //cel AKUTALNY
    public GameObject target;
    public float TargetDistance;
    Vector3 TargetPosition;

    //cele statyczne (między którymi porusza się zombie, dopóki gracz nie będzie w jego zasięgu)
    public GameObject[] staticTargets;
     GameObject actualStaticTarget;

    //właściwości obiektu
    public float attackDistance;
    //poniżej tej odległości zombie zaczyna podążać za graczem
    public float huntingRange;
    public float health;
    public float speed;
    float approximation=1f;
    Animator animator;
    bool attackPlaying = false;
    //w momencie śmierci przyjmuje wartość true
    bool deathMoment=false;
    bool attack = false;


    //zwraca najbliższy statyczny cel lub null jeżeli brak celów statycznych
    GameObject FindClosestStaticTarget()
    {
        GameObject closestTarget=null;
        if (staticTargets.Length>0){
            closestTarget = staticTargets[0];
            float closestDistance = Vector3.Distance(transform.position, staticTargets[0].transform.position);
            foreach (GameObject staticTarget in staticTargets)
            {
                float distance = Vector3.Distance(transform.position, staticTarget.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance=distance;
                    closestTarget = staticTarget;
                }
                    
            }
         }
        return closestTarget;
    }


    //potwór porusza się między kolejnymi celami z tablicy, dopóki gracz nie znajdzie się w jego zasięgu
    void MoveBetweenStaticTargets() { 
       
    
    
    
    
    
    }





    void OnCollisionEnter(Collision col)
    {
        //Debug.Log(col.transform.name);
    }

    void StopHunting()
    {
         //przerwanie pościgu i wyszukiwania ścieżki
            AIPath.canMove = false;
            AIPath.canSearch = false;
    }

    void StartHunting()
    {
         //ponowienie pościgu i wyszukiwania ścieżki
            AIPath.canMove = true;
            AIPath.canSearch = true;
    }
    struct direction
    {
        public bool x;
        public bool y;
        public bool z;
      public void setX(){
          x=true;
          y=false;
          z=false;
      }
        public void setY(){
          x=false;
          y=true;
          z=false;
      }
        public void setZ(){
          x=false;
          y=false;
          z=true;
      }
    };

    //zwraca wartość, do którego dana współrzędna powinna dążyć
    float ChooseDirection(direction dir)
    {
        if (dir.x)
        {
            if (transform.position.x+approximation < TargetPosition.x)
                return speed;
            else if (transform.position.x-approximation > TargetPosition.x)
                return -speed;
            else
                return 0;
        }
       if (dir.y)
        {
            if (transform.position.y+approximation < TargetPosition.y)
                return speed;
            else if (transform.position.y-approximation > TargetPosition.y)
                return -speed;
            else return 0;
        }
        if (dir.z)
        {
            if (transform.position.z +approximation< TargetPosition.z)
                return speed;
            else if (transform.position.z-approximation > TargetPosition.z)
                return -speed;
            else return 0;
        }
       return 0f;//zła wartość (tylko x,y)
    }

    IEnumerator AudioPlay(string name, float delay)
    {

        attackPlaying = true;
        AudioSource audio = GameObject.Find(name).GetComponent<AudioSource>();
        audio.Play();
        yield return new WaitForSeconds(delay);
        attackPlaying = false;
    }



	// Use this for initialization
	void Start () { 
        controller = GetComponent<CharacterController>();
        animator = GetComponent < Animator>();
  
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //uaktualnienie punktów w życia w kontrolerze animacji
        animator.SetFloat("Health", health);
         animator.SetBool("Attack", attack);
        //uaktualnienie odległości od gracza
        TargetPosition = target.transform.position;
        TargetDistance = Vector3.Distance(transform.position, target.transform.position);

        //moment śmierci
        if (health <= 0 && deathMoment==false)
        {
            deathMoment = true;
            StopHunting();

            //dezaktywuje siatki kolizji dla wszystkich części ciała umarłego zombie
            MeshCollider[] meshColliderChildren = gameObject.GetComponentsInChildren<MeshCollider>();
            foreach (MeshCollider meshChildCollider in meshColliderChildren)
                meshChildCollider.enabled = false;      
            AudioPlay("ZombieDead", 0);
        }
        //pościg/atak gracza
        if (TargetDistance < huntingRange && health > 0)
        {
              StartHunting();
            //atak
            if (TargetDistance <= attackDistance){
                StopHunting();
                //odwrócenie w strone gracza
                  transform.LookAt(target.transform);
                  attack = true;
                if (attackPlaying==false)
                  StartCoroutine(AudioPlay("ZombieAttack", 2f));
            }
            else
            {
                 attack = false;
                 StartHunting();
            }
               
        }
            //gracz wychodzi poza zasięg
        else {
            StopHunting();
        }
	}
}
