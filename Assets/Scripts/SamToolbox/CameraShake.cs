using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool m_isShake;
    public float m_shakeAmount;
    public float m_shakeSpeed = 1f;
    private float m_lerpTime;
    private bool m_isAnimating;

    private void Update () {
        if ( m_isShake ) {
            if ( !m_isAnimating ) {
                m_isAnimating = true;
                StartCoroutine( Shake());
            }
            if ( m_lerpTime < 1 ) {
                m_lerpTime += Time.deltaTime;
            }
        } else {
            if ( m_lerpTime >= 1 ) {
                m_lerpTime -= Time.deltaTime;
            }
        }

    }

    public void DoShake (bool _shake) {
        m_isShake = _shake;
    }

    private IEnumerator Shake () {
        Vector3 orig = transform.localPosition;
        Vector3 targ = new Vector3( Random.Range( -m_shakeAmount, m_shakeAmount ), Random.Range( -m_shakeAmount, m_shakeAmount ), transform.localPosition.z );
        float t = 0;
        while ( t < 1 ) {
            t += Time.deltaTime * 1 / m_shakeSpeed;
            Vector3 targ2 = new Vector3( targ.x * m_lerpTime, targ.y * m_lerpTime, targ.z );
            transform.localPosition = Vector3.Lerp( orig, targ2 , t );
            yield return null;
        }
        if ( m_isShake ) {
            StartCoroutine( Shake() );
        } else {
            m_isAnimating = false;
        }
    }

}
