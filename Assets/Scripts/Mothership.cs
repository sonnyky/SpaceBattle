using UnityEngine;
using System.Collections;

public class Mothership : MonoBehaviour {

	public int maxNumPlayerFighters;

    private GameObject fighterJetPrefab, newFighterJet;
    private int numOfPlayerFighters;
    private Vector3 fighterJetInitialPosition;
	// Use this for initialization
	void Start () {
        numOfPlayerFighters = 0;
        fighterJetPrefab = (GameObject)Resources.Load("Prefabs/PlayerFighter");
        fighterJetInitialPosition.x = 0f;
        fighterJetInitialPosition.y = -3.86f;
        fighterJetInitialPosition.z = 0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void createFighterJets()
    {
        newFighterJet = GameObject.Instantiate(fighterJetPrefab, fighterJetInitialPosition, this.transform.rotation) as GameObject; ;
        newFighterJet.name = "PlayerFighter" + numOfPlayerFighters;
        numOfPlayerFighters++;
    }
    public void reduceNumOfPlayerFighters()
    {
        this.numOfPlayerFighters--;
    }

	public bool canCreateFighterJet () {
		return numOfPlayerFighters < maxNumPlayerFighters;
	}
}
