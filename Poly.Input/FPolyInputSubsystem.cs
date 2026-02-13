using System.Collections.Generic;
using System.Threading.Tasks;
using Poly.Common;
using Poly.GameFramework.Core;
using Poly.Log;
using UnityEngine.InputSystem;

namespace Poly.Input
{
    public class FPolyInputSubsystem : FPolyLocalPlayerSubsystem
    {
        public const string DEFAULT_ACTION_MAP = "Default";
        
        private readonly List<APolyInputComponent> registeredComponents = new();
        private readonly PolyInputActions inputActions = new();

        private bool isLocked = true;

        protected override Task InitializeSubsystem()
        {
#if UNITY_IOS || UNITY_ANDROID
            inputActions.bindingMask = new InputBinding { groups = "Mobile" };
#else
            inputActions.bindingMask = new InputBinding { groups = "PC" };
#endif
            RegisterInputCallbacks();
            EnableMap(DEFAULT_ACTION_MAP);
            return base.InitializeSubsystem();
        }

        protected override void DisposeSubsystem()
        {
            UnregisterInputCallbacks();
            registeredComponents.Clear();
            base.DisposeSubsystem();
        }

        public void LockInput()
        {
            isLocked = true;
        }

        public void UnlockInput()
        {
            isLocked = false;
        }

        private void RegisterInputCallbacks()
        {
            foreach (var action in inputActions.Default.Get())
            {
                action.performed += ForwardInput;
                action.started += ForwardInput;
                action.canceled += ForwardInput;
            }
        }
        
        private void UnregisterInputCallbacks()
        {
            foreach (var action in inputActions.Default.Get())
            {
                action.performed -= ForwardInput;
                action.started -= ForwardInput;
                action.canceled -= ForwardInput;
            }
        }

        private void ForwardInput(InputAction.CallbackContext context)
        {
            if (isLocked)
            {
                return;
            }

            foreach (var cmp in registeredComponents)
            {
                cmp.ProcessInput(context);
            }
        }

        public void RegisterInputComponent(APolyInputComponent component)
        {
            if (registeredComponents.Contains(component))
            {
                FPolyLog.Error("Poly.Input", $"Input Component of {component.gameObject.name} is already registered.");
                return;
            }

            registeredComponents.Add(component);
        }

        public void UnregisterInputComponent(APolyInputComponent component)
        {
            if (!registeredComponents.Contains(component))
            {
                FPolyLog.Error("Poly.Input", $"Input Component of {component.gameObject.name} is not registered.");
                return;
            }

            registeredComponents.Remove(component);
        }

        public void EnableMap(string mapId)
        {
            var map = inputActions.asset.FindActionMap(mapId, true);
            map?.Enable();
        }

        public void DisableMap(string mapId)
        {
            var map = inputActions.asset.FindActionMap(mapId, true);
            map?.Disable();
        }
    }
}
