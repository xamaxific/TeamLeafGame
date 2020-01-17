using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class FollowWaypoint : MonoBehaviour
{
    public enum AnimMode { Once, Loop }; //, PingPong
    public bool m_playOnStart = true;
    public float m_speed = 1f;
    public bool m_rotateToMoveDir = true;
    public bool m_deleteWaypoint = true;
    public bool m_debugTimeToArrive;
    public AnimMode m_animMode;
    public GameObject m_followObj;
    public MovementProperties[] m_movements;
    private int m_count = 0;
    private bool m_isPlaying;
    private bool m_softStop;
    
    private void Start () {
    }

    public void OnStartPlay () {
        if (!m_isPlaying && m_playOnStart && m_count < m_movements.Length && !m_softStop ) {
            m_playOnStart = false;
            StartCoroutine( StartAnimation() );
            
        }
    }

    public void OnScriptPlay () {
        if ( !m_isPlaying && !m_playOnStart && m_count < m_movements.Length && !m_softStop) {
         
            StartCoroutine( StartAnimation() );
        }
    }

    public void StopAllPlay () {
        m_isPlaying = false;
        StopAllCoroutines();
    }

    public void SoftLock (bool _b) {
        //animations stop at end but still trigger events 
        m_softStop = _b;
    }



    private IEnumerator StartAnimation () {
        m_isPlaying = true;
        //Debug.Log( m_count );
        float m_waitBeforeStart = m_movements[ m_count ].waitBeforeStart;
        float m_speedMultiplier = m_movements[ m_count ].speedMultiplier;
        LineMaker lineMaker = m_movements[ m_count ].line;
        LineRenderer line = lineMaker.GetComponent<LineRenderer>();
        
        //Debug.Log( line.name );
        Transform[] movePoints = new Transform[ lineMaker.linePoints.Length ];
        
        for ( int i = 0; i < movePoints.Length; i++ ) {
            movePoints[i] = lineMaker.linePoints[ i ].transform;
        }
        transform.position = movePoints[ movePoints.Length - 1 ].position;
       int moveCount = movePoints.Length - 2;
        float t = 0;
        //wait before start

        if ( m_movements[ m_count ].waited == false ) {
            while ( t < m_waitBeforeStart ) {
                t += Time.deltaTime;
                yield return null;
            }
        }
        if ( m_movements[ m_count ].waitOnce ) {
            m_movements[ m_count ].waited = true;
        } 
        //begin movement

        while ( moveCount >= 0 ) {
            t = 0;
            Vector3 orig = transform.position;
            Vector3 targ = movePoints[ moveCount ].position;
            if ( m_rotateToMoveDir ) {
                m_followObj.transform.rotation = movePoints[ moveCount ].rotation;
            }
            float dist = Vector3.Distance( orig, targ );
            float time = dist / m_speed;
            if ( m_debugTimeToArrive ) {
                Debug.Log( this.name + " Time: " + time );
            }
            //lerp to point
            while ( t < 1 ) {
                //Debug.Log( t );
                transform.position = Vector3.Lerp( orig, targ, t );
                //m_line.SetPosition( m_line.positionCount - 1, transform.localPosition );
                t += Time.deltaTime * (1 / time)* m_speedMultiplier;
                yield return null;
            }
            //wait if needed.
            if ( m_deleteWaypoint ) {
                Vector3[] linePoss = new Vector3[ line.positionCount - 1 ];
                for ( int i = 0; i < line.positionCount - 1; i++ ) {
                    linePoss[ i ] = line.GetPosition( i );
                }
                line.positionCount = linePoss.Length;
                line.SetPositions( linePoss );
            }
            moveCount -= 1;
        }
        if ( m_count < m_movements.Length ) {
            m_count++;
        }
        m_isPlaying = false;
        //invoke after everything else is done.
        
        m_movements[ m_count - 1 ].onReachDestination.Invoke();
        if ( !m_softStop ) {
            if ( m_count >= m_movements.Length ) {
                switch ( m_animMode ) {
                    default:
                        break;
                    case AnimMode.Loop:
                        if ( !m_deleteWaypoint ) {
                            m_count = 0;
                            OnScriptPlay();
                        }
                        break;
                        //case AnimMode.PingPong:
                        //    break;
                }
            }
        }
    }

    [Serializable]
    public struct MovementProperties {
        public bool waitOnce;
        [HideInInspector] public bool waited;
        public float waitBeforeStart;
        public float speedMultiplier;
        public LineMaker line;
        public UnityEvent onReachDestination;
        //public float speedMultiplier = 1;
    }
}


