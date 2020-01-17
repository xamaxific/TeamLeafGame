using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof( LineRenderer ) )]
public class LineMaker : MonoBehaviour {
    //PUBLIC
    //public float lineSegmentSize = 0.15f;
    //public float lineWidth = 0.1f;
    [Header( "Gizmos" )]
    public bool showGizmos = true;
    public float gizmoSize = 0.1f;
    public Color gizmoColor = new Color( 1, 0, 0, 0.5f );
    //PRIVATE
    [HideInInspector]public WayPoint[] linePoints = new WayPoint[ 0 ];
    private Vector3[] linePositions = new Vector3[ 0 ];
    private Vector3[] linePositionsOld = new Vector3[ 0 ];
    private bool m_disabled;
    // Update is called once per frame
    public void Awake () {
        m_disabled = true;
    }

    public void Update () {
        if ( !m_disabled ) {
            GetPoints();
            SetPointsToLine();
        }
    }

    void GetPoints () {
        //find curved points in children
        linePoints = this.GetComponentsInChildren<WayPoint>();

        //add positions
        linePositions = new Vector3[ linePoints.Length ];
        for ( int i = 0; i < linePoints.Length; i++ ) {
            linePositions[ i ] = linePoints[ i ].transform.localPosition;
        }
    }

    void SetPointsToLine () {
        //create old positions if they dont match
        if ( linePositionsOld.Length != linePositions.Length ) {
            linePositionsOld = new Vector3[ linePositions.Length ];
        }

        //check if line points have moved
        bool moved = false;
        for ( int i = 0; i < linePositions.Length; i++ ) {
            //compare
            if ( linePositions[ i ] != linePositionsOld[ i ] ) {
                moved = true;
            }
        }

        //update if moved
        if ( moved == true ) {
            LineRenderer line = this.GetComponent<LineRenderer>();

            //get smoothed values
            //Vector3[] smoothedPoints = LineSmoother.SmoothLine( linePositions, lineSegmentSize );
            Vector3[] smoothedPoints = linePositions;
            //set line settings
            //line.SetVertexCount( smoothedPoints.Length );
            line.positionCount = smoothedPoints.Length;
            line.SetPositions( smoothedPoints );
            //line.SetWidth( lineWidth, lineWidth );
            //line.startWidth = lineWidth;
            //line.endWidth = lineWidth;
        }
    }

    void OnDrawGizmosSelected () {
        if ( !m_disabled ) {
            Update();
        }
    }

    void OnDrawGizmos () {
        if ( !m_disabled ) {
            if ( linePoints.Length == 0 ) {
                GetPoints();
            }

            //settings for gizmos
            foreach ( WayPoint linePoint in linePoints ) {
                linePoint.showGizmo = showGizmos;
                linePoint.gizmoSize = gizmoSize;
                linePoint.gizmoColor = gizmoColor;
            }
        }
    }
}
