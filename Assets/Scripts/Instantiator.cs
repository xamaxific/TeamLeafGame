using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public GameObject m_prefab;
    [Tooltip("If < 0 then the instantiated object will exist forever")]
    public float m_waitForDestroy;
    public void InstantiateObject () {
        GameObject spawn = Instantiate( m_prefab, transform.position, transform.rotation ) as GameObject;
        if ( m_waitForDestroy >= 0 ) {
            Destroy( spawn, m_waitForDestroy );
        }

    }
}
