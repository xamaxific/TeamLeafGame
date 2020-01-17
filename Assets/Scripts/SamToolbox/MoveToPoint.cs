using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoint : MonoBehaviour
{
	public Transform m_target;
    //private Vector3 m_origin;
    bool m_isMoving;
	public bool m_isDamp;
	public float m_moveSpeed;
	public AnimationCurve m_animCurve;

    public void StartMove()
	{
		if (!m_isMoving)
        {
			m_isMoving = true;
            StartCoroutine( Move() );
        }
	}

	public void EndMove()
	{
		if (m_isMoving)
		{
            StopAllCoroutines();
            m_isMoving = false;
		}
	}

    private IEnumerator Move () {
        Debug.Log( "start move" );
        Vector3 orig = transform.position;
        Vector3 targ = new Vector3( m_target.position.x, m_target.position.y, orig.z );
        float t = 0;
        while ( t < 1 ) {
            
            if ( m_isDamp ) {
                transform.position = Vector3.Lerp( orig, targ, m_animCurve.Evaluate( t ) );
            } else {
                transform.position = Vector3.Lerp( orig, targ, t );
            }
            t += Time.deltaTime * 1 / m_moveSpeed;
            yield return null;
                 
        }
        m_isMoving = false;
    }
}
