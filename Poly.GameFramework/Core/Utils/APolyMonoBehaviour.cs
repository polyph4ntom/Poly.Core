using UnityEngine;

namespace Poly.GameFramework.Core
{
    public class APolyMonoBehaviour : MonoBehaviour
    {
        protected APolyWorldBase world;

        protected internal virtual void InitializeComponent(APolyWorldBase inWorld)
        {
            this.world = inWorld;
        }
        
        protected internal virtual void PostInitializeComponent(APolyWorldBase inWorld)
        {
            
        }

        protected internal virtual void DisposeComponent(APolyWorldBase inWorld)
        {
            
        }

        public TWorld GetWorld<TWorld>() where TWorld : APolyWorldBase
        {
            return (TWorld) world;
        }
    }
}
