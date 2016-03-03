using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonControl : MonoBehaviour {

	/* The base ship for this button.
	 * When the button is pressed, a ship will be launched
	 * from this mothership.
	 * (Typically this should be the player mothership)
	*/
	public Mothership playerMothershipReferenceObject;
	// The length of time to disable the button after a click
	public float interval;

	private bool inCooldown;
	private float timer;


	// Use this for initialization
	void Start () {
		playerMothershipReferenceObject = GameObject.Find("AstraHeavyCruiser01").GetComponentInChildren<Mothership>();
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!playerMothershipReferenceObject.canCreateFighterJet ()) {
			gameObject.GetComponent<Button> ().interactable = false;
		}

		if (inCooldown) {
			timer -= Time.deltaTime;
			gameObject.transform.Find ("TimerWheel").GetComponent<Image> ().fillAmount = timer / interval;
			if (timer <= 0) {
				// reset timer to zero in case it went negative
				timer = 0;
				this.inCooldown = false;
				gameObject.GetComponent<Button> ().interactable = true;
			}
		}
	}

	void startCooldown() {
		this.inCooldown = true;
		gameObject.GetComponent<Button> ().interactable = false;
		this.timer = interval;
		gameObject.transform.Find ("TimerWheel").GetComponent<Image> ().fillAmount = 1;
	}

	public void launch() {
		playerMothershipReferenceObject.createFighterJets ();
		this.startCooldown ();
	}

}
