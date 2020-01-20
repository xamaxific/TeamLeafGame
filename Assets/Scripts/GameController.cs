using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.Cameras;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public enum GameState { Title, Playing, End };
    private GameState m_gameState;
    public GameState m_GameState {
        get { return m_gameState; }
    }
    public UnityEvent m_onEnterTitle;
    public UnityEvent m_onEnterStart;
    public UnityEvent m_onEnterEnd;
    public Slider m_timerSlider;
    public float m_secondsTilEnd;
    public ThirdPersonUserControl m_tpuc;
    public FreeLookCam m_flc;
    private void Awake () {
        if ( instance == null ) {
            instance = this;
        }
    }

    private void Start () {
        m_gameState = GameState.End;
        EnterTitle();
    }

    public void EnterTitle () {
        if ( m_gameState != GameState.Title ) {
            m_onEnterTitle.Invoke();
            m_flc.LockCursor( false );
            m_gameState = GameState.Title;
        }
    }

    public void EnterStart () {
        if ( m_gameState != GameState.Playing ) {

            m_onEnterStart.Invoke();
            m_flc.LockCursor( true );
            m_flc.m_isPlaying = true;
            m_tpuc.m_isPlaying = true;
            StartCoroutine( RunTimer() );
            m_gameState = GameState.Playing;
        }
    }

    private IEnumerator RunTimer () {
        float t = m_secondsTilEnd;
        while ( t > 0 ) {
            t -= Time.deltaTime;
            m_timerSlider.value = Mathf.Lerp( 0, 1, t / m_secondsTilEnd );
            yield return null;
            //Debug.Log( t );
        }
        EnterEnd();
    }

    public void EnterEnd () {
        if ( m_gameState != GameState.End ) {
            m_onEnterEnd.Invoke();
            m_flc.m_isPlaying = false;
            m_tpuc.m_isPlaying = false;
            m_flc.LockCursor( false );
            m_gameState = GameState.End;
            Debug.Log( "GameEnded!" );
        }
    }

    public void Restart () {
        SceneManager.LoadScene( 0 );
    }

    public void QuitGame () {
        Application.Quit();
    }
}
