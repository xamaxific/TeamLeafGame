using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TurnOffOnActive : MonoBehaviour {
	public List<GameObject> gameObjects;

	[Space]
	[Header("MODIFIERS")]

	// Turns OFF or ON all children of the parent which this script is on isntead of inputting target GameObjects manually.
	public bool CollectChildren;

	// toggle bool: Switches all target objects On or Off
	public bool ShowInstead;

	// Use this for initialization
	void Awake () {
		if (CollectChildren) {
			collectChildren ();
		}
	}

	void collectChildren(){
		foreach (Transform child in transform) {
			gameObjects.Add (child.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable() {
			DoAction ();
	}

	public void ShowEverything(){
		ShowInstead = true;
		DoAction ();
	}

	public void HideEverything(){
		ShowInstead = false;
		DoAction ();
	}

	void DoAction() {
		foreach (GameObject _go in gameObjects) {
			if (_go != null)
			if (!ShowInstead) {
				_go.SetActive (false);
			} else {
                _go.SetActive(false);
				_go.SetActive (true);
			}
		}
	}
}
