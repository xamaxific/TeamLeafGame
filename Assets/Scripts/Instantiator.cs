using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Instantiator : MonoBehaviour
{
    public enum InstantiateMode { InOrder, Random, AllAtOnce }
    public InstantiateMode m_mode;
    public SpawnProperties[] m_spawnPrefabs;
    public int m_count;

    public void InstantiateObject () {

        switch ( m_mode ) {
            case InstantiateMode.InOrder:
                SpawnObject( m_count );
                m_count++;
                if ( m_count >= m_spawnPrefabs.Length ) {
                    m_count = 0;
                }
                break;
            case InstantiateMode.Random:
                int r = UnityEngine.Random.Range( 0, m_spawnPrefabs.Length );
                SpawnObject( r );
                break;
            case InstantiateMode.AllAtOnce:
                for ( int i = 0; i < m_spawnPrefabs.Length; i++ ) {
                    SpawnObject( i );
                }
                break;
            default:
                break;
        }



    }
    private void SpawnObject (int _i) {
        GameObject spawn = Instantiate( m_spawnPrefabs[_i].m_prefab, transform.position, transform.rotation ) as GameObject;
        float t = m_spawnPrefabs[ _i ].m_waitForDestroy;
        if ( t >= 0 ) {
            Destroy( spawn, t );
        }
    }

    [Serializable]
    public struct SpawnProperties {
        public GameObject m_prefab;
        [Tooltip( "If < 0 then the instantiated object will exist forever" )]
        public float m_waitForDestroy;
    }
}
