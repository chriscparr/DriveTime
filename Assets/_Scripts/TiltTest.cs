using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltTest : MonoBehaviour 
{	
	private Gyroscope m_gyro;

	private void Start()
	{
		m_gyro = Input.gyro;
		m_gyro.enabled = true;
	}

	void Update () 
	{
		transform.rotation = GyroCorrection(m_gyro.attitude);
		//transform.Rotate(Input.acceleration.x, 0, -Input.acceleration.z);
		//transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);
	}

	private Quaternion GyroCorrection(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);
	}


	/*
	void Update () 
	{

		transform.Rotate(Input.acceleration.x, 0, -Input.acceleration.z);
		//transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);
	}
	*/
}
