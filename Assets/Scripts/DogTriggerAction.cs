using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DogTriggerAction : MonoBehaviour
{
    public LayerMask m_ntrcMask;
    public float m_castSize = 0.5f;
    public float m_castDist = 0.5f;
    public Camera m_cam;
    public bool m_isInRange;
    public BoxCollider m_mouthCheck;

    public TextMeshProUGUI m_like1;
    public TextMeshProUGUI m_like2;
    public TextMeshProUGUI m_like3;
    public TextMeshProUGUI m_chewedCountText;
    public TextMeshProUGUI m_endGameCount;
    public GameObject m_heartSpawn;
    public GameObject m_sadSpawn;
    public GameObject m_munchPart;
    public int m_objectChewCount;

    private ObjAge m_likeAge;
    private ObjShape m_likeShape;
    private ObjTexture m_likeTexture;

    private InteractableObject m_hitObject;
    //private bool m_retriggerCheck;
    private void Start () {
        m_cam = Camera.main;
        SetLikeParams();
    }

    private void SetLikeParams () {
        //Age
        Array age = Enum.GetValues( typeof( ObjAge ) );
        System.Random random = new System.Random();
        m_likeAge = ( ObjAge ) age.GetValue( random.Next( age.Length ) );
        m_like1.text = "the " + m_likeAge.ToString() + ",";
        //Shape
        Array shape = Enum.GetValues( typeof( ObjShape ) );
        random = new System.Random();
        m_likeShape = ( ObjShape ) shape.GetValue( random.Next( shape.Length ) );
        m_like2.text = "and the " + m_likeShape.ToString() + ",";
        //Texture
        Array texture = Enum.GetValues( typeof( ObjTexture ) );
        random = new System.Random();
        m_likeTexture = ( ObjTexture ) texture.GetValue( random.Next( texture.Length ) );
        m_like3.text = "and the " + m_likeTexture.ToString();
    }

    private void OnTriggerEnter ( Collider other ) {
        if ( other.tag == "InteractableObject" ) {
            if ( m_isInRange == false ) {
                m_hitObject = other.GetComponent<InteractableObject>();
                m_hitObject.ShowUI();
                m_isInRange = true;
            } else {
                if ( other.gameObject != m_hitObject.gameObject ) {
                    m_hitObject.HideUI();
                    m_hitObject = other.GetComponent<InteractableObject>();
                    m_hitObject.ShowUI();
                    m_isInRange = true;
                }
            }

        }

    }

    private void OnTriggerExit ( Collider other ) {
        if ( m_isInRange == true ) {
            if ( other.gameObject == m_hitObject.gameObject ) {
                m_hitObject.HideUI();
                m_hitObject = null;
                m_isInRange = false;

            }
        }
    }

    public void Interact () {
        
        if ( m_isInRange ) {
            Instantiate( m_munchPart, transform.position + new Vector3 ( 0, 0.092f, -0.046f), transform.rotation );
            if ( !m_hitObject.m_isChewed ) {
                int count = 0;
                for ( int i = 0; i < m_hitObject.m_ageTags.Length; i++ ) {
                    if ( m_likeAge == m_hitObject.m_ageTags[ i ] ) {
                        count++;
                        break;
                    }
                }
                for ( int i = 0; i < m_hitObject.m_shapeTags.Length; i++ ) {
                    if ( m_likeShape == m_hitObject.m_shapeTags[ i ] ) {
                        count++;
                        break;
                    }
                }
                for ( int i = 0; i < m_hitObject.m_textureTags.Length; i++ ) {
                    if ( m_likeTexture == m_hitObject.m_textureTags[ i ] ) {
                        count++;
                        break;
                    }
                }
                switch ( count ) {
                    case 0:
                        GameObject spawn = Instantiate( m_sadSpawn, transform.position, transform.rotation ) as GameObject;
                        break;
                    case 1:
                        m_hitObject.Chew( 5 );
                        GameObject spawn1 = Instantiate( m_heartSpawn, transform.position, transform.rotation ) as GameObject;
                        ParticleSystem.MainModule mainPart1 = spawn1.GetComponent<ParticleSystem>().main;
                        mainPart1.maxParticles = 1;
                        break;
                    case 2:
                        m_hitObject.Chew( 10 );
                        GameObject spawn2 = Instantiate( m_heartSpawn, transform.position, transform.rotation ) as GameObject;
                        ParticleSystem.MainModule mainPart2 = spawn2.GetComponent<ParticleSystem>().main;
                        mainPart2.maxParticles = 3;
                        break;
                    default:
                        if ( count > 2 ) {
                            m_hitObject.Chew( 20 );
                            GameObject spawn3 = Instantiate( m_heartSpawn, transform.position, transform.rotation ) as GameObject;
                            ParticleSystem.MainModule mainPart3 = spawn3.GetComponent<ParticleSystem>().main;
                            mainPart3.maxParticles = 10;
                        }
                        break;
                }
                if ( m_hitObject.m_isChewed ) {
                    m_objectChewCount++;
                    m_chewedCountText.text = m_objectChewCount.ToString();
                    m_endGameCount.text = m_objectChewCount.ToString();
                }

            } else {

            }
        }
    }
}
