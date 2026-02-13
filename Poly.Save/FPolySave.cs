using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Poly.Save
{
    public static class FPolySave
    {
        private static Dictionary<string, IPolySaveable> saveables = new();

        public static void Register(IPolySaveable saveable)
        {
            saveables.TryAdd(saveable.SaveID, saveable);
        }

        public static void Unregister(IPolySaveable saveable)
        {
            saveables.Remove(saveable.SaveID);
        }

        public static void SaveGame()
        {
            var saveData = new Dictionary<string, object>();

            foreach (var saveable in saveables.Values)
            {
                saveData[saveable.SaveID] = saveable.CaptureState();
            }
            
            var path = Application.persistentDataPath + "/save.dat";
            
            using var stream = new FileStream(path, FileMode.Create);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, saveData);
        }

        public static void LoadGame()
        {
            var path = Application.persistentDataPath + "/save.dat";
            if (!File.Exists(path))
            {
                return;
            }

            using var stream = new FileStream(path, FileMode.Open);
            var formatter = new BinaryFormatter();
            var saveData = formatter.Deserialize(stream) as Dictionary<string, object>;

            foreach (var kvp in saveables)
            {
                if (saveData != null && saveData.TryGetValue(kvp.Key, out var state))
                {
                    kvp.Value.RestoreState(state);
                }
            }
        }

    }
}
