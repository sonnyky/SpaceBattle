using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {
    private Vector3 cur_pos, new_pos;
    private Vector3 velocity;
    private bool collided;
    private float damagePower;
    // Use this for initialization
    void Start () {
        damagePower = 2.0f;
        cur_pos = this.gameObject.transform.localPosition;
        velocity.x = 0;
        velocity.y = 0.1f;
        velocity.z = 0;
        collided = false;
    }

    private void onCollisionEnter()
    {
        collided = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!collided)
        {
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
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
