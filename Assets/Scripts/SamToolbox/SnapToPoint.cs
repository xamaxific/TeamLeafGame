using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToPoint : MonoBehaviour
{
    public Transform m_target;

    public void OnSnap () {
        transform.position = m_target.position;
    }

}
