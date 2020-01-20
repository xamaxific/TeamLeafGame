using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonActionScript : MonoBehaviour
{
    public KeyCode m_keyboardKey;
    public UnityEvent m_onKeyDown;
    //protected bool m_isDown;
    public float m_waitTime = 0f;
    private bool m_isWaiting;
    public void Update () {
        if ( GameController.instance.m_GameState == GameController.GameState.Playing ) {
            if ( !m_isWaiting ) {
                if ( Input.GetKeyDown( m_keyboardKey ) ) {
                    //m_isDown = true;
                    if ( m_waitTime > 0 ) {
                        m_isWaiting = true;
                        StartCoroutine( WaitTimer() );
                    }
                    m_onKeyDown.Invoke();
                }
                
            }
        }
    }

    private IEnumerator WaitTimer () {
        float t = 0;
        while ( t < m_waitTime ) {
            t += Time.deltaTime;
            yield return null;
        }
        m_isWaiting = false;
    }
}
