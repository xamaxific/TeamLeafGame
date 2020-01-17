using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(UIController))]
public class MousePointer : MonoBehaviour
{

    public LayerMask m_layerMask;
    public float m_castDistance;
    public Transform m_canvasObject;
    public bool m_canvasSpaceWorld;
    Camera m_cam;

    UIController m_animCtrl;
    bool m_down;
    RectTransform m_rt;
    InteractableTrigger m_tempObj;

    private void Start () {
        m_cam = Camera.main;
    }

    private void Update (){

        if ( Input.GetMouseButtonDown( 0 ) ) {
            if ( !m_down ) {
                ShowPointer();
                m_down = true;
                TraceForAnim();
            }
        } else if(Input.GetMouseButtonUp(0)) {
            if ( m_down ) {
                HidePointer();
                m_down = false;
                Release();
                //TraceForAnim();
            }
        }
        if ( m_canvasSpaceWorld ) {
            m_rt.position = Input.mousePosition;
        } else {
            //Vector3 mouseToWorld = m_canvasObject.position - Camera.main.ScreenToWorldPoint( Input.mousePosition );
            Vector3 mouseToWorld = Input.mousePosition;
            m_rt.localPosition = new Vector3( mouseToWorld.x-Screen.width/2, mouseToWorld.y-Screen.height/2, 0);
        }
        //Debug.Log( m_rt.position );
        //Debug.Log( Input.mousePosition );
        //Vector3 mPoint = Camera.main.ViewportToScreenPoint( Input.mousePosition );
    }

    private void Awake () {
        m_animCtrl = GetComponent<UIController>();
        m_rt = GetComponent<RectTransform>();
        m_down = false;
        HidePointer();
    }

    private void TraceForAnim () {

        Ray ray = m_cam.ScreenPointToRay( Input.mousePosition );
        RaycastHit2D hit2D;
        RaycastHit hit;
        Vector3 mousePos = m_cam.ScreenToWorldPoint( Input.mousePosition );
        if ( hit2D = Physics2D.Raycast( new Vector2( mousePos.x, mousePos.y ), Vector3.zero ) ) {
            
            if ( hit2D.collider.CompareTag( "Interactable" ) ) {
                m_tempObj = hit2D.collider.GetComponent<InteractableTrigger>();
                if ( m_tempObj != null ) {
                    m_tempObj.OnTriggerDown();
                } else {
                    Debug.LogWarning( "No InteractableTriggerScript found!" );
                }
            }
        } else if ( Physics.Raycast( ray, out hit, m_castDistance, m_layerMask ) ) {
            Debug.DrawLine( ray.origin, hit.point, Color.red, 100f );
            if ( hit.collider.CompareTag( "Interactable" ) ) {
                m_tempObj = hit.collider.GetComponent<InteractableTrigger>();
                
                if ( m_tempObj != null ) {
                    m_tempObj.OnTriggerDown();
                } else {
                    Debug.LogWarning( "No InteractableTriggerScript found!" );
                }
            }
        }
    }

    private void Release () {
        if ( m_tempObj != null ) {
            if ( m_tempObj != null ) {
                m_tempObj.OnTriggerUp();
            }
            m_tempObj = null;
        }
    }



    //private void OnEnable () {
    //    ShowPointer();
    //}

    //private void OnDisable () {
    //    HidePointer();
    //}

    public void ShowPointer () {
        m_animCtrl.Show();
    }

    public void HidePointer () {
        m_animCtrl.Hide();
    }
}
