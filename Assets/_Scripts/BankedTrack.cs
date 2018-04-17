using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace drivetime
{
	public class BankedTrack : MonoBehaviour 
	{
		[SerializeField]
		private GameObject[] m_waypointObjects;
		
		private Vector3[] m_waypoints;
		
		public Vector3[] Waypoints {get{ return m_waypoints;}}
		
		private void Awake()
		{
			m_waypoints = new Vector3[m_waypointObjects.Length];
			for (int i = 0; i < m_waypointObjects.Length; i++)
			{
				m_waypoints [i] = m_waypointObjects [i].transform.position;
			}
		}
	}
}
