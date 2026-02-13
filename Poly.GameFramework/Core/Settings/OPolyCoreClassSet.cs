using System.Collections.Generic;
using Poly.Common;
using UnityEngine;

namespace Poly.GameFramework.Core
{
	[CreateAssetMenu(menuName = "Polyphantom/Core/New Core Class Set")]
	public class OPolyCoreClassSet : ScriptableObject
	{
		[field: SerializeField, FPolySubclassPicker(typeof(FPolyGameInstanceBase))]
		public FPolyTypeReference GameInstance { get; private set; }

		[field: SerializeField, FPolySubclassPicker(typeof(APolyWorldBase))]
		public FPolyTypeReference World { get; private set; }
	
		[field: SerializeField, FPolySubclassPicker(typeof(FPolyLocalPlayerBase))]
		public FPolyTypeReference LocalPlayer { get; private set; }
	
		[field: SerializeField]
		public APolyPlayerControllerBase PlayerController { get; private set; }

		[field: SerializeField]
		public List<FPolySerializableKeyValuePair<APolyPawnBase, FPolyTagReference>> PolyPawnsToSpawn { get; private set; }
	}
}
