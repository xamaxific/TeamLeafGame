using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
[RequireComponent( typeof(LineRenderer) )]
public class CurvedLineRenderer : MonoBehaviour 
{
    //PUBLIC
    public bool m_update = false;
	public float lineSegmentSize = 0.15f;
    [Range(2,50)]
    public int m_splinePoints = 2;
	public float lineWidth = 0.1f;
	[Header("Gizmos")]
	public bool showGizmos = true;
	public float gizmoSize = 0.1f;
	public Color gizmoColor = new Color(1,0,0,0.5f);
    public BezierSpline m_spline;
    //PRIVATE
    private CurvedLinePoint[] linePoints = new CurvedLinePoint[ 0 ];
    private int m_splineLength;
	private Vector3[] linePositions = new Vector3[0];
	private Vector3[] linePositionsOld = new Vector3[0];

	// Update is called once per frame
	public void Update () 
	{
        if ( m_update ) {
            GetPoints();
            SetPointsToLine();
            m_update = false;
        }
	}

	void GetPoints()
	{
        //Debug.Log( "yay" );
        //find curved points in children
        linePoints = this.GetComponentsInChildren<CurvedLinePoint>();
        //LineRenderer line = this.GetComponent<LineRenderer>();
        m_splineLength = m_spline.ControlPointCount;
        //add positions
        //linePositions = new Vector3[ linePoints.Length ];
        //for ( int i = 0; i < linePoints.Length; i++ ) {
        //    linePositions[ i ] = linePoints[ i ].transform.position;
        //}
        linePositions = new Vector3[ m_splineLength ];
        for ( int i = 0; i < m_splineLength; i++ ) {
            linePositions[ i ] = m_spline.GetControlPoint( i );
        }
    }

	void SetPointsToLine()
	{
		//create old positions if they dont match
		if( linePositionsOld.Length != linePositions.Length )
		{
			linePositionsOld = new Vector3[linePositions.Length];
		}

		//check if line points have moved
		//bool moved = false;
		for( int i = 0; i < linePositions.Length; i++ )
		{
			//compare
			if( linePositions[i] != linePositionsOld[i] )
			{
                Debug.Log( "yay" );
                //moved = true;
			}
		}

        //update if moved
        //if( moved == true )
        //{
        //	LineRenderer line = this.GetComponent<LineRenderer>();

        //          //get smoothed values
        //          //Vector3[] smoothedPoints = LineSmoother.SmoothLine( linePositions, lineSegmentSize );
        //          Vector3[] smoothedPoints = new Vector3[ m_splinePoints ];
        //          for ( int i = 0; i < m_splinePoints; i++ ) {
        //              float ev = i / m_splinePoints;
        //              Debug.Log( m_spline.GetPoint( ev ) );
        //              smoothedPoints[ i ] = m_spline.GetPoint( ev );
        //          }
        //          //set line settings
        //          line.positionCount = smoothedPoints.Length;
        //          //line.SetVertexCount( smoothedPoints.Length );
        //	line.SetPositions( smoothedPoints );
        //          line.startWidth = lineWidth;
        //          line.endWidth = lineWidth;
        //	//line.SetWidth( lineWidth, lineWidth );
        //}
        LineRenderer line = this.GetComponent<LineRenderer>();

        //get smoothed values
        //Vector3[] smoothedPoints = LineSmoother.SmoothLine( linePositions, lineSegmentSize );
        Vector3[] smoothedPoints = new Vector3[ m_splinePoints ];
        for ( int i = 0; i < m_splinePoints; i++ ) {
            float ev = (float)i / (m_splinePoints - 1);
            Debug.Log( ev + "|" + m_spline.GetPoint( ev ) );
            smoothedPoints[ i ] =  m_spline.GetPoint( ev ) - transform.position;
        }
        //set line settings
        line.positionCount = smoothedPoints.Length;
        //line.SetVertexCount( smoothedPoints.Length );
        line.SetPositions( smoothedPoints );
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        //line.SetWidth( lineWidth, lineWidth );
    }

    void OnDrawGizmosSelected()
	{
		//Update();
	}

	void OnDrawGizmos()
	{
        //if( linePoints.Length == 0 )
        //{
        //	GetPoints();
        //}

        //if ( m_splineLength == 0 ) {
        //    GetPoints();
        //}
        //settings for gizmos
        //foreach ( CurvedLinePoint linePoint in linePoints ) {
        //    linePoint.showGizmo = showGizmos;
        //    linePoint.gizmoSize = gizmoSize;
        //    linePoint.gizmoColor = gizmoColor;
        //}
    }
}
