using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public bool m_isFollowing = true;
    private void Update () {
        if ( m_isFollowing ) {
            Follow();
        }
    }

    public void Follow () {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        Vector3 v3 = new Vector3( mousePos.x, mousePos.y, transform.position.z );
        transform.position = v3;
    }
}
