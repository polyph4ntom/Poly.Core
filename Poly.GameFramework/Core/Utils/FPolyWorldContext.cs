namespace Poly.GameFramework.Core
{
    public sealed class FPolyWorldContext
    {
        private APolyWorldBase currentWorld;
        public string ScenePath { get; private set; }

        internal FPolyWorldContext(string scenePath)
        {
            ScenePath = scenePath;
            currentWorld = null;
        }

        internal void SetCurrentWorld(APolyWorldBase inWorld)
        {
            currentWorld = inWorld;
        }

        internal void Invalidate()
        {
            if (currentWorld == null)
            {
                return;
            }
            
            currentWorld.Invalidate();
            currentWorld = null;

            ScenePath = string.Empty;
        }
    }
}