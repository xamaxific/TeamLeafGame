using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTransform : MonoBehaviour
{
    public bool m_fixedPosition;
    public Vector3 m_position;
    public bool m_fixedRotation;
    public Vector3 m_rotation;
    //public bool m_fixedScale;
    //public Vector3 m_scale;

    private void Update () {
        if ( m_fixedPosition ) {
            transform.position = m_position;
        }
        if ( m_fixedRotation ) {
            transform.rotation = Quaternion.Euler(m_rotation);
        }
        //if ( m_fixedScale ) {
        //    transform.lossyScale = m_scale;
        //}
    }


}
