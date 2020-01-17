using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AnimationTimers : MonoBehaviour
{
    public bool m_playOnStart;
    public float m_secondsToWait;
    public UnityEvent m_onTimeUp;

    private void Start () {

    }

    public void OnStartPlay () {
        if ( m_playOnStart ) {
            StartCoroutine( Countdown() );
        }
    }

    public void OnScriptPlay () {
        if ( !m_playOnStart ) {
            StartCoroutine( Countdown() );
        }
    }

    private IEnumerator Countdown () {
        float t = m_secondsToWait;
        while ( t > 0 ) {
            t -= Time.deltaTime;
            yield return null;
        }
        m_onTimeUp.Invoke();
    }

}
