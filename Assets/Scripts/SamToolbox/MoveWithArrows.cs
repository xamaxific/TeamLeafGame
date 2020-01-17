using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithArrows : MonoBehaviour
{
    public float m_moveSpeed;

	private void Update() {
        float x = Input.GetAxis( "Horizontal" );
        float y = Input.GetAxis( "Vertical" );

        Vector3 v3 = new Vector3( x, y, 0f ) * m_moveSpeed;

        transform.position += v3;

    }
}
