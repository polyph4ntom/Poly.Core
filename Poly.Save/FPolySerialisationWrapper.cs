using System.Collections.Generic;
using UnityEngine;

namespace Poly.Save
{
    [System.Serializable]
    public class FPolySerialisationWrapper
    {
        public List<string> keys = new();
        public List<string> jsonValues = new();

        public FPolySerialisationWrapper(Dictionary<string, object> data)
        {
            foreach (var kvp in data)
            {
                keys.Add(kvp.Key);
                jsonValues.Add(JsonUtility.ToJson(kvp.Value));
            }
        }

        public Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>();
            for (int i = 0; i < keys.Count; i++)
            {
                dict[keys[i]] = JsonUtility.FromJson<object>(jsonValues[i]);
            }

            return dict;
        }
    }
}
