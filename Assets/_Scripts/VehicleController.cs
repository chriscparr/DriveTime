﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace drivetime.vehicles
{
	public class VehicleController : MonoBehaviour 
	{
		public float VehicleSpeed {get { return m_carRigidBody.velocity.magnitude;}}

		[SerializeField]
		private VehicleHUD m_hud;
		[SerializeField]
		private Axle[] m_axles;
		[SerializeField]
		private float m_enginePower = 150f;
		[SerializeField]
		private float m_steerMaxAngle = 50f;
		[SerializeField]
		private GameObject m_centerOfMass;
		[SerializeField]
		private float m_brakingCoefficient = 0.5f;
		[SerializeField]
		private MeshCollider m_carCollider;

		private Rigidbody m_carRigidBody;

		private float m_power;
		private float m_brake;
		private float m_steer;

		private void Awake()
		{
			m_carRigidBody = GetComponent<Rigidbody>();
			m_carRigidBody.centerOfMass = m_centerOfMass.transform.localPosition;
		}
			
		private void FixedUpdate ()
		{
			m_power = 0f;
			m_brake = 0f;
			m_steer = 0f;

			m_power = Input.GetAxis("Vertical") * m_enginePower * Time.fixedDeltaTime * 250f;
			m_steer = Input.GetAxis("Horizontal") * m_steerMaxAngle;

			m_axles[0].leftWheelCollider.steerAngle = m_steer;
			m_axles[0].rightWheelCollider.steerAngle = m_steer;


			if (Input.GetKey (KeyCode.Space))		//engage brakes
			{
				m_axles [1].leftWheelCollider.motorTorque = 0f;
				m_axles [1].rightWheelCollider.motorTorque = 0f;

				m_brake = m_carRigidBody.mass * m_brakingCoefficient;

				m_axles [0].leftWheelCollider.brakeTorque = m_brake;
				m_axles [0].rightWheelCollider.brakeTorque = m_brake;
				m_axles [1].leftWheelCollider.brakeTorque = m_brake;
				m_axles [1].rightWheelCollider.brakeTorque = m_brake;
			}
			else
			if (Input.GetKeyUp (KeyCode.Space))		//release brakes
			{
				m_axles [0].leftWheelCollider.brakeTorque = 0f;
				m_axles [0].rightWheelCollider.brakeTorque = 0f;
				m_axles [1].leftWheelCollider.brakeTorque = 0f;
				m_axles [1].rightWheelCollider.brakeTorque = 0f;
			}
			else
			{
				m_axles[1].leftWheelCollider.motorTorque = m_power;
				m_axles[1].rightWheelCollider.motorTorque = m_power;
			}
			SetWheelTransforms ();
			m_hud.UpdateSpeedText (VehicleSpeed);
		}

		private void SetWheelTransforms()
		{
			foreach (Axle axl in m_axles)
			{
				Vector3 position;
				Quaternion rotation;
				//left
				axl.leftWheelCollider.GetWorldPose(out position, out rotation);
				axl.leftWheel.transform.SetPositionAndRotation (position, rotation);
				//right
				axl.rightWheelCollider.GetWorldPose(out position, out rotation);
				axl.rightWheel.transform.SetPositionAndRotation (position, rotation);
			}
		}
	}
}
