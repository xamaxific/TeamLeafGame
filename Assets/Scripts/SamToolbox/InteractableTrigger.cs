using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InteractableTrigger : MonoBehaviour
{
    public InteractableTrigger m_parentTrigger;
    public UnityEvent m_onInteracted;
    public UnityEvent m_onMouseUp;

    protected bool m_isInteracting;

    public virtual void OnTriggerDown () {
        m_isInteracting = true;
        if ( m_parentTrigger != null ) {
            m_parentTrigger.OnTriggerDown();
        }
        m_onInteracted.Invoke();
    }

    public virtual void OnTriggerUp () {
        m_isInteracting = false;
        if ( m_parentTrigger != null ) {
            m_parentTrigger.OnTriggerUp();
        }
        m_onMouseUp.Invoke();
    }

}
