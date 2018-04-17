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
		
		public void UpdateSpeedText(float a_speed)
		{
			int rounded = Mathf.RoundToInt (a_speed);
			m_speedText.text = rounded.ToString() + " MPH";
		}
		
		public void UpdatePositionText(int a_position)
		{
			//later...
		}
		
	}

}


