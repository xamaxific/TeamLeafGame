using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TurnOnDelayOrder : MonoBehaviour
{
	public List<GameObject> gameObjects;

	[Space]
	[Header ("MODIFIERS")]
	public float delayPerObject = 0.05f;
	public bool CollectChildrenOnAwake;
	public bool CollectChildrenOnEnabled = false;
	public bool turnOffInstead;
	public bool DoOnEnable;

    public bool FirstChild = false;
    bool firstChildSkipped = false;

	// Use this for initialization
	void Awake ()
	{
		if (CollectChildrenOnAwake) {
			foreach (Transform child in transform) {
				gameObjects.Add (child.gameObject);
			}
		}
	}


	//Does every time the object is enabled
	void OnEnable ()
	{
		if (CollectChildrenOnEnabled) {
			gameObjects.Clear ();
            if (FirstChild == true)
            {
                foreach (Transform child in transform)
                {
                    gameObjects.Add(child.gameObject.transform.GetChild(0).gameObject);
                }
            }
            else
            {
                foreach (Transform child in transform)
                {
                    gameObjects.Add(child.gameObject);
                }
            }
		}

		if (DoOnEnable) {
			if (!turnOffInstead) {
				HideEverything ();
				ShowUpEverythingWithDelay ();
			} else {
				HideUpEverythingWithDelay ();
			}
		}
	}


	//Function - To be accessed iternally: Hide Everything
	void HideEverything ()
	{
		foreach (GameObject _go in gameObjects) {
			if (_go != null) {
				_go.SetActive (false);
			}
		}
	}

	//Public Function - To be accessed externally: Show Everything with delay
	public void ShowUpEverythingWithDelay ()
	{

		int index = 1;
		foreach (GameObject go in gameObjects) {
			index++;
			StartCoroutine (ShowObject (go, index));

		}
	}

	//Public Function - To be accessed externally: Hide Everything with delay
	public void HideUpEverythingWithDelay ()
	{
		
		int index = 1;
		foreach (GameObject go in gameObjects) {
			index++;
			StartCoroutine (HideObject (go, index));
		}
	}
		
	//Coroutine: Turns on objects with delay
	IEnumerator ShowObject (GameObject _go, int _index)
	{
		yield return new WaitForSeconds (delayPerObject * (_index - 1));
            if (_go != null)
                _go.SetActive(true);
	}

	//Coroutine: Turns off objects with delay
	IEnumerator HideObject (GameObject _go, int _index)
	{
		yield return new WaitForSeconds (delayPerObject * (_index));
		if (_go != null)
			_go.SetActive (false);
	}
}
