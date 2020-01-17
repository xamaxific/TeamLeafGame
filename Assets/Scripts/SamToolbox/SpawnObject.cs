using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public Vector3 m_spawnRange;
    public float m_spawnTime;
    public GameObject[] m_prefabs;
    public bool m_spawn;
    private void Start () {
        StartCoroutine( Spawn() );
    }

    private IEnumerator Spawn () {
        float t = 0;
        while ( m_spawn ) {
            while ( t < m_spawnTime ) {
                t += Time.deltaTime;
                yield return null;
            }
            float x = Random.Range( -m_spawnRange.x, m_spawnRange.x ) + transform.position.x;
            float y = Random.Range( -m_spawnRange.y, m_spawnRange.y ) + transform.position.y;
            float z = Random.Range( -m_spawnRange.z, m_spawnRange.z ) + transform.position.z;

            Instantiate( m_prefabs[ Random.Range( 0, m_prefabs.Length ) ], new Vector3( x, y, z ), Quaternion.identity );
            t = 0;
        }
    }
}
