using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace drivetime.vehicles
{
	[System.Serializable]
	public class Axle
	{
		public GameObject leftWheel;
		public GameObject rightWheel;
		public WheelCollider leftWheelCollider;
		public WheelCollider rightWheelCollider;
		public bool applyMotor;
		public bool applySteering;
	}
}
