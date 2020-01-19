using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAtCamera : MonoBehaviour {

	void Update () {
        if ( Camera.main != null ) {
            transform.forward = Camera.main.transform.forward;
        }
//		transform.LookAt (Camera.main.transform.position, -Vector3.up);
	}
}
