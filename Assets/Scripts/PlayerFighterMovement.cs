using UnityEngine;
using System.Collections;

public class PlayerFighterMovement : MonoBehaviour {
    Vector3 cur_pos, new_pos;
    Vector3 velocity;
    Mothership playerMothershipReferenceObject;
	bool collided;
	// Use this for initialization
	void Start () {
        playerMothershipReferenceObject = GameObject.Find("AstraHeavyCruiser01").GetComponentInChildren<Mothership>();
        cur_pos = this.gameObject.transform.localPosition;
        velocity.x = 0;
        velocity.y = 0.05f;
        velocity.z = 0;
		collided = false;
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
			}
			else
			{
				this.DestroyThisObject();
				playerMothershipReferenceObject.reduceNumOfPlayerFighters();
			}
		}
    }
}
