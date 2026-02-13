namespace Poly.Save
{
    public interface IPolySaveable
    {
        string SaveID { get; }
        object CaptureState();
        void RestoreState(object state);
    }
}
