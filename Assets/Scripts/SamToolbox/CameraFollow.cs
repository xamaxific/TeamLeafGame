using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CameraFollow : MonoBehaviour
{
    public Camera m_camera;
    public KeyCode m_toggleToFollow;
    public Transform m_target;
    public CameraSettings[] m_camSettings;
    Vector3 offset;
    public bool m_follow;
    bool m_isChanging;
    void Update()
    {
        if ( Input.GetKey( m_toggleToFollow ) ) {
            m_follow = !m_follow;
        }
        if ( m_target != null ) {
            if ( m_follow ) {
                transform.position = m_target.position + offset;
            } else {
                offset = transform.position - m_target.position;
            }
        }
    }

    public void ToggleToFollow (bool _b) {
        m_follow = _b;
    }

    public void UpdateSize (int _setting) {
        if ( !m_isChanging ) {
            m_isChanging = true;
            StartCoroutine( ChangeCamSize(_setting) );
        }
    }

    IEnumerator ChangeCamSize (int _setting) {
        float origSize = m_camera.orthographicSize;
        float targSize = m_camSettings[ _setting ].m_changeSize;
        float t = 0;
        while ( t < 1 ) {
            m_camera.orthographicSize = Mathf.Lerp( origSize, targSize, t);
            t += Time.deltaTime * 1 / m_camSettings[ _setting ].m_changeSpeed;
            yield return null;
        }
        m_isChanging = false;
    }

    [Serializable]
    public struct CameraSettings {
        public float m_changeSize;
        public float m_changeSpeed;
    }

}
