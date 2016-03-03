using UnityEngine;
using System.Collections;

public class PlayerFighterMovement : MonoBehaviour {

	public float range;

    Vector3 cur_pos, new_pos;
    Vector3 velocity;
    Mothership playerMothershipReferenceObject;

    private GameObject projectileAsset, newProjectile;
    private int numOfProjectiles;

    private bool collided, inRange, shooting;
	// Use this for initialization
	void Start () {
        playerMothershipReferenceObject = GameObject.Find("AstraHeavyCruiser01").GetComponentInChildren<Mothership>();

        //Initiating weapon for player fighter jet
        projectileAsset = (GameObject)Resources.Load("Prefabs/Laser");
        numOfProjectiles = 0;
        cur_pos = this.gameObject.transform.localPosition;
        velocity.x = 0;
        velocity.y = 0.05f;
        velocity.z = 0;
		collided = false;
		inRange = false;
        shooting = false;
    }

    void DestroyThisObject()
    {
        Destroy(gameObject);
    }

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.name == "AstraHeavyCruiser01_enemy") {
			this.collided = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!collided) {
			Vector3 viewPos = Camera.main.WorldToViewportPoint(this.transform.position);
			if (viewPos.y < 1.0f && viewPos.y > 0f)
			{
				new_pos.x = cur_pos.x + velocity.x;
				new_pos.y = cur_pos.y + velocity.y;
				new_pos.z = cur_pos.z + velocity.z;
				this.gameObject.transform.localPosition = new_pos;
				cur_pos = new_pos;

				if (!inRange) {
					checkRange ();
				}
				else {
					//start shooting once in range
					if (shooting == false) {
						StartCoroutine ("spawnProjectiles", gameObject.name);
					}
				}
			}
			else
			{
				this.DestroyThisObject();
				playerMothershipReferenceObject.reduceNumOfPlayerFighters();
			}
        }
        else
        {
            //start shooting after collision
            if (shooting == false)
            {
                StartCoroutine("spawnProjectiles", gameObject.name);
            }
        }
    }

    IEnumerator spawnProjectiles(string playerFighterId)
    {
        shooting = true;
        while (true)
        {
            newProjectile = GameObject.Instantiate(projectileAsset, cur_pos, this.transform.rotation) as GameObject; ;
            newProjectile.name = "newProjectile" + numOfProjectiles;
            numOfProjectiles++;
			yield return new WaitForSeconds(2);
        }
    }

	void checkRange() {
		GameObject enemy = GameObject.Find ("AstraHeavyCruiser01_enemy");
		float distance = Vector3.Distance (transform.position, enemy.transform.position);
		if (distance <= range) {
			this.inRange = true;
		}
	}
}
