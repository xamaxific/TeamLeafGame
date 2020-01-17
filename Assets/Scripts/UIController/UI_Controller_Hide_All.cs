using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller_Hide_All : MonoBehaviour {




	// Use this for initialization
	void Start () {

      
	
	}
	
	// Update is called once per frame
	void Update () {

      

		
	}

    public void HideAll(){
        GetChildRecursive(this.gameObject);
        if (this.gameObject.GetComponent<UIController>()){
            this.gameObject.GetComponent<UIController>().Hide();
        }
    }

    private void GetChildRecursive(GameObject obj)
    {
        if (null == obj)
            return;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
                continue;
            //Do whatever you like to the children here:
            if (child.gameObject.GetComponent<UIController>() == null)
            {
                Debug.LogWarning("Doesn't contain UIController");
            }
            else{
                child.gameObject.GetComponent<UIController>().Hide();
                Debug.LogWarning("Contains UICOntroller");
            }
            GetChildRecursive(child.gameObject);
        }

    }

}
