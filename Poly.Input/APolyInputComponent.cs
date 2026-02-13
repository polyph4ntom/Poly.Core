using System;
using System.Collections.Generic;
using Poly.GameFramework.Core;
using UnityEngine.InputSystem;

namespace Poly.Input
{
	public class APolyInputComponent : APolyMonoBehaviour
	{
		public const string TAP_ACTION = "Tap";
		public const string HOLD_ACTION = "Hold";
		public const string MOVE_ACTION = "Move";

		private class PolyInputBinding
		{
			public string actionName;
			public InputActionPhase phase;
			public Action<InputAction.CallbackContext> callback;
		}

		private List<PolyInputBinding> bindings = new();

		public void Bind(string actionName, InputActionPhase phase, Action<InputAction.CallbackContext> callback)
		{
			bindings.Add(new PolyInputBinding
			{
				actionName = actionName,
				phase = phase,
				callback = callback
			});
		}

		public void Unbind(string actionName, InputActionPhase phase, Action<InputAction.CallbackContext> callback)
		{
			bindings.RemoveAll(b => b.actionName == actionName && b.phase == phase && b.callback == callback);
		}

		public void ProcessInput(InputAction.CallbackContext context)
		{
			foreach (var binding in bindings)
			{
				if (binding.actionName == context.action.name && binding.phase == context.phase)
				{
					binding.callback?.Invoke(context);
				}
			}
		}
	}
}