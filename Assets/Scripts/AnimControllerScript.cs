using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class AnimControllerScript : MonoBehaviour
{

    public Rigidbody m_rb;
    private Animator m_animator;

    private void Start () {
        m_animator = GetComponent<Animator>();
    }

    private void FixedUpdate () {
        float v = m_rb.velocity.magnitude;
        Debug.Log( v );
        m_animator.SetFloat( "IdleWalk", v );
    }

}
