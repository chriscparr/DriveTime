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

		private int m_waypointIndex = 0;
		private Vector3[] m_waypoints;

		private void Start()
		{
			m_waypoints = m_track.Waypoints;
			m_agent.destination = m_waypoints [0];
		}

		private void GotoNextPoint()
		{
			m_waypointIndex++;
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

		private void Update()
		{
			if (m_agent.remainingDistance < 2f)
			{
				GotoNextPoint();
			}
		}
	}

}
