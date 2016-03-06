using UnityEngine;
using System.Collections;

public class HitPointBlock : MonoBehaviour {
    private Vector3 curScale;
	// Use this for initialization
	void Start () {
        curScale = gameObject.transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void HitPointReduceTo(float scale)
    {
        curScale.y = scale * curScale.y;
        gameObject.transform.localScale = curScale;
    }
}
