using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Poly.Common;
using Poly.Events;
using Poly.Log;
using UnityEngine;

namespace Poly.GameFramework.Core
{

    public abstract class APolyWorldBase : APolyMonoSubsystemOwner<FPolyWorldSubsystem>
    {
        private FPolyGameInstanceBase gameInstance;
        private APolyPawnBase[] replacers;

        protected readonly List<APolyPlayerControllerBase> playerControllersOnMap = new();

        public string ScenePath { get; private set; }
        public FPolyEventBus WorldEventBus { get; private set; } = new();

        protected internal override void PostSubsystemInitialize(FPolyWorldSubsystem subsystem)
        {
            subsystem.SetOwner(this);
        }

        protected internal virtual Task PrepareWorldLoop(int playerIdx)
        {
            return Task.CompletedTask;
        }

        protected internal virtual void PrepareWorld(FPolyGameInstanceBase gi, string scenePath,
            List<FPolySerializableKeyValuePair<APolyPawnBase, FPolyTagReference>> pawnsToSpawn)
        {
            gameObject.hideFlags = HideFlags.HideInHierarchy;
            ScenePath = scenePath;
            gameInstance = gi;

            foreach (var s in registeredSubsystems)
            {
                s.Value.PostWorldPrepared();
            }

            foreach (var s in gameInstance.registeredSubsystems)
            {
                s.Value.PostWorldPrepared();
            }

            foreach (var s in gameInstance.localPlayer.registeredSubsystems)
            {
                s.Value.PostWorldPrepared();
            }

            InitializePolyBehaviours();
            SpawnPawnAtStartPos(pawnsToSpawn);
        }

        private void InitializePolyBehaviours()
        {
            var behaviours =
                FindObjectsByType<APolyMonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
            
            foreach (var b in behaviours)
            {
                b.InitializeComponent(this);
            }

            foreach (var b in behaviours)
            {
                b.PostInitializeComponent(this);
            }
        }

        private void SpawnPawnAtStartPos(List<FPolySerializableKeyValuePair<APolyPawnBase, FPolyTagReference>> pawnsToSpawn)
        {
            replacers = FindObjectsByType<APolyPawnBase>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
            if (replacers.Length > 0)
            {
                return;
            }

            replacers = SpawnPawnsForWorld(pawnsToSpawn);
        }

        private APolyPawnBase[] SpawnPawnsForWorld(List<FPolySerializableKeyValuePair<APolyPawnBase, FPolyTagReference>> pawnsToSpawnList)
        {
            var startPoses = FindObjectsByType<APolyPlayerStart>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
            var startPosByTag = startPoses.ToDictionary(startPos => startPos.tag);

            var spawnedPawns = new APolyPawnBase[pawnsToSpawnList.Count];
            for (var i = 0; i < pawnsToSpawnList.Count; i++)
            {
                var newPawnPrefab = pawnsToSpawnList[i].Key;
                var spawnedPrefab = SpawnObject(newPawnPrefab);
                spawnedPrefab.gameObject.SetActive(false);
                spawnedPawns[i] = spawnedPrefab;

                if (startPosByTag.TryGetValue(pawnsToSpawnList[i].Value.Tag, out var startPos))
                {
                    spawnedPrefab.transform.SetPositionAndRotation(startPos.transform.position,
                        startPos.transform.rotation);
                }
            }

            return spawnedPawns;
        }
        
        internal APolyPlayerControllerBase SpawnPlayerController(APolyPlayerControllerBase playerControllerPrefab)
        {
            var toSpawn = SpawnObject(playerControllerPrefab);
            playerControllersOnMap.Add(toSpawn);
            return toSpawn;
        }

        public void SwitchReplacer(int playerIdx, int replacerIdx)
        {
            if (replacerIdx < 0 || replacerIdx >= replacers.Length)
            {
                FPolyLog.Error("Poly.GameFramework",
                    $"There is no replacer with idx spawned in current world {replacerIdx}");
                return;
            }

            var replacer = replacers[replacerIdx];
            playerControllersOnMap[playerIdx].PossessPawn(replacer);
        }

