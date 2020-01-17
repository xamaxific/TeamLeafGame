using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SmoothFloat
{

	public float Value = 0;
	public float Target = 0;

	public float SmoothTime = 2;

	public float Velocity = 0;

	/*
	SmoothFloat (float initialValue, float smoothTime)
	{
		Value = initialValue;
		Target = initialValue;

		SmoothTime = smoothTime;
	}*/


	public void Update ()
	{
		Value = Mathf.SmoothDamp(Value, Target, ref Velocity, SmoothTime);
	}

	public static implicit operator float (SmoothFloat t)
	{
		return t.Value;
	}
}
