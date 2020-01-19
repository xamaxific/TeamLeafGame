using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent( typeof( Rigidbody ) )]
[RequireComponent(typeof(CharacterController))]
public class SamTPController : MonoBehaviour
{
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

    [SerializeField] float m_MovingTurnSpeed = 360;
    [SerializeField] float m_StationaryTurnSpeed = 180;
    [SerializeField] float m_JumpPower = 6f;
    [Range( 1f, 4f )] [SerializeField] float m_GravityMultiplier = 2f;
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    [SerializeField] float m_GroundCheckDistance = 0.1f;

    Rigidbody m_Rigidbody;
    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;
    CharacterController m_cController;
    private void Start () {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_cController = GetComponent<CharacterController>();
        //m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
        // get the transform of the main camera
        if ( Camera.main != null ) {
            m_Cam = Camera.main.transform;
        } else {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject );
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }
    }


    private void Update () {
        if ( !m_Jump ) {
            m_Jump = CrossPlatformInputManager.GetButtonDown( "Jump" );
        }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate () {
        // read inputs
        float h = CrossPlatformInputManager.GetAxis( "Horizontal" );
        float v = CrossPlatformInputManager.GetAxis( "Vertical" );
        bool crouch = false;//Input.GetKey( KeyCode.C );

        // calculate move direction to pass to character
        if ( m_Cam != null ) {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale( m_Cam.forward, new Vector3( 1, 0, 1 ) ).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        } else {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }
#if !MOBILE_INPUT
        // walk speed multiplier
        //if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

        // pass all parameters to the character control script
        Move( m_Move, crouch, m_Jump );
        m_Jump = false;
    }

    public void Move ( Vector3 move, bool crouch, bool jump ) {

        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        
        if ( move.magnitude > 1f ) move.Normalize();
        UpdateAnimator( Vector3.ProjectOnPlane( move, m_GroundNormal ) );
        move = transform.InverseTransformDirection( move );
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane( move, m_GroundNormal );
        
        m_TurnAmount = Mathf.Atan2( move.x, move.z );
        m_ForwardAmount = move.z;

        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if ( m_IsGrounded ) {
            HandleGroundedMovement( crouch, jump );
        } else {
            HandleAirborneMovement();
        }

        // send input and other state parameters to the animator
        
    }
    void UpdateAnimator ( Vector3 move ) {
        m_cController.Move( move * m_MoveSpeedMultiplier );
        //m_Rigidbody.AddForce( move * m_MoveSpeedMultiplier );
    }


    void HandleAirborneMovement () {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = ( Physics.gravity * m_GravityMultiplier ) - Physics.gravity;
        m_Rigidbody.AddForce( extraGravityForce );

        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }


    void HandleGroundedMovement ( bool crouch, bool jump ) {
        // check whether conditions are right to allow a jump:
        if ( jump && !crouch ) //&& m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded")
        {
            // jump!
            m_Rigidbody.velocity = new Vector3( m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z );
            m_IsGrounded = false;
            //m_Animator.applyRootMotion = false;
            m_GroundCheckDistance = 0.1f;
        }
    }

    void ApplyExtraTurnRotation () {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp( m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount );
        transform.Rotate( 0, m_TurnAmount * turnSpeed * Time.deltaTime, 0 );
    }


    void CheckGroundStatus () {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine( transform.position + ( Vector3.up * 0.1f ), transform.position + ( Vector3.up * 0.1f ) + ( Vector3.down * m_GroundCheckDistance ) );
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if ( Physics.Raycast( transform.position + ( Vector3.up * 0.1f ), Vector3.down, out hitInfo, m_GroundCheckDistance ) ) {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
        } else {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
        }
    }
}

