using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform m_target;
    public bool m_isFollowing;
    public bool m_isDamp;
    public bool m_followMouse;
    public bool m_offset;
    public Vector3 m_freeDir = new Vector3(1,1,1);
    Vector3 m_tempOffset;
    [Range( 0, 1 )]
    public float m_dampSpeed;
    private void FixedUpdate () {
        if ( m_isFollowing ) {
            Follow();
        }
    }

    public void OnStartInteract () {
        if ( !m_isFollowing ) {
            if(m_offset){
                if ( m_followMouse ) {
                    m_tempOffset = Camera.main.ScreenToWorldPoint( Input.mousePosition );
                } else {
                    if ( m_target != null ) {
                        m_tempOffset = m_target.position;
                    } else {
                        Debug.LogWarning( "No Follow Object Detected" );
                    }
                }
            }
            m_tempOffset = transform.position - m_tempOffset;
            m_isFollowing = true;
        }
    }

    public void OnEndInteract () {
        if ( m_isFollowing ) {
            m_isFollowing = false;
        }
    }

    private void Follow () {

        Vector3 targ = new Vector3();
        if ( m_followMouse ) {
            targ = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        } else {
            if ( m_target != null ) {
                targ = m_target.position;
            } else {
                Debug.LogWarning( "No Follow Object Detected" );
            }
        }

        if ( m_offset ) {
            targ += m_tempOffset; 
        }
        if ( m_freeDir.x <= 0 ) {
            targ.x = transform.position.x;
        }
        if ( m_freeDir.y <= 0 ) {
            targ.y = transform.position.y;
        }
        if ( m_freeDir.z <= 0 ) {
            targ.z = transform.position.z;
        }
        Vector3 v3 = new Vector3( targ.x, targ.y, transform.position.z );
        if ( m_isDamp ) {
            //float s = 1/m_dampSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp( transform.position, v3, m_dampSpeed);
        } else {
            transform.position = v3;
        }
    }
}
