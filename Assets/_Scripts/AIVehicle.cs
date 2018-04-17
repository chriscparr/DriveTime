using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace drivetime.vehicles
{
	public class AIVehicle : MonoBehaviour 
	{
		[SerializeField]
		private BankedTrack m_track;	//some game manager / car spawner will provide this in future.
		[SerializeField]
		private NavMeshAgent m_agent;

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
		private Transform m_agentTransform;
		private int m_waypointIndex = 0;
		private Vector3[] m_waypoints;

		private float m_power;
		private float m_brake;
		private float m_steer;

		private void Awake()
		{
			m_carRigidBody = GetComponent<Rigidbody>();
			m_carRigidBody.centerOfMass = m_centerOfMass.transform.localPosition;
			m_agentTransform = m_agent.gameObject.transform;
			m_agentTransform.localPosition = Vector3.zero;
			m_agentTransform.localRotation = Quaternion.Euler (Vector3.zero);
		}

		private void Start()
		{
			m_waypoints = m_track.Waypoints;
			m_agent.destination = m_waypoints [0];
		}

		private void FixedUpdate ()
		{
			m_brake = m_carRigidBody.mass * m_brakingCoefficient;
			m_power = 0f;
			m_steer = 0f;

			Vector3 navDirection = transform.InverseTransformDirection(m_agent.desiredVelocity);			
			Vector3 forwardWorldPoint = transform.TransformPoint (transform.position + (transform.forward * 2f));

			float horizontalNav = Mathf.Clamp (navDirection.x, -1f, 1f);
			float verticalNav = Mathf.Clamp (navDirection.z, -1f, 1f);

			m_agentTransform.localPosition = Vector3.zero;
			m_agentTransform.localRotation = Quaternion.Euler (Vector3.zero);

			m_power = verticalNav * m_enginePower * Time.fixedDeltaTime * 250f;
			m_steer = horizontalNav * m_steerMaxAngle;

			m_axles[0].leftWheelCollider.steerAngle = m_steer;
			m_axles[0].rightWheelCollider.steerAngle = m_steer;

			if (m_axles [1].leftWheelCollider.rpm > 0f)	//we're moving forward
			{
				if (verticalNav >= 0f)
				{
					m_axles [0].leftWheelCollider.brakeTorque = 0f;
					m_axles [0].rightWheelCollider.brakeTorque = 0f;
					m_axles [1].leftWheelCollider.brakeTorque = 0f;
					m_axles [1].rightWheelCollider.brakeTorque = 0f;
					m_axles [1].leftWheelCollider.motorTorque = m_power;
					m_axles [1].rightWheelCollider.motorTorque = m_power;
				}
				if (verticalNav < 0f)
				{
					m_axles [0].leftWheelCollider.brakeTorque = m_brake;
					m_axles [0].rightWheelCollider.brakeTorque = m_brake;
					m_axles [1].leftWheelCollider.brakeTorque = m_brake;
					m_axles [1].rightWheelCollider.brakeTorque = m_brake;
					m_axles [1].leftWheelCollider.motorTorque = 0f;
					m_axles [1].rightWheelCollider.motorTorque = 0f;
				}

			}
			if (m_axles [1].leftWheelCollider.rpm < 0f)	//we're moving backwards
			{
				if (verticalNav <= 0f)
				{
					m_axles [0].leftWheelCollider.brakeTorque = 0f;
					m_axles [0].rightWheelCollider.brakeTorque = 0f;
					m_axles [1].leftWheelCollider.brakeTorque = 0f;
					m_axles [1].rightWheelCollider.brakeTorque = 0f;
					m_axles [1].leftWheelCollider.motorTorque = m_power;
					m_axles [1].rightWheelCollider.motorTorque = m_power;
				}
				if (verticalNav > 0f)
				{
					m_axles [0].leftWheelCollider.brakeTorque = m_brake;
					m_axles [0].rightWheelCollider.brakeTorque = m_brake;
					m_axles [1].leftWheelCollider.brakeTorque = m_brake;
					m_axles [1].rightWheelCollider.brakeTorque = m_brake;
					m_axles [1].leftWheelCollider.motorTorque = 0f;
					m_axles [1].rightWheelCollider.motorTorque = 0f;
				}

			}
			if (m_axles [1].leftWheelCollider.rpm == 0f)	//we're stationary
			{
				m_axles [0].leftWheelCollider.brakeTorque = 0f;
				m_axles [0].rightWheelCollider.brakeTorque = 0f;
				m_axles [1].leftWheelCollider.brakeTorque = 0f;
				m_axles [1].rightWheelCollider.brakeTorque = 0f;
				m_axles[1].leftWheelCollider.motorTorque = m_power;
				m_axles[1].rightWheelCollider.motorTorque = m_power;
			}




			SetWheelTransforms ();

			if (m_agent.remainingDistance < 3f)
			{
				GotoNextPoint();
			}

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

		private void GotoNextPoint()
		{
			m_waypointIndex++;
			Debug.Log ("Now heading to waypoint " + m_waypointIndex.ToString());
			if (m_waypointIndex < m_waypoints.Length)
			{
				m_agent.destination = m_waypoints[m_waypointIndex];
			}
			else
			{
				m_waypointIndex = 0;
				m_agent.destination = m_waypoints[m_waypointIndex];
			}
		}
	}

}
