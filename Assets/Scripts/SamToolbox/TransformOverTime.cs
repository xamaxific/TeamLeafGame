using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformOverTime : MonoBehaviour
{
    public Vector3 m_moveAmount;
    public Vector3 m_rotateAmount;
    public bool m_isTransforming;

    private void FixedUpdate () {
        if ( m_isTransforming ) {
            float x = m_rotateAmount.x * Time.deltaTime;
            float y = m_rotateAmount.y * Time.deltaTime;
            float z = m_rotateAmount.z * Time.deltaTime;
            transform.rotation = transform.rotation * Quaternion.Euler( new Vector3(x,y,z) );
            x = m_moveAmount.x * Time.deltaTime;
            y = m_moveAmount.y * Time.deltaTime;
            z = m_moveAmount.z * Time.deltaTime;
            transform.position = transform.position + new Vector3( x, y, z );
        }
    }
}
