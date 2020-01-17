using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineFollow : MonoBehaviour
{
    public bool m_interactable;
    public Transform m_target;
    bool m_isChanging;
    // Update is called once per frame
    void Update()
    {
        if ( m_interactable && Input.GetKey( KeyCode.Z ) ) {
            float z = m_target.transform.position.z;
            Vector3 camToPoint = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            m_target.transform.position = new Vector3( camToPoint.x, camToPoint.y, z );
        }

        LineRenderer line = this.GetComponent<LineRenderer>();
        if ( line.positionCount == 2 ) {
            line.SetPosition( 1, m_target.position - transform.position );
        } else {
            Debug.Log( "Too Many Points to follow" );
        }
    }
}
