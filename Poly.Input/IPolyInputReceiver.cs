using UnityEngine.InputSystem;

namespace Poly.Input
{
    public interface IPolyInputTapReceiver
    {
        public void OnTapReceived(InputAction.CallbackContext context);
    }

    public interface IPolyInputHoldReceiver
    {
        public void OnHoldReceived(InputAction.CallbackContext context);
    }
}