        protected internal virtual void Invalidate()
        {
            var behaviours = FindObjectsByType<APolyMonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
            foreach (var b in behaviours)
            {
                b.DisposeComponent(this);
            }

            for (var i = replacers.Length - 1; i >= 0; i--)
            {
                Destroy(replacers[i].gameObject);
            }

            foreach (var pc in playerControllersOnMap)
            {
                pc.Dispose();
                Destroy(pc.gameObject);
            }

            playerControllersOnMap.Clear();
            Dispose();
            
            Destroy(this.gameObject);
        }


        protected internal override void Update()
        {
            base.Update();
            foreach (var pc in playerControllersOnMap)
            {
                pc.Tick(Time.deltaTime);
            }
        }

        protected internal override void FixedUpdate()
        {
            base.FixedUpdate();
            foreach (var pc in playerControllersOnMap)
            {
                pc.Tick(Time.fixedDeltaTime);
            }
        }

        protected internal override void LateUpdate()
        {
            base.LateUpdate();
            foreach (var pc in playerControllersOnMap)
            {
                pc.LateTick(Time.fixedDeltaTime);
            }
        }

        public TObject SpawnObject<TObject>(TObject prefab, Transform parent = null)
            where TObject : MonoBehaviour
        {
            var spawnedPrefab = Instantiate(prefab, parent);

            var polyMono = spawnedPrefab as APolyMonoBehaviour;
            if (polyMono == null)
            {
                return spawnedPrefab;
            }

            polyMono.InitializeComponent(this);
            polyMono.PostInitializeComponent(this);

            return spawnedPrefab;
        }

        public TObject SpawnObject<TObject>(TObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
            where TObject : MonoBehaviour
        {
            var spawnedPrefab = Instantiate(prefab, position, rotation, parent);

            var polyMono = spawnedPrefab as APolyMonoBehaviour;
            if (polyMono == null)
            {
                return spawnedPrefab;
            }

            polyMono.InitializeComponent(this);
            polyMono.PostInitializeComponent(this);

            return spawnedPrefab;
        }

        public TComponent SpawnEmptyObjectWithComponent<TComponent>(string label, Vector3 position, Quaternion rotation, Transform parent = null)
            where TComponent : MonoBehaviour
        {
            var go = SpawnEmptyObject(label);
            go.transform.SetPositionAndRotation(position, rotation);
            if (parent != null)
            {
                go.transform.SetParent(parent);
            }

            var comp = go.AddComponent<TComponent>();
            var polyMono = comp as APolyMonoBehaviour;

            if (polyMono == null)
            {
                return comp;
            }

            polyMono.InitializeComponent(this);
            polyMono.PostInitializeComponent(this);

            return comp;
        }

        public GameObject SpawnEmptyObject(string label)
        {
            var newObj = new GameObject(label);
            newObj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            return newObj;
        }

        public void DespawnObject(MonoBehaviour objectToDespawn, float timeToDespawn = 0f)
        {
            if (objectToDespawn is APolyMonoBehaviour b)
            {
                b.DisposeComponent(this);
            }

            Destroy(objectToDespawn, timeToDespawn);
        }
        
        public void DestroyObject(GameObject objectToDespawn, float timeToDespawn = 0f)
        {
            Destroy(objectToDespawn, timeToDespawn);
        }

        public APolyPlayerControllerBase GetFirstPlayerController()
        {
            return playerControllersOnMap[0];
        }

        public T GetFirstPlayerController<T>() where T : APolyPlayerControllerBase
        {
            return playerControllersOnMap[0] as T;
        }

        public APolyPlayerControllerBase GetPlayerController(int playerIdx)
        {
            return playerControllersOnMap[playerIdx];
        }

        public T GetPlayerController<T>(int playerIdx) where T : APolyPlayerControllerBase
        {
            return playerControllersOnMap[playerIdx] as T;
        }

        public FPolyGameInstanceBase GetGameInstance()
        {
            return gameInstance;
        }

        public T GetGameInstance<T>() where T : FPolyGameInstanceBase
        {
            return gameInstance as T;
        }
    }
}
