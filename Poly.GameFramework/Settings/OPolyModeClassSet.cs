using Poly.Common;
using Poly.Loop;
using Poly.UI;
using UnityEngine;

namespace Poly.GameFramework
{
	[CreateAssetMenu(menuName = "Polyphantom/Core/New Mode Settings")]
	public class OPolyModeClassSet : ScriptableObject
	{
		[field: SerializeField,  FPolySubclassPicker(typeof(FPolyModelRegistrar))]
		public FPolyTypeReference ModelsRegistrar { get; private set; }
		
		[field: SerializeField, FPolySubclassPicker(typeof(FPolyLoopState))]
		public FPolyTypeReference EntryState { get; private set; }
	}
}
