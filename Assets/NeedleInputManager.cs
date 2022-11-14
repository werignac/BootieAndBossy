using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NeedleInputManager : MonoBehaviour
{
	
	[SerializeField]
	private InputAction thrustAction;
	[SerializeField]
	private InputAction torqueAction;

	private void Start()
	{
		thrustAction.Enable();
		torqueAction.Enable();
	}

	private void OnDestroy()
	{
		thrustAction.Disable();
		torqueAction.Disable();
	}

	private float GetThrust()
	{
		return thrustAction.ReadValue<float>();
	}

	private float GetTorque()
	{
		return torqueAction.ReadValue<float>();
	}

    // Update is called once per frame
    void Update()
    {
		float _thrust = GetThrust();
		float _torque = GetTorque();

		var data = new NeedleInputData { thrust = _thrust, torque = _torque };

		SendMessage("ReceiveInput", data, SendMessageOptions.RequireReceiver);
    }
}

public struct NeedleInputData
{
	public float thrust;
	public float torque;
}
