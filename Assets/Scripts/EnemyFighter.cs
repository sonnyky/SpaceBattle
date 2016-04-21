using UnityEngine;
using System.Collections;

/*
This script controls enemy fighter jets movement and other behaviors such as receiving damage and giving score to the player upon destruction.
*/

public class EnemyFighter : MonoBehaviour {

    public float range;

    Vector3 cur_pos, new_pos;
    Vector3 velocity;
    Mothership playerMothershipReferenceObject;

    private GameObject projectileAsset, newProjectile;
    private int numOfProjectiles;

    private float thisShipHp, initialShipHp;
    private bool collided, inRange, shooting;

    // Use this for initialization
    void Start () {
        initialShipHp = thisShipHp = 20.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.collided = true;
        }
    }

    public void TakeDamage(float DamagePower)
    {
        /*
        Called by the projectile, with the projectile's damage power as input. After that, the daaged ship will tell its HP bar how much to decrease
        */
        thisShipHp -= DamagePower;
        gameObject.GetComponentInChildren<HitPointBlock>().HitPointReduceTo(thisShipHp/initialShipHp);
        if(thisShipHp <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

}
