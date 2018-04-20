using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace drivetime.vehicles
{
	public class VehicleHUD : MonoBehaviour 
	{
		[SerializeField]
		private Text m_positionText;
		[SerializeField]
		private Text m_speedText;

		[SerializeField]
		private Button m_accelBtn;
		[SerializeField]
		private Button m_brakeBtn;
		[SerializeField]
		private Button m_leftBtn;
		[SerializeField]
		private Button m_rightBtn;

		private bool m_accelerate = false;
		private bool m_brake = false;
		private bool m_left = false;
		private bool m_right = false;

		public float AccelerateInput { get { return m_accelerate ? 1f : 0f; }}
		public float BrakeInput { get { return m_brake ? 1f : 0f; }}

		public void UpdateSpeedText(float a_speed)
		{
			int rounded = Mathf.RoundToInt (a_speed);
			m_speedText.text = rounded.ToString() + " MPH";
		}
		
		public void UpdatePositionText(int a_position)
		{
			//later...
		}

		public float GetSteerAmount()
		{
			if (m_right && !m_left)
			{
				return 1f;
			}
			if (m_left && !m_right)
			{
				return -1f;
			}
			return 0f;
		}

		private void OnAcceleratePressed()
		{
			m_accelerate = true;
		}
		private void OnAccelerateReleased()
		{
			m_accelerate = false;
		}
		private void OnBrakePressed()
		{
			m_brake = true;
		}
		private void OnBrakeReleased()
		{
			m_brake = false;
		}
		private void OnLeftPressed()
		{
			m_left = true;
		}
		private void OnLeftReleased()
		{
			m_left = false;
		}
		private void OnRightPressed()
		{
			m_right = true;
		}
		private void OnRightReleased()
		{
			m_right = false;
		}
		
	}

}


