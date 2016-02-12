using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonControl : MonoBehaviour {

	/* The base ship for this button.
	 * When the button is pressed, a ship will be launched
	 * from this mothership.
	 * (Typically this should be the player mothership)
	*/
	public Mothership hq;
	// The length of time to disable the button after a click
	public float interval;

	private float timer;


	// Use this for initialization
	void Start () {
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		gameObject.transform.Find ("TimerWheel").GetComponent<Image> ().fillAmount = timer / interval;
		if (gameObject.GetComponent<Button> ().interactable == false && timer <= 0) {
			// reset timer to zero in case it went negative
			timer = 0;
			gameObject.GetComponent<Button> ().interactable = true;
		}
	}

	public void launch() {
		hq.createFighterJets ();
		gameObject.GetComponent<Button> ().interactable = false;
		this.timer = interval;
		gameObject.transform.Find ("TimerWheel").GetComponent<Image> ().fillAmount = 1;
	}

}
