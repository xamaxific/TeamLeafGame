using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GlobalController : MonoBehaviour {
    public int m_sceneToReload;
    public KeyCode m_startKey = KeyCode.Space;
    public KeyCode m_restartKey = KeyCode.R;
    [HideInInspector]public AnimationTimers[] animationTimers;
    [HideInInspector] public FollowWaypoint[] followWaypointScripts;
    public AnimationInfo[] m_manualAnims;
    private bool m_doOnce;
    public enum AnimMode { both, none, showOnly, hideOnly };

    private void Awake () {
        animationTimers = FindObjectsOfType<AnimationTimers>();
        followWaypointScripts = FindObjectsOfType<FollowWaypoint>();
    }

    private void Update () {
        if ( Input.GetKeyDown( m_startKey ) && !m_doOnce ) {
            foreach ( AnimationTimers at in animationTimers ) {
                at.OnStartPlay();
            }
            foreach ( FollowWaypoint cm in followWaypointScripts ) {
                cm.OnStartPlay();
            }
            Debug.Log( "Scene Start" );
            m_doOnce = true;
        }

        if ( Input.GetKeyDown( m_restartKey) ) {
            SceneManager.LoadScene( SceneManager.GetActiveScene().name );
            
        }
        for ( int i = 0; i < m_manualAnims.Length; i++ ) {
            if ( Input.GetKeyDown( m_manualAnims[ i ].m_animKey ) ) {
                if (  m_manualAnims[ i ].aMode ==AnimMode.both || m_manualAnims[i].aMode == AnimMode.showOnly) {
                    foreach ( UIController uc in m_manualAnims[ i ].m_manualAnim ) {
                        if ( uc.gameObject.activeSelf ) {
                            uc.Show();
                        }
                    }
                    m_manualAnims[ i ].m_onShow.Invoke();
                }
            } else if ( Input.GetKeyUp( m_manualAnims[ i ].m_animKey ) ) {
                if ( m_manualAnims[ i ].aMode == AnimMode.both || m_manualAnims[ i ].aMode == AnimMode.hideOnly ) {
                    foreach ( UIController uc in m_manualAnims[ i ].m_manualAnim ) {
                        if ( uc.gameObject.activeSelf ) {
                            uc.Hide();
                        }
                    }
                    m_manualAnims[ i ].m_onHide.Invoke();
                }
            }
        }

    }

    [Serializable]
    public struct AnimationInfo {
        public UIController[] m_manualAnim;
        public UnityEvent m_onShow;
        public UnityEvent m_onHide;
        public AnimMode aMode;
        public KeyCode m_animKey;
        [HideInInspector]public bool isPlay; 
    }
}
