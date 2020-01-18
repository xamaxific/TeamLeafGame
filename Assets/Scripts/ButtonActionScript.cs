using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonActionScript : MonoBehaviour
{
    public KeyCode m_keyboardKey;
    public UnityEvent m_onKeyDown;
    public UnityEvent m_onKeyUp;
    protected bool m_isDown;
    public void Update () {
        if ( Input.GetKeyDown( m_keyboardKey ) ) {
            m_isDown = true;
            m_onKeyDown.Invoke();
        } else if ( Input.GetKeyUp( m_keyboardKey ) ) {
            m_onKeyUp.Invoke();
            m_isDown = false;
        }
    }
}
