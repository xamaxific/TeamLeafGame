using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InteractableObject : MonoBehaviour
{
    public GameObject m_uiThing;
    public TextMeshProUGUI m_myName;
    public string m_objectName;
    [Header( "Age" )]
    public ObjAge[] m_ageTags;
    [Header( "Shape" )]
    public ObjShape[] m_shapeTags;
    [Header( "Texture" )]
    public ObjTexture[] m_textureTags;
    public bool m_isChewed;
    public int m_chewScore;

    private void Start () {
        if ( m_objectName == "" ) {
            m_objectName = gameObject.name;
        }
        m_myName.text = m_objectName;
        m_uiThing.SetActive( false );
    }

    public void ShowUI () {
        m_uiThing.SetActive( true );
    }

    public void HideUI () {
        m_uiThing.SetActive( false );
    }

    public void Chew ( int _chew ) {
        m_chewScore += _chew;
        if ( m_chewScore > 100 ) {
            m_isChewed = true;
        }
    }

}
